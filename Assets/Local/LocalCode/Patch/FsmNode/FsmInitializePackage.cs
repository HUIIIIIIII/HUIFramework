using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LocalCode;
using LocalCode.Common;
using LocalCode.Patch;
using UnityEngine;
using UniFramework.Machine;
using YooAsset;

internal class FsmInitializePackage : IStateNode
{
    private StateMachine machine;

    void IStateNode.OnCreate(StateMachine machine)
    {
        this.machine = machine;
    }
    void IStateNode.OnEnter()
    {
        InitPackage().Forget();
    }
    void IStateNode.OnUpdate()
    {
    }
    void IStateNode.OnExit()
    {
    }
    
    private async UniTask InitPackage()
    {
        var local_setting = (GameLocalSetting)machine.GetBlackboardValue(BlackBoardKey.local_setting);
        var play_mode = local_setting.GamePlayMode;
        var package_name = (string)machine.GetBlackboardValue(BlackBoardKey.package_name);
        
        // creat package
        var package = YooAssets.TryGetPackage(package_name);
        if (package == null)
            package = YooAssets.CreatePackage(package_name);

        if(package.InitializeStatus == EOperationStatus.Succeed)
        {
            // if package is already initialized, just change state
            machine.ChangeState<FsmRequestPackageVersion>();
            return;
        }
        // in editor mode
        InitializationOperation init_operation = null;
        if (play_mode == EPlayMode.EditorSimulateMode)
        {
            var build_result = EditorSimulateModeHelper.SimulateBuild(package_name);
            var package_root = build_result.PackageRootDirectory;
            var create_params = new EditorSimulateModeParameters();
            create_params.EditorFileSystemParameters = FileSystemParameters.CreateDefaultEditorFileSystemParameters(package_root);
            init_operation = package.InitializeAsync(create_params);
        }

        // in offline mode
        if (play_mode == EPlayMode.OfflinePlayMode)
        {
            var create_params = new OfflinePlayModeParameters();
            create_params.BuildinFileSystemParameters = FileSystemParameters.CreateDefaultBuildinFileSystemParameters();
            init_operation = package.InitializeAsync(create_params);
        }

        // in online_mode
        if (play_mode == EPlayMode.HostPlayMode)
        {
            string default_host_server = GetHostServerURL();
            string fallback_host_server = GetHostServerURL();
            IRemoteServices remote_services = new RemoteServices(default_host_server, fallback_host_server);
            var create_params = new HostPlayModeParameters();
            create_params.BuildinFileSystemParameters = FileSystemParameters.CreateDefaultBuildinFileSystemParameters();
            create_params.CacheFileSystemParameters = FileSystemParameters.CreateDefaultCacheFileSystemParameters(remote_services);
            init_operation = package.InitializeAsync(create_params);
        }

        // in WebGL mode
        if (play_mode == EPlayMode.WebPlayMode)
        {
#if UNITY_WEBGL && WEIXINMINIGAME && !UNITY_EDITOR
            var create_params = new WebPlayModeParameters();
			string default_host_server = GetHostServerURL();
            string fallback_host_server = GetHostServerURL();
            string package_root = $"{WeChatWASM.WX.env.USER_DATA_PATH}/__GAME_FILE_CACHE"; //notices :  has subdirectory, need to change it
            IRemoteServices remote_services = new RemoteServices(default_host_server, fallback_host_server);
            create_params.WebServerFileSystemParameters = WechatFileSystemCreater.CreateFileSystemParameters(package_root, remote_services);
            initializationOperation = package.InitializeAsync(create_params);
#else
            var create_params = new WebPlayModeParameters();
            create_params.WebServerFileSystemParameters = FileSystemParameters.CreateDefaultWebServerFileSystemParameters();
            init_operation = package.InitializeAsync(create_params);
#endif
        }

        await init_operation;
        
        // show popup window if initialization failed
        if (init_operation.Status != EOperationStatus.Succeed)
        {
            GameLog.LogWarning($"{init_operation.Error}");
            new PatchEvent.InitializeFailed().SendMsg();
            var owner = machine.Owner as PackageLoad; 
            owner.RetryLoad();
        }
        else
        {
            machine.ChangeState<FsmRequestPackageVersion>();
        }
    }

    /// <summary>
    /// get asset server URL based on the platform
    /// </summary>
    private string GetHostServerURL()
    {
        var local_setting = (GameLocalSetting)machine.GetBlackboardValue(BlackBoardKey.local_setting);
        string host_server_ip = local_setting.DefaultHostServer;
        string app_version = "v1.0";

#if UNITY_EDITOR
        if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.Android)
            return $"{host_server_ip}/CDN/Android/{app_version}";
        else if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.iOS)
            return $"{host_server_ip}/CDN/IPhone/{app_version}";
        else if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.WebGL)
            return $"{host_server_ip}/CDN/WebGL/{app_version}";
        else
            return $"{host_server_ip}/CDN/PC/{app_version}";
#else
        if (Application.platform == RuntimePlatform.Android)
            return $"{host_server_ip}/CDN/Android/{app_version}";
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
            return $"{host_server_ip}/CDN/IPhone/{app_version}";
        else if (Application.platform == RuntimePlatform.WebGLPlayer)
            return $"{host_server_ip}/CDN/WebGL/{app_version}";
        else
            return $"{host_server_ip}/CDN/PC/{app_version}";
#endif
    }

    /// <summary>
    /// remote services interface for getting URLs
    /// </summary>
    private class RemoteServices : IRemoteServices
    {
        private readonly string default_host_server;
        private readonly string fall_back_host_server;

        public RemoteServices(string defaultHostServer, string fallBackHostServer)
        {
            default_host_server = defaultHostServer;
            fall_back_host_server = fallBackHostServer;
        }
        string IRemoteServices.GetRemoteMainURL(string fileName)
        {
            return $"{default_host_server}/{fileName}";
        }
        string IRemoteServices.GetRemoteFallbackURL(string fileName)
        {
            return $"{fall_back_host_server}/{fileName}";
        }
    }
}