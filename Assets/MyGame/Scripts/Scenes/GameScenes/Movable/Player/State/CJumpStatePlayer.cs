using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CJumpStatePlayer : CPlayerStateBase
{
    public override EMovableState StateType() { return EMovableState.eJump; }

    protected Vector3 m_RotateLocalAxis = Vector3.zero;
    protected float m_Dis = 1.0f;
    protected bool m_RotationLoop = true;
    

    public CJumpStatePlayer(CMovableBase pamMovableBase) : base(pamMovableBase)
    {

    }

    protected override void InState()
    {
        m_Dis = Vector3.Distance(m_MyPlayerMemoryShare.m_MyTransform.position, m_MyPlayerMemoryShare.m_EndPos);

        Vector3 lTempTorque = m_MyPlayerMemoryShare.m_MyTransform.position - m_MyPlayerMemoryShare.m_CentralHighPos;
        // Vector3 lTempExplosionPos = m_MyPlayerMemoryShare.m_MyTransform.position + (lTempTorque.normalized * 3.0f);
        //m_RotateLocalAxis = Vector3.Cross(m_MyPlayerMemoryShare.m_CentralHighPos - m_MyPlayerMemoryShare.m_MyTransform.position, m_MyPlayerMemoryShare.m_EndPos - m_MyPlayerMemoryShare.m_MyTransform.position);
        m_RotateLocalAxis = Vector3.Cross(m_MyPlayerMemoryShare.m_AddForce, Vector3.up);
        m_RotateLocalAxis.Normalize();

        //m_MyPlayerMemoryShare.m_MyRigidbody.maxAngularVelocity = 7.0f;
        m_MyPlayerMemoryShare.m_MyRigidbody.useGravity = true;
        m_MyPlayerMemoryShare.m_MyRigidbody.isKinematic = false;
        m_MyPlayerMemoryShare.m_MyRigidbody.velocity = m_MyPlayerMemoryShare.m_AddForce;
        //m_MyPlayerMemoryShare.m_MyRigidbody.angularVelocity = m_MyPlayerMemoryShare.m_AddForce;
        //m_MyPlayerMemoryShare.m_MyRigidbody.inertiaTensorRotation = Quaternion.AngleAxis(m_Dis * 2000.0f, m_RotateLocalAxis) * m_MyPlayerMemoryShare.m_AllObj.rotation;
        //  m_MyPlayerMemoryShare.m_MyRigidbody.AddExplosionForce(100.0f, lTempExplosionPos, 10.0f, 3.0F);
        //  m_MyPlayerMemoryShare.m_MyRigidbody.AddTorque(lTempTorque * 10.0f,  ForceMode.Acceleration);

        // m_MyPlayerMemoryShare.m_MyPlayer.transform.DOPath(m_MyPlayerMemoryShare.m_BezierPath, 2.8f).SetLookAt(0).SetEase(Ease.Linear);

        //Tweener lTempTweener = m_MyPlayerMemoryShare.m_MyPlayer.transform.DOPath(m_MyPlayerMemoryShare.m_BezierPath, m_Dis * 0.1f).SetEase(Ease.Linear);
        //lTempTweener.SetId(m_MyPlayerMemoryShare.m_MyPlayer.transform);

        //lTempTweener.onComplete = () => {
        //    int MaxIndex = m_MyPlayerMemoryShare.m_BezierPath.Length - 1;
        //    Vector3 lTempEndVelocity = m_MyPlayerMemoryShare.m_BezierPath[MaxIndex] - m_MyPlayerMemoryShare.m_BezierPath[MaxIndex - 1];
        //    m_MyPlayerMemoryShare.m_MyRigidbody.constraints = RigidbodyConstraints.None;
        //    m_MyPlayerMemoryShare.m_MyRigidbody.velocity = lTempEndVelocity * 5.0f;
        //    m_MyPlayerMemoryShare.m_MyRigidbody.ResetInertiaTensor();
        //    m_MyPlayerMemoryShare.m_MyRigidbody.rotation = Quaternion.AngleAxis(Time.deltaTime * m_Dis * 20.0f, m_RotateLocalAxis) * m_MyPlayerMemoryShare.m_MyRigidbody.rotation;
        //    //m_MyPlayerMemoryShare.m_MyRigidbody.angularVelocity = lTempEndVelocity;
        //    ChangState(EMovableState.eWait);
        //};
        m_RotationLoop = true;
    }

    protected override void updataState()
    {
        base.updataState();

        if (m_RotationLoop)
            m_MyPlayerMemoryShare.m_MyRigidbody.rotation = Quaternion.AngleAxis(Time.deltaTime * m_Dis * -20.0f, m_RotateLocalAxis) * m_MyPlayerMemoryShare.m_MyRigidbody.rotation;

        //m_MyPlayerMemoryShare.m_AllObj.RotateAroundLocal(m_RotateLocalAxis, Time.deltaTime * m_Dis * 0.5f);
        // m_MyPlayerMemoryShare.m_MyTransform.rotation = Quaternion.AngleAxis(Time.deltaTime * m_Dis * 20.0f, m_RotateLocalAxis) * m_MyPlayerMemoryShare.m_AllObj.rotation;
        if (m_StateTime >= 5.0f)
            ChangState(EMovableState.eDeath);
    }

    protected override void OutState()
    {

    }

    public override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == (int)StaticGlobalDel.ELayerIndex.eFloor)
        {
            if (m_StateTime < 0.1f)
                return;

            ChangState(EMovableState.eDeath);
        }

        if (m_StateTime > 0.1f)
            m_RotationLoop = false;
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.tag != StaticGlobalDel.TagWin)
            return;

        ChangState(EMovableState.eWin);
    }

    public override void OnTriggerStay(Collider other)
    {
        CChangeDirTag lTempCChangeDirTag = other.gameObject.GetComponent<CChangeDirTag>();

        if (lTempCChangeDirTag == null)
            return;

        m_MyPlayerMemoryShare.m_MyRigidbody.velocity += lTempCChangeDirTag.Force;
    }
}
