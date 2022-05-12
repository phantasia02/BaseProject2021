using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CDragStatePlayer : CPlayerStateBase
{
    public override EMovableState StateType() { return EMovableState.eDrag; }
    Vector3 m_BuffForceNormal = Vector3.zero;
    Vector3 m_BuffCameraForward = Vector3.zero;
    Vector3 m_BuffCameraRight = Vector3.zero;

    public CDragStatePlayer(CMovableBase pamMovableBase) : base(pamMovableBase)
    {

    }

    protected override void InState()
    {
        m_MyPlayerMemoryShare.m_LinePath.gameObject.SetActive(true);
        m_MyPlayerMemoryShare.m_LinePath.positionCount = 0;
        m_MyPlayerMemoryShare.m_MyPlayer.ObserverPlayStartEvent().OnNext(UniRx.Unit.Default);

        StaticGlobalDel.ObjListChangLayer(m_MyPlayerMemoryShare.m_TargetListRenderObj, StaticGlobalDel.ELayerIndex.eRenderFlashModelShow);

        m_BuffCameraForward = m_MyGameManager.MainCamera.gameObject.transform.forward;
        m_BuffCameraForward.y = 0.0f;
        m_BuffCameraForward.Normalize();

        m_BuffCameraRight = m_MyGameManager.MainCamera.gameObject.transform.right;
        m_BuffCameraRight.y = 0.0f;
        m_BuffCameraRight.Normalize();
    }

    protected override void updataState()
    {
        base.updataState();
    }

    protected override void OutState()
    {
        m_MyPlayerMemoryShare.m_LinePath.gameObject.SetActive(false);
        StaticGlobalDel.ObjListChangLayer(m_MyPlayerMemoryShare.m_TargetListRenderObj, StaticGlobalDel.ELayerIndex.ePlayer);
    }


    public override void MouseDrag()
    {
        Vector3 lTempEndPos = Vector3.zero;
        Vector3 lTempV3 = m_MyPlayerMemoryShare.m_CurMouseDownPos - m_MyPlayerMemoryShare.m_DownMouseDownPos;
        //Debug.Log($"======================================");
        //Debug.Log($"lTempV3 = {lTempV3}");
        

        lTempV3 =   (m_BuffCameraForward    * (lTempV3.y / Screen.height)   * m_MyPlayerMemoryShare.m_CurStageData.AddForce.z) +
                    (m_BuffCameraRight      * (lTempV3.x / Screen.width)    * m_MyPlayerMemoryShare.m_CurStageData.AddForce.x) +
                    (Vector3.up             * (lTempV3.y / Screen.height)   * m_MyPlayerMemoryShare.m_CurStageData.AddForce.y);


        Vector3 lTempForce = lTempV3;
        lTempForce.y = 0.0f;

        if (Mathf.Abs(lTempForce.sqrMagnitude) <= Mathf.Epsilon)
            return;
            
        m_BuffForceNormal = lTempForce.normalized;
        m_MyPlayerMemoryShare.m_AddForce = lTempV3;

        //lTempV3 = ((lTempV3.x / Screen.width) * m_MyPlayerMemoryShare.m_MyTransform.right * 40.0f) +
        //    ((lTempV3.y / Screen.height) * m_MyPlayerMemoryShare.m_MyTransform.forward * 300.0f) +
        //    ((lTempV3.y / Screen.height) * m_MyPlayerMemoryShare.m_MyTransform.up * 600.0f);

        //Transform lTempCameraTransform = m_MyGameManager.MainCamera.transform;

        //lTempEndPos = m_MyPlayerMemoryShare.m_MyTransform.position + (lTempCameraTransform.forward * lTempV3.z) + (lTempCameraTransform.right * lTempV3.x);
        //m_MyPlayerMemoryShare.m_CentralHighPos = ((m_MyPlayerMemoryShare.m_MyTransform.position + lTempEndPos) * 0.5f) + (lTempCameraTransform.up * Mathf.Abs(lTempV3.z * 1.0f));

        ////Debug.Log($"m_MyPlayerMemoryShare.m_CentralHighPos = {m_MyPlayerMemoryShare.m_CentralHighPos}");
        ////Debug.Log($"m_EndPos = {m_MyPlayerMemoryShare.m_EndPos}");

        //Ray ray = new Ray(m_MyPlayerMemoryShare.m_CentralHighPos, (lTempEndPos - m_MyPlayerMemoryShare.m_CentralHighPos) * 100.0f);
        //RaycastHit[] lTempRaycastHit = Physics.RaycastAll(ray, 100.0f, StaticGlobalDel.g_FloorMask);

        ////Debug.Log(" ==================== ");
        //foreach (var item in lTempRaycastHit)
        //{
        //    m_MyPlayerMemoryShare.m_EndPos = item.point;
        //    break;
        //}

        //for (int i = 0; i < m_MyPlayerMemoryShare.m_BezierPath.Length; i++)
        //{
        //    var t = i / (float)(m_MyPlayerMemoryShare.m_BezierPath.Length + 2);
        //    m_MyPlayerMemoryShare.m_BezierPath[i] = StaticGlobalDel.GetBezierPoint(t, 
        //        m_MyPlayerMemoryShare.m_MyTransform.position, m_MyPlayerMemoryShare.m_CentralHighPos, m_MyPlayerMemoryShare.m_EndPos);
        //}

        //m_MyPlayerMemoryShare.m_LinePath.positionCount = m_MyPlayerMemoryShare.m_BezierPath.Length;
        //m_MyPlayerMemoryShare.m_LinePath.SetPositions(m_MyPlayerMemoryShare.m_BezierPath);

        // m_MyGameManager.ShowPath(m_MyPlayerMemoryShare.m_AddForce, m_MyPlayerMemoryShare.m_MyRigidbody);

        GameObject predictionBall = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        SceneManager.MoveGameObjectToScene(predictionBall, m_MyGameManager.scenePrediction);
        predictionBall.transform.position = m_MyPlayerMemoryShare.m_MyRigidbody.gameObject.transform.position;
        predictionBall.transform.rotation = m_MyPlayerMemoryShare.m_MyRigidbody.gameObject.transform.rotation;
       // predictionBall.transform.localScale = Vector3.one * 3.0f;

        Rigidbody lTempRigidbody = predictionBall.AddComponent<Rigidbody>();
        lTempRigidbody.mass = m_MyPlayerMemoryShare.m_MyRigidbody.mass;
        lTempRigidbody.velocity = m_MyPlayerMemoryShare.m_AddForce;

        //m_MyPlayerMemoryShare.m_LinePath.positionCount = m_MyPlayerMemoryShare.m_BezierPath.Length;
        //m_MyPlayerMemoryShare.m_LinePath.SetPositions(m_MyPlayerMemoryShare.m_BezierPath);
        m_MyPlayerMemoryShare.m_LinePath.positionCount = 0;

        List<Vector3> lTempAllPathPoint = new List<Vector3>();
        const float SimulateTime = 2.0f;
        //const float CmaxTime = 2.5f;
        float lTempCutTime = 0.0f;

        //  Debug.Log($"==============================================");
        while (lTempCutTime < m_MyPlayerMemoryShare.m_CurStageData.PredictionTime)
        {
            m_MyGameManager.scenePredictionPhysics.Simulate(Time.fixedDeltaTime * SimulateTime);
            lTempCutTime += Time.fixedDeltaTime * SimulateTime;

            //  Debug.Log($"predictionBall.transform.position = {predictionBall.transform.position}");
            lTempAllPathPoint.Add(predictionBall.transform.position);

        }

        m_MyPlayerMemoryShare.m_LinePath.positionCount = lTempAllPathPoint.Count;
        m_MyPlayerMemoryShare.m_LinePath.SetPositions(lTempAllPathPoint.ToArray());

        GameObject.Destroy(predictionBall);

        if (m_MyPlayerMemoryShare.m_MyPlayer.UpdateObjDir != null)
            m_MyPlayerMemoryShare.m_MyPlayer.UpdateObjDir.forward = -m_BuffForceNormal;
    }

    public override void MouseUp()
    {
        if (m_MyPlayerMemoryShare.m_MyPlayer.PlayAnimaton(true))
        {
            //m_MyPlayerMemoryShare.m_MyPlayer.UpdateObjDir.forward = -m_BuffForceNormal;
            m_MyPlayerMemoryShare.m_MyPlayer.SetChangState(EMovableState.eWait, 1);
        }
        else
            ChangState(EMovableState.eJump);
    }
}
