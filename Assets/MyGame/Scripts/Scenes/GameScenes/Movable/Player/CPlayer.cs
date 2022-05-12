using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
using UniRx;
using MYgame.Scripts.Scenes.GameScenes.Data;


public class DataPathNode
{
    public DataPathNode(Vector3 pos)
    {
        m_Postion = pos;
    }

    public Vector3 m_Postion = Vector3.zero;
}

/// <summary>
/// Player Memory Share Data
/// </summary>
public class CPlayerMemoryShare : CMemoryShareBase
{
    public CPlayer                              m_MyPlayer                  = null;
    public bool                                 m_bDown                     = false;
    public Vector3                              m_OldMouseDownPos           = Vector3.zero;
    public Vector3                              m_CurMouseDownPos           = Vector3.zero;
    public Vector3                              m_DownMouseDownPos          = Vector3.zero;
    public float                                m_DownTime                  = -1.0f;

    public Vector3                              m_CtrlSkillBuffDir          = Vector3.zero;

    public UniRx.ReactiveProperty<float>        m_AnimationVal              = new ReactiveProperty<float>(0.5f);
    public float                                m_valSpeed                  = 1700.0f;

    public StageData                            m_CurStageData              = null;

    public Transform                            m_StartePos                 = null;
    public Vector3                              m_EndPos                    = Vector3.zero;
    public Vector3                              m_CentralHighPos            = Vector3.zero;
    public Vector3[]                            m_BezierPath                = new Vector3[20];
    public Vector3                              m_AddForce                  = Vector3.zero;

    public Vector3                              m_HitWaterPoint             = Vector3.zero;
    public Transform                            m_AllObj                    = null;



    public CDataAllSkill                        m_AllSkill                  = new CDataAllSkill();
    public UniRx.Subject<UniRx.Unit>            m_FritPlay                  = new UniRx.Subject<UniRx.Unit>();

    public LineRenderer                         m_LinePath                  = null;
    public List<GameObject>                     m_PlayerListRenderObj       = null;
    public List<GameObject>                     m_TargetListRenderObj       = null;
};

/// <summary>
/// Player Actor
/// </summary>
public class CPlayer : CMovableBase
{
    public override EMovableType MyMovableType() { return EMovableType.ePlayer; }
    public override EObjType ObjType() { return EObjType.ePlayer; }

    protected float m_MaxMoveDirSize = 5.0f;
    public float MaxMoveDirSize => m_MaxMoveDirSize;

    protected CPlayerMemoryShare m_MyPlayerMemoryShare = null;

    // ==================== SerializeField ===========================================

    [SerializeField] protected GameObject m_PlayDragToACT = null;
    [SerializeField] protected Transform m_UpdateObjDir = null;
    public Transform UpdateObjDir => m_UpdateObjDir;

    [SerializeField] protected List<GameObject> m_PlayerListRenderObj = null;
    [SerializeField] protected List<GameObject> m_TargetListRenderObj = null;

    // ==================== SerializeField ===========================================



    public float AnimationVal
    {
        set {
                float lTempValue = Mathf.Clamp(value, 0.0f, 1.0f);
                m_MyPlayerMemoryShare.m_AnimationVal.Value = lTempValue;
            }
        get { return m_MyPlayerMemoryShare.m_AnimationVal.Value; }
    }

    public Transform StartePos
    {
        set => m_MyPlayerMemoryShare.m_StartePos = value;
        get => m_MyPlayerMemoryShare.m_StartePos;
    }

    //public float SkillRadius
    //{
    //    set => m_MyPlayerMemoryShare.m_SkillRadius = value;
    //    get => m_MyPlayerMemoryShare.m_SkillRadius;
    //}

    protected Vector3 m_OldMouseDragDir = Vector3.zero;

    public override float DefSpeed { get { return m_MyPlayerMemoryShare.m_valSpeed; } }
    public float ValSpeed
    {
        set => m_MyPlayerMemoryShare.m_valSpeed = value;
        get => m_MyPlayerMemoryShare.m_valSpeed;
    }


    protected override void AddInitState()
    {
        m_AllState[(int)StaticGlobalDel.EMovableState.eWait].AllThisState.Add(new CWaitStatePlayer(this));
        m_AllState[(int)StaticGlobalDel.EMovableState.eWait].AllThisState.Add(new CReadyPlayStatePlayer(this));


        m_AllState[(int)StaticGlobalDel.EMovableState.eDrag].AllThisState.Add(new CDragStatePlayer(this));
        m_AllState[(int)StaticGlobalDel.EMovableState.eJump].AllThisState.Add(new CJumpStatePlayer(this));


        if (m_MyGameManager.CurStageData.WinMoveWinPos)
            m_AllState[(int)StaticGlobalDel.EMovableState.eWin].AllThisState.Add(new CWinStatePlayer(this));
        else
            m_AllState[(int)StaticGlobalDel.EMovableState.eWin].AllThisState.Add(new CWinNoPosStatePlayer(this));

        m_AllState[(int)StaticGlobalDel.EMovableState.eDeath].AllThisState.Add(new CDeathStatePlayer(this));
    }

    protected override void CreateMemoryShare()
    {
        m_MyPlayerMemoryShare = new CPlayerMemoryShare();
        m_MyMemoryShare = m_MyPlayerMemoryShare;

        m_MyPlayerMemoryShare.m_MyPlayer = this;
        m_MyPlayerMemoryShare.m_CurStageData        = m_MyGameManager.CurStageData;
        m_MyPlayerMemoryShare.m_LinePath            = this.GetComponentInChildren<LineRenderer>();
        m_MyPlayerMemoryShare.m_AllObj              = this.transform.Find("AllObj");
        m_MyPlayerMemoryShare.m_PlayerListRenderObj = m_PlayerListRenderObj;
        m_MyPlayerMemoryShare.m_TargetListRenderObj = m_TargetListRenderObj;

        base.CreateMemoryShare();

        m_MaxMoveDirSize = Screen.width > Screen.height ? (float)Screen.width : (float)Screen.height;
        m_MaxMoveDirSize = m_MaxMoveDirSize / 5.0f;

        this.transform.position = m_MyGameManager.StartPosition.position;
        this.transform.rotation = m_MyGameManager.StartPosition.rotation;

        StartePos = m_MyGameManager.StartPosition;
        // ============ Skill ==================
        //m_MyPlayerMemoryShare.m_AllSkill.ListAllSkill.Add(new CPlayerChargeSkill(this));
    }
    

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();


        //UpdateAnimationVal().Subscribe(_ => {
        //    UpdateAnimationChangVal();
        //}).AddTo(this.gameObject);

#if DEBUGPC

#endif

    }

    public void InitGameStart()
    {
        //CGameSceneWindow lTempCGameSceneWindow = CGameSceneWindow.SharedInstance;
        //lTempCGameSceneWindow.SetData(m_MyPlayerMemoryShare.m_AllArchitecturalTopics, false);
    }

    public void UpdateAnimationChangVal()
    {
       // if (m_MyPlayerMemoryShare.m_isupdateAnimation)
    }

    protected override void Update()
    {
        base.Update();

        InputUpdata();

        //m_MyPlayerMemoryShare.m_AllSkill.UpdateSkill(Time.deltaTime);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void InputUpdata()
    {
#if DEBUGPC
        CDebugWindow lTempDebugWindow = CDebugWindow.SharedInstance;
        if (lTempDebugWindow != null)
        {
            if (lTempDebugWindow.IsShow())
                return;
        }
#endif

        if ((int)m_MyGameManager.CurState < (int)CGameManager.EState.ePlay)
            return;

        //if (Input.GetMouseButtonDown(0))
        //{
        //    PlayerMouseDown();
        //}

        if (Input.GetMouseButtonUp(0))
        {
            PlayerMouseUp();
        }
        else if (Input.GetMouseButton(0))
        {
            PlayerMouseDrag();
        }
    }

    //public void PlayerMouseDown()
    //{
    //    DataState lTempDataState = m_AllState[(int)CurState];
    //    if (m_CurState != CMovableStatePototype.EMovableState.eNull && lTempDataState != null && lTempDataState.AllThisState[lTempDataState.index] != null)
    //        lTempDataState.AllThisState[lTempDataState.index].MouseDown();

    //    if (!m_MyPlayerMemoryShare.m_bDown)
    //    {
    //        m_MyPlayerMemoryShare.m_bDown = true;
    //        m_MyPlayerMemoryShare.m_OldMouseDownPos = Input.mousePosition;
    //        m_MyPlayerMemoryShare.m_DownMouseDownPos = Input.mousePosition;
    //        m_MyPlayerMemoryShare.m_DownTime = Time.realtimeSinceStartup;
    //    }
    //}

    public override void OnMouseDown()
    {
        //DataState lTempDataState = m_AllState[(int)CurState];
        //if (m_CurState != CMovableStatePototype.EMovableState.eNull && lTempDataState != null && lTempDataState.AllThisState[lTempDataState.index] != null)
        //    lTempDataState.AllThisState[lTempDataState.index].MouseDown();

        base.OnMouseDown();

        if (!m_MyPlayerMemoryShare.m_bDown)
        {
            m_MyPlayerMemoryShare.m_bDown = true;
            m_MyPlayerMemoryShare.m_OldMouseDownPos = Input.mousePosition;
            m_MyPlayerMemoryShare.m_DownMouseDownPos = Input.mousePosition;
            m_MyPlayerMemoryShare.m_DownTime = Time.realtimeSinceStartup;
        }

    }

    public void PlayerMouseDrag()
    {
        //if (!m_MyPlayerMemoryShare.m_bDown)
        //    return;

        m_MyPlayerMemoryShare.m_CurMouseDownPos = Input.mousePosition;

        DataState lTempDataState = m_AllState[(int)CurState];
        if (m_CurState != CMovableStatePototype.EMovableState.eNull && lTempDataState != null && lTempDataState.AllThisState[lTempDataState.index] != null)
            lTempDataState.AllThisState[lTempDataState.index].MouseDrag();

        m_MyPlayerMemoryShare.m_OldMouseDownPos = m_MyPlayerMemoryShare.m_CurMouseDownPos;
    }

    public void PlayerMouseUp()
    {
        if (m_MyPlayerMemoryShare.m_bDown)
        {
            DataState lTempDataState = m_AllState[(int)CurState];
            if (m_CurState != CMovableStatePototype.EMovableState.eNull && lTempDataState != null && lTempDataState.AllThisState[lTempDataState.index] != null)
                lTempDataState.AllThisState[lTempDataState.index].MouseUp();

            m_MyPlayerMemoryShare.m_bDown = false;
            m_MyPlayerMemoryShare.m_OldMouseDownPos = Vector3.zero;
            m_MyPlayerMemoryShare.m_DownTime = -1.0f;
        }
    }

    public bool PlayAnimaton(bool showTimeline)
    {
        if (m_PlayDragToACT == null)
            return false;

        m_PlayDragToACT.SetActive(showTimeline);
        return true;
    }

    public void PlayEnd()
    {
        ObserverPlayEndEvent().OnNext(UniRx.Unit.Default);
    }

    //public void UpdateBrickAmount()
    //{

    //}

    // ===================== UniRx ======================

    public UniRx.Subject<UniRx.Unit> m_PlayStartEvent = new UniRx.Subject<UniRx.Unit>();

    public UniRx.Subject<UniRx.Unit> ObserverPlayStartEvent()
    {
        return m_PlayStartEvent ?? (m_PlayStartEvent = new UniRx.Subject<UniRx.Unit>());
    }


    public UniRx.Subject<UniRx.Unit> m_PlayEndEvent = new UniRx.Subject<UniRx.Unit>();

    public UniRx.Subject<UniRx.Unit> ObserverPlayEndEvent()
    {
        return m_PlayEndEvent ?? (m_PlayEndEvent = new UniRx.Subject<UniRx.Unit>());
    }

    public UniRx.Subject<UniRx.Unit> ObserverFritPlay()
    {
        return m_MyPlayerMemoryShare.m_FritPlay ?? (m_MyPlayerMemoryShare.m_FritPlay = new UniRx.Subject<UniRx.Unit>());
    }

    // ===================== UniRx ======================
}
