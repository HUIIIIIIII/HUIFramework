using System.Collections;
using System.Collections.Generic;
using LocalCode;
using LocalCode.Common;
using LocalCode.Patch;
using UnityEngine;
using UniFramework.Machine;
using YooAsset;

public class FsmCreateDownloader : IStateNode
{
    private StateMachine machine;

    void IStateNode.OnCreate(StateMachine machine)
    {
        this.machine = machine;
    }
    void IStateNode.OnEnter()
    {
        CreateDownloader();
    }
    void IStateNode.OnUpdate()
    {
    }
    void IStateNode.OnExit()
    {
    }

    void CreateDownloader()
    {
        var package_name = (string)machine.GetBlackboardValue(BlackBoardKey.package_name);
        var package = YooAssets.GetPackage(package_name);
        int downloading_max_num = 10;
        int failed_try_again = 3;
        var downloader = package.CreateResourceDownloader(downloading_max_num, failed_try_again);
        machine.SetBlackboardValue(BlackBoardKey.downloadler, downloader);

        if (downloader.TotalDownloadCount == 0)
        {
            GameLog.Log("Not found any download files !");
            machine.ChangeState<FsmStartGame>();
        }
        else
        {
            // found new update files, suspend the flow system
            // notices: Developers need to check disk space before downloading
            int total_download_count = downloader.TotalDownloadCount;
            long total_download_bytes = downloader.TotalDownloadBytes;
            new PatchEvent.FoundUpdateFiles(total_download_count, total_download_bytes).SendMsg();
        }
    }
}