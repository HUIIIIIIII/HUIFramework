using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LocalCode;
using LocalCode.Common;
using LocalCode.Patch;
using UnityEngine;
using UniFramework.Machine;
using YooAsset;

public class FsmUpdatePackageManifest : IStateNode
{
    private StateMachine machine;

    void IStateNode.OnCreate(StateMachine machine)
    {
        this.machine = machine;
    }
    void IStateNode.OnEnter()
    {
       new PatchEvent.PatchStepsChange("refresh package manifest！").SendMsg();
    }
    void IStateNode.OnUpdate()
    {
    }
    void IStateNode.OnExit()
    {
    }

    private async UniTask UpdateManifest()
    {
        var package_name = (string)machine.GetBlackboardValue(BlackBoardKey.package_name);
        var package_version = (string)machine.GetBlackboardValue(BlackBoardKey.package_version);
        var package = YooAssets.GetPackage(package_name);
        var operation = package.UpdatePackageManifestAsync(package_version);
        await operation;

        if (operation.Status != EOperationStatus.Succeed)
        {
            GameLog.LogWarning(operation.Error);
            new PatchEvent.PackageManifestUpdateFailed().SendMsg();
            var owner = machine.Owner as PackageLoad; 
            owner.RetryLoad();
            return;
        }
        else
        {
            machine.ChangeState<FsmCreateDownloader>();
        }
    }
}