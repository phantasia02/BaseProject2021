using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using UnityEngine.SceneManagement;
using MYgame.Scripts.Window;
using System;

public class CGameSceneWindow : CSingletonMonoBehaviour<CGameSceneWindow>
{
    public enum EMoveTotalImageState
    {
        eNull = 0,
        eOpenStart = 1,
        eMoveUpdate = 2,
        eMax
    }

    CChangeScenes m_ChangeScenes = new CChangeScenes();
    [SerializeField] protected GameObject m_ShowObj = null;
    public GameObject _ShowObj => m_ShowObj;
    [SerializeField] protected Timer _timer = null;


    [SerializeField]
    [Tooltip("Total Fram Light curve")]
    protected AnimationCurve m_TotalFramLightCurve;

    protected int m_CurCoin = 0;
    public int CurCoinNumber => m_CurCoin;
    public Timer timer => _timer;



    private void OnValidate()
    {
    }

    public void SetTemptextPos(Vector3 pos)
    {
    }

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public bool GetShow() { return m_ShowObj.activeSelf; }
    public void ShowObj(bool showObj)
    {
        if (showObj)
        {
        }
        m_ShowObj.SetActive(showObj);
        // CGameSceneWindow.SharedInstance.SetCurState(CGameSceneWindow.EState.eEndStop);
    }


}
