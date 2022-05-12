using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CWinNoPosStatePlayer : CPlayerStateBase
{
    public override EMovableState StateType() { return EMovableState.eWin; }

    public CWinNoPosStatePlayer(CMovableBase pamMovableBase) : base(pamMovableBase)
    {

    }

    protected override void InState()
    {
        //m_MyPlayerMemoryShare.m_MyPlayer.AllColliderEnabled(false);
        m_MyGameManager.SetState(CGameManager.EState.eWinUI);
    }

    protected override void updataState()
    {
        base.updataState();
    }

    protected override void OutState()
    {

    }
}
