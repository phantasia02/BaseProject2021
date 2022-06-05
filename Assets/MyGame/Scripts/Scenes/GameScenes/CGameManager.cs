using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Cinemachine;
using UniRx;
using UniRx.Triggers;
using MYgame.Scripts.Scenes.GameScenes.Data;
using MYgame.Scripts.Window;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System;

public class CGameManager : MonoBehaviour
{
    public enum EState
    {
        eReady              = 0,
        ePlay               = 1,
        eGameOver           = 2,
        eWinUI              = 3,
        eMax
    };
    
    bool m_bDown = false;

    CChangeScenes m_ChangeScenes = new CChangeScenes();
    protected ResultUI m_MyResultUI = null;
    public ResultUI MyResultUI { get { return m_MyResultUI; } }

    protected Camera m_Camera = null;
    public Camera MainCamera { get { return m_Camera; } }

    protected CPlayer m_Player = null;
    public CPlayer Player { get { return m_Player; } }
    // ==================== SerializeField ===========================================

    //[SerializeField] GameObject m_WinCamera = null;
    //public GameObject WinCamera { get { return m_WinCamera; } }
    [Header("Result OBJ")]
    [SerializeField] protected GameObject   m_WinObjAnima                   = null;
    [SerializeField] protected GameObject   m_OverObjAnima                  = null;
    [SerializeField] protected Transform    m_CurVcamObjAnima               = null;
    public Transform CurVcamObjAnima => m_CurVcamObjAnima;

    [SerializeField] protected GameObject[] m_TimeGameObject = null;
    public GameObject GetTimeObj(int index)
    {
        if (m_TimeGameObject.Length <= index)
            return null;

        return m_TimeGameObject[index];
    }

    [SerializeField] protected GameObject   PrefabGameSceneData             = null;
    [SerializeField] protected GameObject   m_BG                            = null;

    // ==================== SerializeField ===========================================

    protected CGameObjBasManager m_MyGameObjBasManager = null;

    protected CinemachineTargetGroup m_EndCinemachineTargetGroup = null;
    protected bool isApplicationQuitting = false;
    public bool GetisApplicationQuitting { get { return isApplicationQuitting; } }

    protected StageData m_CurStageData = null;
    public StageData CurStageData
    {
        get
        {
            if (m_CurStageData == null)
            {
                CGGameSceneData lTempGameSceneData = CGGameSceneData.SharedInstance;
                m_CurStageData = lTempGameSceneData.LevelToStageData(SceneManager.GetActiveScene().buildIndex - 1);
            }

            return m_CurStageData;
        }
    }

    //protected float m_TotleColorRatio = 1.0f;
    //public float TotleColorRatio => m_TotleColorRatio;

    private EState m_eCurState = EState.eReady;
    public EState CurState { get { return m_eCurState; } }
    protected float m_StateTime = 0.0f;
    protected float m_StateUnscaledTime = 0.0f;
    protected int m_StateCount = 0;
    protected Vector3 m_OldInput;
    protected float m_HalfScreenWidth = 600.0f;
    public float HalfScreenWidth => m_HalfScreenWidth;

    protected Transform m_StartPosition = null;
    public Transform StartPosition => m_StartPosition;

    protected Transform m_WinPosition = null;
    public Transform WinPosition => m_WinPosition;

    //protected Scene sceneMain;
    //protected Scene _scenePrediction;
    //public Scene scenePrediction => _scenePrediction;
    //protected PhysicsScene _sceneMainPhysics;
    //protected PhysicsScene _scenePredictionPhysics;
    //public PhysicsScene scenePredictionPhysics => _scenePredictionPhysics;

    protected bool m_OpenPhysics = true;
    public bool OpenPhysics
    {
        set => m_OpenPhysics = value;
        get => m_OpenPhysics;
    }

    void Awake()
    {
        Application.targetFrameRate = 60;
        const float HWRatioPototype = StaticGlobalDel.g_fcbaseHeight / StaticGlobalDel.g_fcbaseWidth;
        float lTempNewHWRatio = ((float)Screen.height / (float)Screen.width);
        m_HalfScreenWidth = (StaticGlobalDel.g_fcbaseWidth / 2.0f) * (lTempNewHWRatio / HWRatioPototype);
       // m_StartPosition = GameObject.Find("StartPosition").transform;


        GameObject lTempCameraObj = GameObject.FindGameObjectWithTag("MainCamera");
        if (lTempCameraObj != null)
            m_Camera = lTempCameraObj.GetComponent<Camera>();

        //Physics.autoSimulation = false;
        //sceneMain = SceneManager.GetActiveScene();
        //_sceneMainPhysics = sceneMain.GetPhysicsScene();

        //CreateSceneParameters sceneParam = new CreateSceneParameters(LocalPhysicsMode.Physics3D);
        //_scenePrediction = SceneManager.CreateScene("ScenePredictPhysics", sceneParam);
        //_scenePredictionPhysics = scenePrediction.GetPhysicsScene();

        //if (m_BG != null)
        //{
        //    GameObject lTempBG = GameObject.Instantiate(m_BG);
        //    SceneManager.MoveGameObjectToScene(lTempBG, scenePrediction);
        //}

        //m_EndCinemachineTargetGroup = this.GetComponentInChildren<CinemachineTargetGroup>();


        StaticGlobalDel.CreateSingletonObj(PrefabGameSceneData);

        //if (CurStageData.WinMoveWinPos)
        //    m_WinPosition = GameObject.Find("WinPos").transform;


        //Physics.gravity = CurStageData.Gravity;
        //for (int i = 0; i < MyTargetBuilding.BrickRandomLevelAllColor.Length; i++)
        //{
        //    m_DictionaryDataLevelAllColor.Add(MyTargetBuilding.BrickRandomLevelAllColor[i].ID, MyTargetBuilding.BrickRandomLevelAllColor[i]);
        //    MyTargetBuilding.BrickRandomLevelAllColor[i].TotleColorRatio = 0.0f;
        //    foreach (DataColor Dcolor in MyTargetBuilding.BrickRandomLevelAllColor[i]._brickColors)
        //        MyTargetBuilding.BrickRandomLevelAllColor[i].TotleColorRatio += Dcolor._Ratio;
        //}

        m_Player = this.GetComponentInChildren<CPlayer>();
        m_MyResultUI = GameObject.FindObjectOfType<ResultUI>();
        if (m_MyResultUI != null)
        {
            m_MyResultUI.Over.onClick.AddListener(OnReset);
            m_MyResultUI.Next.onClick.AddListener(OnNext);
        }

        m_MyGameObjBasManager = CGameObjBasManager.SharedInstance;
        StaticGlobalDel.fixedDeltaTime = Time.fixedDeltaTime;
    }

    // Start is called before the first frame update
    void Start()
    {
        //var lTempUpdateAs = this.UpdateAsObservable();
        //lTempUpdateAs.First(_ => Input.GetMouseButtonDown(0)).Subscribe(Var=> {


        //});

        CReadyGameWindow lTempCReadyGameWindow = CReadyGameWindow.SharedInstance;
        if (lTempCReadyGameWindow)
            lTempCReadyGameWindow.ShowWindowUI();

        //Player.SetChangState(CMovableStatePototype.EMovableState.eWait);
        //Player.ObserverPlayStartEvent().First().
        //    Subscribe(VarRename => {
        //        SetState(EState.ePlay);

        //        if (lTempCReadyGameWindow && lTempCReadyGameWindow.GetShow())
        //            lTempCReadyGameWindow.CloseShowUI();

        //        CGameSceneWindow lTempGameSceneWindow = CGameSceneWindow.SharedInstance;
        //        if (lTempGameSceneWindow)
        //        {

        //        }
        //    }).AddTo(this);

       // SetState(EState.ePlay);
    }

    //private void FixedUpdate()
    //{
    //    if (m_OpenPhysics)
    //        _sceneMainPhysics.Simulate(Time.fixedDeltaTime);
    //}

    public void SetState(EState lsetState)
    {
        if (lsetState == m_eCurState)
            return;

        if (m_eCurState == EState.eWinUI || m_eCurState == EState.eGameOver)
            return;

        EState lOldState = m_eCurState;
        m_StateTime = 0.0f;
        m_StateCount = 0;
        m_StateUnscaledTime = 0.0f;
        m_eCurState = lsetState;

        CGameSceneWindow lTempGameSceneWindow = CGameSceneWindow.SharedInstance;

        switch (m_eCurState)
        {
            case EState.eReady:
                {
                    
                }
                break;
            case EState.ePlay:
                {
                    
                    //if (lTempGameSceneWindow != null)
                    //{
                    //    lTempGameSceneWindow.ShowObj(true);
                    //    lTempGameSceneWindow.MyGameStatusUI.StartTimer(TimeOut);
                    //}


                }
                break;
            case EState.eWinUI:
                {
                    if (SceneManager.GetActiveScene().buildIndex == 4)
                    {
                        //CActorBaseListData lTempCActorBaseListData = GetTypeActorBaseListData(CActor.EActorType.eNPC);


                    }

                    m_MyResultUI.ShowSuccessUI(0.5f);
                }
                break;
            case EState.eGameOver:
                {
                    //if (lTempGameSceneWindow)
                    //{
                    //    m_MyResultUI.SetCutTagetCoin(lTempGameSceneWindow.CurCoinNumber, lTempGameSceneWindow.TargetCoinNumber);
                    //}
                    //if (lTempGameSceneWindow)
                    //    lTempGameSceneWindow.ShowObj(false);

                    m_MyResultUI.ShowFailedUI(0.0f);
                }
                break;
        }
    }

    public void Update()
    {
        

        //m_MyGameObjBasManager.
    }

    public void UsePlayTick()
    {
//#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            m_bDown = true;
            m_OldInput = Input.mousePosition;
            //InputRay();
           // OKAllGroupQuestionHole(0);
        }
        else if (Input.GetMouseButton(0))
        {
            //float moveX = (Input.mousePosition.x - m_OldInput.x) / m_HalfScreenWidth;
            //m_Player.SetXMove(moveX);
            //m_OldInput = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (m_bDown)
            {
                m_OldInput = Vector3.zero;
                m_bDown = false;
            }
        }

    }

    public void OnNext()
    {
        m_ChangeScenes.LoadGameScenes();
    }

    public void OnReset()
    {
        m_ChangeScenes.ResetScene();
    }

    public void OnDestroy()
    {
        DOTween.KillAll();

        if (m_MyResultUI != null)
            m_MyResultUI.DeactivateUI();
    }

    void OnApplicationQuit() { isApplicationQuitting = true; }

}
