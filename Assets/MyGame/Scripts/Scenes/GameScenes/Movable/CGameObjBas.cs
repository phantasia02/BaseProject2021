using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IGameObjOpenGravity
{
    void OpenGravity(bool open);
}



public class CGameObjBasListData
{
    public List<CGameObjBas> m_GameObjBasListData = new List<CGameObjBas>();
}

public abstract class CGameObjBas : MonoBehaviour
{
    public enum EObjType
    {
        eMovable            = 0,
        etestGamebase       = 1,
        eMax
    }

    abstract public EObjType ObjType();
    protected Transform m_OriginalParent = null;
    protected CGameManager m_MyGameManager = null;
    protected CGameObjBasManager m_GameObjBasManager = null;

    protected int m_GameObjBasIndex = -1;
    public int GameObjBasIndex
    {
        set { m_GameObjBasIndex = value; }
        get { return m_GameObjBasIndex; }
    }

    protected virtual void Awake()
    {
        m_MyGameManager = GetComponentInParent<CGameManager>();

        if (m_MyGameManager == null)
            m_MyGameManager = GameObject.FindObjectOfType<CGameManager>();

        
        m_OriginalParent = gameObject.transform.parent;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        if (m_GameObjBasManager == null)
            m_GameObjBasManager = CGameObjBasManager.SharedInstance;

        m_GameObjBasManager.AddGameObjBasListData(this);
    }

    public virtual void Init()
    {

    }

    protected virtual void Update()
    {

    }

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        
    }

    protected virtual void OnDestroy()
    {
        m_GameObjBasManager.RemoveGameObjBasListData(this);
    }

    public virtual void ReceiveMessage(DataMessage data)
    {
        switch (data.m_MessageType)
        {
            case EMessageType.eDestroyObj:
                Destroy(this.gameObject);
                break;
            //default:
            //    Debug.LogError("Error Null Message");
            //    break;
        }
    }
}
