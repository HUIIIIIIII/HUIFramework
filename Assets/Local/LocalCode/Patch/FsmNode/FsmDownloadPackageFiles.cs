using System.Collections;
using Cysharp.Threading.Tasks;
using LocalCode;
using LocalCode.Patch;
using UnityEngine;
using UniFramework.Machine;
using YooAsset;

public class FsmDownloadPackageFiles : IStateNode
{
    private StateMachine machine;

    void IStateNode.OnCreate(StateMachine machine)
    {
        this.machine = machine;
    }

    void IStateNode.OnEnter()
    {
        BeginDownloadAsync().Forget();
    }

    void IStateNode.OnUpdate()
    {
    }

    void IStateNode.OnExit()
    {
    }

    private async UniTask BeginDownloadAsync()
    {
        var downloader = (ResourceDownloaderOperation)machine.GetBlackboardValue(BlackBoardKey.downloadler);
        downloader.DownloadErrorCallback = ( data) =>
        {
            new PatchEvent.WebFileDownloadFailed(data.FileName, data.ErrorInfo).SendMsg();
            var owner = machine.Owner as PackageLoad; 
            owner.RetryLoad();
        };
        downloader.DownloadUpdateCallback = ( data) =>
        {
            new PatchEvent.DownloadUpdate(data.CurrentDownloadCount, data.TotalDownloadCount,data.CurrentDownloadBytes, data.TotalDownloadBytes).SendMsg();
        };
        downloader.BeginDownload();
        await downloader;
        //detect download result
        if (downloader.Status != EOperationStatus.Succeed)
            return;
        machine.ChangeState<FsmDownloadPackageOver>();
    }
}