using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CJumpStateBase : CMovableStatePototype
{
    public override EMovableState StateType() { return EMovableState.eJump; }

    public CJumpStateBase(CMovableBase pamMovableBase) : base(pamMovableBase)
    {

    }

    protected override void InState()
    {

    }

    protected override void updataState()
    {

    }

    protected override void OutState()
    {

    }
}
