using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CReadyPlayStatePlayer : CPlayerStateBase
{
    public override EMovableState StateType() { return EMovableState.eWait; }

    public CReadyPlayStatePlayer(CMovableBase pamMovableBase) : base(pamMovableBase)
    {

    }

    protected override void InState()
    {

    }

    protected override void updataState()
    {
        base.updataState();
    }

    protected override void OutState()
    {

    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.tag != StaticGlobalDel.TagPlayTouchObject)
            return;

        ChangState(EMovableState.eJump);
    }

  
}
