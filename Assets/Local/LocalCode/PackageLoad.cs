using System.Collections;
using System.Collections.Generic;
using LocalCode;
using LocalCode.Common;
using LocalCode.Patch;
using UniFramework.Event;
using UniFramework.Machine;
using UnityEngine;
using YooAsset;

public class PackageLoad : GameAsyncOperation
{
     private enum ESteps
    {
        None,
        Update,
        Done,
    }
    private readonly EventGroup event_group = new EventGroup();
    private readonly StateMachine machine;
    private readonly string packge_name;
    private ESteps _steps = ESteps.None;
    private int retry_count = 0;
    private readonly GameLocalSetting local_setting;

    public PackageLoad(string packge_name,GameLocalSetting local_setting)
    {
        this.packge_name = packge_name;
        this.local_setting = local_setting;
        
        // create state machine
        machine = new StateMachine(this);
        machine.AddNode<FsmInitializePackage>();
        machine.AddNode<FsmRequestPackageVersion>();
        machine.AddNode<FsmUpdatePackageManifest>();
        machine.AddNode<FsmCreateDownloader>();
        machine.AddNode<FsmDownloadPackageFiles>();
        machine.AddNode<FsmDownloadPackageOver>();
        machine.AddNode<FsmClearCacheBundle>();
        machine.AddNode<FsmStartGame>();

        machine.SetBlackboardValue(BlackBoardKey.package_name, packge_name);
        machine.SetBlackboardValue(BlackBoardKey.local_setting, local_setting);
    }
    protected override void OnStart()
    {
        _steps = ESteps.Update;
        machine.Run<FsmInitializePackage>();
    }
    protected override void OnUpdate()
    {
        if (_steps == ESteps.None || _steps == ESteps.Done)
            return;

        if (_steps == ESteps.Update)
        {
            machine.Update();
        }
    }
    protected override void OnAbort()
    {
    }

    public void SetFinish()
    {
        _steps = ESteps.Done;
        Status = EOperationStatus.Succeed;
    }

    public void RetryLoad()
    {
        if (retry_count >= local_setting?.MaxDownloadRetryCount)
        {
            return;
        }
        retry_count++;
        machine.ChangeState<FsmInitializePackage>();
    }
}
