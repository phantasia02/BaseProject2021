using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MYgame.Scripts.Scenes.GameScenes.Data;

public abstract class CPlayerStateBase : CMovableStatePototype
{
    protected CPlayerMemoryShare m_MyPlayerMemoryShare = null;


    public CPlayerStateBase(CMovableBase pamMovableBase) : base(pamMovableBase)
    {
        m_MyPlayerMemoryShare = (CPlayerMemoryShare)m_MyMemoryShare;
    }

    protected override void InState()
    {
    }

    protected override void updataState()
    {
    }

    //public override void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.layer == (int)StaticGlobalDel.ELayerIndex.eWater)
    //        SetHitWater(collision);

    //}


    public void UpdateSpeed()
    {
        if (m_MyPlayerMemoryShare.m_TotleSpeed.Value != m_MyPlayerMemoryShare.m_TargetTotleSpeed)
        {
           // m_MyMemoryShare.m_TotleSpeed.Value = Mathf.MoveTowards(m_MyPlayerMemoryShare.m_TotleSpeed.Value, m_MyPlayerMemoryShare.m_TargetTotleSpeed, m_MyPlayerMemoryShare.m_AddSpeedSecond * Time.deltaTime);
            m_MyMemoryShare.m_TotleSpeed.Value = Mathf.Lerp(m_MyPlayerMemoryShare.m_TotleSpeed.Value, m_MyPlayerMemoryShare.m_TargetTotleSpeed, 5.0f * Time.deltaTime);

            if (Mathf.Abs(m_MyPlayerMemoryShare.m_TotleSpeed.Value - m_MyPlayerMemoryShare.m_TargetTotleSpeed) < 0.1f)
                m_MyMemoryShare.m_TotleSpeed.Value = m_MyMemoryShare.m_TargetTotleSpeed;
        }
    }



    
}
