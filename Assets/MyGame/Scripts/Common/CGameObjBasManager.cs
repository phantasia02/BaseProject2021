using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public enum EMessageType
{
    eNull       = 0,
    eDestroyObj = 1,
    eUptest     = 10,
    eMax
}

public class DataMessage
{
    public int              m_SendObjID         = -1;
    public int              m_ReceiveObjID      = -1;
    public EMessageType     m_MessageType       = EMessageType.eNull;

    public List<int>        m_Listint           = null;
    public List<float>      m_Listfloat         = null;
    public List<string>     m_Liststring        = null;
    public List<Vector3>    m_ListVector3       = null;

    //public DataMessage(int SendObjID, int ReceiveObjID, EMessageType MessageType)
    //{
    //    m_SendObjID     = SendObjID;
    //    m_ReceiveObjID  = ReceiveObjID;
    //    m_MessageType   = MessageType;
    //}
}

public class CGameObjBasManager : CSingletonMonoBehaviour<CGameObjBasManager>
{
    // ==================== All ObjData  ===========================================

    protected CGameObjBasListData[] m_AllGameObjBas = new CGameObjBasListData[(int)CGameObjBas.EObjType.eMax];
    public CGameObjBasListData GetTypeGameObjBaseListData(CGameObjBas.EObjType type) { return m_AllGameObjBas[(int)type]; }

    protected CMovableBaseListData[] m_AllMovableBase = new CMovableBaseListData[(int)CMovableBase.EMovableType.eMax];
    public CMovableBaseListData GetTypeMovableBaseListData(CMovableBase.EMovableType type) { return m_AllMovableBase[(int)type]; }

    protected CActorBaseListData[] m_AllActorBase = new CActorBaseListData[(int)CActor.EActorType.eMax];
    public CActorBaseListData GetTypeActorBaseListData(CActor.EActorType type) { return m_AllActorBase[(int)type]; }

    // ==================== All ObjData ===========================================
    public Hashtable m_GameObjBasHashtable = new Hashtable();

    protected bool isApplicationQuitting = false;

    void OnApplicationQuit() { isApplicationQuitting = true; }


    void Awake()
    {
        for (int i = 0; i < m_AllGameObjBas.Length; i++)
            m_AllGameObjBas[i] = new CGameObjBasListData();

        for (int i = 0; i < m_AllMovableBase.Length; i++)
            m_AllMovableBase[i] = new CMovableBaseListData();

        for (int i = 0; i < m_AllActorBase.Length; i++)
            m_AllActorBase[i] = new CActorBaseListData();
    }

    // Start is called before the first frame update
    void Start()
    {
        // ============= test ====================
        //Observable.Timer(TimeSpan.FromSeconds(1))
        //    .Subscribe(_ => { RemoveGameObjBas(m_AllGameObjBas[1].m_GameObjBasListData[0].GetInstanceID()); })
        //    .AddTo(this);

        // ============= test ====================
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddGameObjBasListData(CGameObjBas addGameObjBas)
    {
        if (isApplicationQuitting)
            return;

        if (addGameObjBas == null)
            return;

        int lTempTypeIndex = (int)addGameObjBas.ObjType();

        addGameObjBas.GameObjBasIndex = m_AllGameObjBas[lTempTypeIndex].m_GameObjBasListData.Count;
        m_AllGameObjBas[lTempTypeIndex].m_GameObjBasListData.Add(addGameObjBas);
        m_GameObjBasHashtable.Add(addGameObjBas.GetInstanceID(), addGameObjBas);
    }

    public void RemoveGameObjBasListData(CGameObjBas addGameObjBas)
    {
        if (isApplicationQuitting)
            return;

        if (addGameObjBas == null)
            return;

        int lTempTypeIndex = (int)addGameObjBas.ObjType();
        List<CGameObjBas> lTempGameObjBasList = m_AllGameObjBas[lTempTypeIndex].m_GameObjBasListData;

        lTempGameObjBasList.Remove(addGameObjBas);
        for (int i = 0; i < lTempGameObjBasList.Count; i++)
            lTempGameObjBasList[i].GameObjBasIndex = i;

        m_GameObjBasHashtable.Remove(addGameObjBas.GetInstanceID());
    }

    public void RemoveGameObjBas(int InstanceID)
    {
        if (!m_GameObjBasHashtable.ContainsKey(InstanceID))
            return;

        CGameObjBas lTempCGameObjBas = (CGameObjBas)m_GameObjBasHashtable[InstanceID];
        Destroy(lTempCGameObjBas.gameObject);
    }

    public void AddMovableBaseListData(CMovableBase addMovableBase)
    {
        if (addMovableBase == null)
            return;

        int lTempTypeIndex = (int)addMovableBase.MyMovableType();
        m_AllMovableBase[lTempTypeIndex].m_MovableBaseListData.Add(addMovableBase);
    }

    public void RemoveMovableBaseListData(CMovableBase removeMovableBase)
    {
        if (isApplicationQuitting)
            return;

        if (removeMovableBase == null)
            return;

        int lTempTypeIndex = (int)removeMovableBase.MyMovableType();
        List<CMovableBase> lTempMovableBaseList = m_AllMovableBase[lTempTypeIndex].m_MovableBaseListData;
        lTempMovableBaseList.Remove(removeMovableBase);
    }

    public void AddActorBaseListData(CActor addActorBase)
    {
        if (addActorBase == null)
            return;

        int lTempTypeIndex = (int)addActorBase.MyActorType();
        m_AllActorBase[lTempTypeIndex].m_ActorBaseListData.Add(addActorBase);
    }

    public void RemoveActorBaseListData(CActor removeActorBase)
    {
        if (isApplicationQuitting)
            return;

        if (removeActorBase == null)
            return;

        int lTempTypeIndex = (int)removeActorBase.MyActorType();
        List<CActor> lTempActorBaseList = m_AllActorBase[lTempTypeIndex].m_ActorBaseListData;
        lTempActorBaseList.Remove(removeActorBase);
    }


    // =============== Change Actor state Type  ========================
    public void SetAllActorTypeState(CActor.EActorType type, CMovableStatePototype.EMovableState state, int Stateindex = -1)
    {
        int index = (int)type;

        if (m_AllActorBase.Length <= index)
            return;

        foreach (var item in m_AllActorBase[index].m_ActorBaseListData)
            item.SetChangState(state, Stateindex);
    }
    // =============== Change Actor state Type  ========================

    public void TypeSendMessage()
    { 
    
    }



    public void SendMessageID(DataMessage SendData)
    {
        if (SendData == null)
            return;

        if (!m_GameObjBasHashtable.ContainsKey(SendData.m_ReceiveObjID))
            return;

        CGameObjBas lTempCGameObjBas = (CGameObjBas)m_GameObjBasHashtable[SendData.m_ReceiveObjID];
        lTempCGameObjBas.ReceiveMessage(SendData);
    }
}
