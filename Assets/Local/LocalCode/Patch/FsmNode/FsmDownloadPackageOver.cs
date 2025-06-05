﻿using System.Collections;
using System.Collections.Generic;
using LocalCode;
using UnityEngine;
using UniFramework.Machine;

internal class FsmDownloadPackageOver : IStateNode
{
    private StateMachine machine;

    void IStateNode.OnCreate(StateMachine machine)
    {
        this.machine = machine;
    }
    void IStateNode.OnEnter()
    {
        new PatchEvent.PatchStepsChange("asset download over！").SendMsg();
        machine.ChangeState<FsmClearCacheBundle>();
    }
    void IStateNode.OnUpdate()
    {
    }
    void IStateNode.OnExit()
    {
    }
}