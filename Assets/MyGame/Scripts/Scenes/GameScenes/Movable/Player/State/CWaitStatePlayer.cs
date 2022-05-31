using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class CWaitStatePlayer : CPlayerStateBase
{
    public override EMovableState StateType() { return EMovableState.eWait; }

    public CWaitStatePlayer(CMovableBase pamMovableBase) : base(pamMovableBase)
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

    //public override void MouseDown()
    //{
    //    ChangState(EMovableState.eDrag);
        
    //}

    //public override void MouseDrag()
    //{
    //    Ray ray = Camera.main.ScreenPointToRay(m_MyPlayerMemoryShare.m_CurMouseDownPos);

    //    //if (Physics.Raycast(ray, out RaycastHit hit, 128))
    //    //{
    //    //  //  Debug.Log(hit.collider.name);
    //    //    Debug.Log(hit.collider.gameObject.layer);
    //    //}


    //    RaycastHit[] lTempRaycastHit = Physics.RaycastAll(ray, 100.0f, StaticGlobalDel.g_PlayerMask);

    //  //  Debug.Log(" ==================== ");
    //    foreach (var item in lTempRaycastHit)
    //    {
    //        Debug.Log(item.collider.gameObject.layer);
    //        //  m_MyPlayerMemoryShare.m_EndPos = item.point;
    //        //  break;
    //    }
    //}

}
