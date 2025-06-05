using System.Collections;
using System.Collections.Generic;
using LocalCode;
using UnityEngine;
using UniFramework.Machine;

internal class FsmStartGame : IStateNode
{
    private PackageLoad _owner;

    void IStateNode.OnCreate(StateMachine machine)
    {
        _owner = machine.Owner as PackageLoad;
    }
    void IStateNode.OnEnter()
    {
        new PatchEvent.PatchStepsChange("Enter Game！").SendMsg();
        _owner.SetFinish();
    }
    void IStateNode.OnUpdate()
    {
    }
    void IStateNode.OnExit()
    {
    }
}