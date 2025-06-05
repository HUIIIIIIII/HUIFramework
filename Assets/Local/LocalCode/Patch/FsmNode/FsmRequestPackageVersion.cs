using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LocalCode;
using LocalCode.Common;
using LocalCode.Patch;
using UnityEngine;
using UniFramework.Machine;
using YooAsset;

internal class FsmRequestPackageVersion : IStateNode
{
    private StateMachine machine;

    void IStateNode.OnCreate(StateMachine machine)
    {
        this.machine = machine;
    }

    void IStateNode.OnEnter()
    {
        new PatchEvent.PatchStepsChange("request package version !").SendMsg();
        UpdatePackageVersion().Forget();
    }

    void IStateNode.OnUpdate()
    {
    }

    void IStateNode.OnExit()
    {
    }

    private async UniTask UpdatePackageVersion()
    {
        var package_name = (string)machine.GetBlackboardValue(BlackBoardKey.package_name);
        var package = YooAssets.GetPackage(package_name);
        var operation = package.RequestPackageVersionAsync();
        await operation;

        if (operation.Status != EOperationStatus.Succeed)
        {
            GameLog.LogWarning(operation.Error);
            new PatchEvent.PackageVersionRequestFailed().SendMsg();
            var owner = machine.Owner as PackageLoad;
            owner.RetryLoad();
        }
        else
        {
            GameLog.Log($"Request package version : {operation.PackageVersion}");
            machine.SetBlackboardValue("PackageVersion", operation.PackageVersion);
            machine.ChangeState<FsmUpdatePackageManifest>();
        }
    }
}