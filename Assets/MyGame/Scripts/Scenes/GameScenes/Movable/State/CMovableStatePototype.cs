﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CMovableStateData{}

public abstract class CMovableStatePototype : CExternalOBJBase
{
    public enum EMovableState
    {
        eNull       = 0,
        eWait       = 1,
        eDrag       = 2,
        eMove       = 3,
        eAtk        = 4,
        eJump       = 5,
        eJumpDown   = 6,
        eHit        = 7,
        eWin        = 8,
        eDeath      = 9,
        eFinish     = 10,
        eFlee       = 11,
        eMax
    }

    public virtual int Priority => 0;
    abstract public EMovableState StateType();

    protected float m_StateTime = 0.0f;
    protected float m_StateUnscaledTime = 0.0f;
    protected int m_StateCount = 0;
    protected float m_OldStateTime = 0.0f;
    protected float m_OldStateUnscaledTime = 0.0f;
    protected int m_OldStateCount = 0;

    protected Vector3 m_v3DownPos = Vector3.zero;
    protected bool m_bDownOKPos = false;

    public CMovableStatePototype(CMovableBase pamMovableBase) : base(pamMovableBase)
    {
    }

    public void ClearTime()
    {
       m_OldStateTime          = m_StateTime         = 0.0f;
       m_OldStateUnscaledTime  = m_StateUnscaledTime = 0.0f;
       m_OldStateCount         = m_StateCount        = 0;
    }

    public void InMovableState()
    {
        InState();
        ClearTime();
    }

    public void updataMovableState()
    {
        m_OldStateTime          = m_StateTime;
        m_OldStateUnscaledTime  = m_StateUnscaledTime;
        m_OldStateCount         = m_StateCount;

        m_StateTime         += Time.deltaTime;
        m_StateUnscaledTime += Time.unscaledDeltaTime;
        m_StateCount++;
        updataState();
    }

    public void ChangState(EMovableState state){m_MyMovable.SetChangState(state);}

    public void FixedupdataMovableState()
    {
        FixedupdataState();
    }

    public virtual void UpdateOriginalAnimation()
    {

    }

    public void OutMovableState()
    {
        OutState();
    }

    public bool MomentinTime(float time){return m_OldStateTime < time && m_StateTime >= time;}

    public EMovableState RandomState(List<CMovableStatePototype.EMovableState> RandomList)
    {
        if (RandomList == null)
            return EMovableState.eMax;

        return RandomList[Random.Range(0, RandomList.Count)];
    }

    protected virtual void InState() {}

    protected virtual void updataState(){}
    protected virtual void FixedupdataState(){}

    public virtual void LateUpdate(){}

    protected virtual void OutState(){}

    public virtual void Input(RaycastHit hit){}

    public virtual void OnTriggerEnter(Collider other){}

    public virtual void OnTriggerExit(Collider other){}

    public virtual void OnTriggerStay(Collider other){}

    public virtual void OnCollisionEnter(Collision collision){}

    public virtual void MouseDown(){}
    public virtual void MouseDrag(){}
    public virtual void MouseUp(){}

    public void UseGravityRigidbody(bool useGravity)
    {
        if (m_MyMemoryShare.m_MyRigidbody == null)
            return;
        
        m_MyMemoryShare.m_MyRigidbody.useGravity = useGravity;
        m_MyMemoryShare.m_MyRigidbody.isKinematic = !useGravity;
    }

    public void UseTirgger(bool setuseTirgger)
    {
        if (m_MyMemoryShare.m_MyAllCollider == null)
            return;

        foreach (var item in m_MyMemoryShare.m_MyAllCollider)
            item.isTrigger = setuseTirgger;
    }

    public bool FloatingToFloorChack()
    {
        return false;
    }

}
