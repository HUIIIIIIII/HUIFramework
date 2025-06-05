using System.Collections;
using System.Collections.Generic;
using LocalCode;
using LocalCode.Patch;
using UnityEngine;
using UniFramework.Machine;
using YooAsset;

internal class FsmClearCacheBundle : IStateNode
{
    private StateMachine machine;

    void IStateNode.OnCreate(StateMachine machine)
    {
        this.machine = machine;
    }
    void IStateNode.OnEnter()
    {
        new PatchEvent.PatchStepsChange("clear unused cache files！").SendMsg();
        var package_name = (string)machine.GetBlackboardValue(BlackBoardKey.package_name);
        var package = YooAssets.GetPackage(package_name);
        var operation = package.ClearCacheFilesAsync(EFileClearMode.ClearUnusedBundleFiles);
        operation.Completed += OnOperationCompleted;
    }
    void IStateNode.OnUpdate()
    {
    }
    void IStateNode.OnExit()
    {
    }

    private void OnOperationCompleted(YooAsset.AsyncOperationBase obj)
    {
        machine.ChangeState<FsmStartGame>();
    }
}