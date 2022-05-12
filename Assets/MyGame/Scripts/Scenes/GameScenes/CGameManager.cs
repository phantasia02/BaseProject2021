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

    [SerializeField] protected GameObject   PrefabGameSceneData             = null;
    [SerializeField] protected GameObject   m_BG                            = null;



    //[Header("Other UI")]
    //[SerializeField] protected GameObject   m_HistoryWindow     = null;
    // ==================== All ObjData  ===========================================

    //protected CGameObjBasListData[]     m_AllGameObjBas     = new CGameObjBasListData[(int)CGameObjBas.EObjType.eMax];
    //public CGameObjBasListData GetTypeGameObjBaseListData(CGameObjBas.EObjType type) { return m_AllGameObjBas[(int)type]; }

    //protected CMovableBaseListData[]    m_AllMovableBase    = new CMovableBaseListData[(int)CMovableBase.EMovableType.eMax];
    //public CMovableBaseListData GetTypeMovableBaseListData(CMovableBase.EMovableType type) { return m_AllMovableBase[(int)type]; }

    //protected CActorBaseListData[]      m_AllActorBase      = new CActorBaseListData[(int)CActor.EActorType.eMax];
    //public CActorBaseListData GetTypeActorBaseListData(CActor.EActorType type) { return m_AllActorBase[(int)type]; }


    // ==================== All ObjData ===========================================

    // ==================== SerializeField ===========================================

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

    protected Scene sceneMain;
    protected Scene _scenePrediction;
    public Scene scenePrediction => _scenePrediction;
    protected PhysicsScene _sceneMainPhysics;
    protected PhysicsScene _scenePredictionPhysics;
    public PhysicsScene scenePredictionPhysics => _scenePredictionPhysics;

    

    void Awake()
    {
        Application.targetFrameRate = 60;
        const float HWRatioPototype = StaticGlobalDel.g_fcbaseHeight / StaticGlobalDel.g_fcbaseWidth;
        float lTempNewHWRatio = ((float)Screen.height / (float)Screen.width);
        m_HalfScreenWidth = (StaticGlobalDel.g_fcbaseWidth / 2.0f) * (lTempNewHWRatio / HWRatioPototype);
        m_StartPosition = GameObject.Find("StartPosition").transform;



        m_MyResultUI = gameObject.GetComponentInChildren<ResultUI>(true);

        if (m_MyResultUI != null)
        {
            m_MyResultUI.Over.onClick.AddListener(OnReset);
            m_MyResultUI.Next.onClick.AddListener(OnNext);

        }

        GameObject lTempCameraObj = GameObject.FindGameObjectWithTag("MainCamera");
        if (lTempCameraObj != null)
            m_Camera = lTempCameraObj.GetComponent<Camera>();

        Physics.autoSimulation = false;
        sceneMain = SceneManager.GetActiveScene();
        _sceneMainPhysics = sceneMain.GetPhysicsScene();

        CreateSceneParameters sceneParam = new CreateSceneParameters(LocalPhysicsMode.Physics3D);
        _scenePrediction = SceneManager.CreateScene("ScenePredictPhysics", sceneParam);
        _scenePredictionPhysics = scenePrediction.GetPhysicsScene();

        GameObject lTempBG = GameObject.Instantiate(m_BG);
        SceneManager.MoveGameObjectToScene(lTempBG, scenePrediction);

        //m_EndCinemachineTargetGroup = this.GetComponentInChildren<CinemachineTargetGroup>();
        //for (int i = 0; i < m_AllGameObjBas.Length; i++)
        //    m_AllGameObjBas[i] = new CGameObjBasListData();

        //for (int i = 0; i < m_AllMovableBase.Length; i++)
        //    m_AllMovableBase[i] = new CMovableBaseListData();

        //for (int i = 0; i < m_AllActorBase.Length; i++)
        //    m_AllActorBase[i] = new CActorBaseListData();


        StaticGlobalDel.CreateSingletonObj(PrefabGameSceneData);

        if (CurStageData.WinMoveWinPos)
            m_WinPosition = GameObject.Find("WinPos").transform;

        //for (int i = 0; i < MyTargetBuilding.BrickRandomLevelAllColor.Length; i++)
        //{
        //    m_DictionaryDataLevelAllColor.Add(MyTargetBuilding.BrickRandomLevelAllColor[i].ID, MyTargetBuilding.BrickRandomLevelAllColor[i]);
        //    MyTargetBuilding.BrickRandomLevelAllColor[i].TotleColorRatio = 0.0f;
        //    foreach (DataColor Dcolor in MyTargetBuilding.BrickRandomLevelAllColor[i]._brickColors)
        //        MyTargetBuilding.BrickRandomLevelAllColor[i].TotleColorRatio += Dcolor._Ratio;
        //}

        CAudioManager lTempAudioManager = CAudioManager.SharedInstance;
        lTempAudioManager.PlayBGM(CAudioManager.EBGM.eOutGame);
        m_Player = this.GetComponentInChildren<CPlayer>();
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

        Player.SetChangState(CMovableStatePototype.EMovableState.eWait);
        Player.ObserverPlayStartEvent().First().
            Subscribe(VarRename => {
                SetState(EState.ePlay);

                if (lTempCReadyGameWindow && lTempCReadyGameWindow.GetShow())
                    lTempCReadyGameWindow.CloseShowUI();

                CGameSceneWindow lTempGameSceneWindow = CGameSceneWindow.SharedInstance;
                if (lTempGameSceneWindow)
                {

                }
            }).AddTo(this);

        //Observable.EveryUpdate().First(_ => Input.GetMouseButtonDown(0)).Subscribe(
        //OnNext =>
        //{
        //    if (m_eCurState == EState.eReady)
        //    {
        //        SetState(EState.ePlay);

        //        CReadyGameWindow lTempCReadyGameWindow = CReadyGameWindow.SharedInstance;
        //        if (lTempCReadyGameWindow && lTempCReadyGameWindow.GetShow())
        //            lTempCReadyGameWindow.CloseShowUI();

        //        CGameSceneWindow lTempGameSceneWindow = CGameSceneWindow.SharedInstance;
        //        if (lTempGameSceneWindow)
        //        {

        //        }
        //    }
        //},
        //OnCompleted => { Debug.Log("OK"); }
        //).AddTo(this);

       // SetState(EState.ePlay);
    }

    private void FixedUpdate()
    {
        _sceneMainPhysics.Simulate(Time.fixedDeltaTime);
    }

    //public void ShowPath(Vector3 AddForce, Rigidbody RefRigidbody)
    //{
    //    if (!_sceneMainPhysics.IsValid())
    //        return;

    //    GameObject predictionBall = GameObject.CreatePrimitive(PrimitiveType.Sphere);
    //    SceneManager.MoveGameObjectToScene(predictionBall, scenePrediction);
    //    predictionBall.transform.position = RefRigidbody.gameObject.transform.position;
    //    predictionBall.transform.rotation = RefRigidbody.gameObject.transform.rotation;
    //    predictionBall.transform.localScale = Vector3.one * 3.0f;

    //    Rigidbody lTempRigidbody = predictionBall.AddComponent<Rigidbody>();
    //    lTempRigidbody.mass = RefRigidbody.mass;
    //    lTempRigidbody.velocity = AddForce;

    //    //m_MyPlayerMemoryShare.m_LinePath.positionCount = m_MyPlayerMemoryShare.m_BezierPath.Length;
    //    //m_MyPlayerMemoryShare.m_LinePath.SetPositions(m_MyPlayerMemoryShare.m_BezierPath);
    //    m_LinePath.positionCount = 0;

    //    List<Vector3> lTempAllPathPoint = new List<Vector3>();
    //    const float CmaxTime = 3.0f;
    //    float lTempCutTime = 0.0f;

    //  //  Debug.Log($"==============================================");
    //    while (lTempCutTime < CmaxTime)
    //    {
    //        _scenePredictionPhysics.Simulate(Time.fixedDeltaTime * 4.0f);
    //        lTempCutTime += Time.fixedDeltaTime * 4.0f;

    //      //  Debug.Log($"predictionBall.transform.position = {predictionBall.transform.position}");
    //        lTempAllPathPoint.Add(predictionBall.transform.position);
           
    //    }

    //    m_LinePath.positionCount = lTempAllPathPoint.Count;
    //    m_LinePath.SetPositions(lTempAllPathPoint.ToArray());

    //    Destroy(predictionBall);
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

    void OnApplicationQuit() { isApplicationQuitting = true; }

    //public void SetWinUI()
    //{
    //    SetState(EState.eWinUI);
       
    //}

    //public void SetLoseUI()
    //{
    //    SetState(EState.eGameOver);
    //}
    

    // ==================== All ObjData  ===========================================

    //public void AddGameObjBasListData(CGameObjBas addGameObjBas)
    //{
    //    if (isApplicationQuitting)
    //        return;

    //    if (addGameObjBas == null)
    //        return;

    //    int lTempTypeIndex = (int)addGameObjBas.ObjType();

    //    addGameObjBas.GameObjBasIndex = m_AllGameObjBas[lTempTypeIndex].m_GameObjBasListData.Count;
    //    m_AllGameObjBas[lTempTypeIndex].m_GameObjBasListData.Add(addGameObjBas);
    //    m_AllGameObjBas[lTempTypeIndex].m_GameObjBasHashtable.Add(addGameObjBas.GetInstanceID(), addGameObjBas);
    //}

    //public void RemoveGameObjBasListData(CGameObjBas addGameObjBas)
    //{
    //    if (isApplicationQuitting)
    //        return;

    //    if (addGameObjBas == null)
    //        return;

    //    int lTempTypeIndex = (int)addGameObjBas.ObjType();
    //    List<CGameObjBas> lTempGameObjBasList = m_AllGameObjBas[lTempTypeIndex].m_GameObjBasListData;

    //    lTempGameObjBasList.Remove(addGameObjBas);
    //    for (int i = 0; i < lTempGameObjBasList.Count; i++)
    //        lTempGameObjBasList[i].GameObjBasIndex = i;

    //    m_AllGameObjBas[lTempTypeIndex].m_GameObjBasHashtable.Remove(addGameObjBas.GetInstanceID());
    //}

    //public void AddMovableBaseListData(CMovableBase addMovableBase)
    //{
    //    if (addMovableBase == null)
    //        return;

    //    int lTempTypeIndex = (int)addMovableBase.MyMovableType();
    //    m_AllMovableBase[lTempTypeIndex].m_MovableBaseListData.Add(addMovableBase);
    //}

    //public void RemoveMovableBaseListData(CMovableBase removeMovableBase)
    //{
    //    if (isApplicationQuitting)
    //        return;

    //    if (removeMovableBase == null)
    //        return;

    //    int lTempTypeIndex = (int)removeMovableBase.MyMovableType();
    //    List<CMovableBase> lTempMovableBaseList = m_AllMovableBase[lTempTypeIndex].m_MovableBaseListData;
    //    lTempMovableBaseList.Remove(removeMovableBase);
    //}

    //public void AddActorBaseListData(CActor addActorBase)
    //{
    //    if (addActorBase == null)
    //        return;

    //    int lTempTypeIndex = (int)addActorBase.MyActorType();
    //    m_AllActorBase[lTempTypeIndex].m_ActorBaseListData.Add(addActorBase);
    //}

    //public void RemoveActorBaseListData(CActor removeActorBase)
    //{
    //    if (isApplicationQuitting)
    //        return;

    //    if (removeActorBase == null)
    //        return;

    //    int lTempTypeIndex = (int)removeActorBase.MyActorType();
    //    List<CActor> lTempActorBaseList = m_AllActorBase[lTempTypeIndex].m_ActorBaseListData;
    //    lTempActorBaseList.Remove(removeActorBase);
    //}

    // ==================== All ObjData  ===========================================
}
