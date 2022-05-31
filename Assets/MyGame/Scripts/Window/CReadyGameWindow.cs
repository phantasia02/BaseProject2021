using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UniRx;

public class CReadyGameWindow : CSingletonMonoBehaviour<CReadyGameWindow>
{

    //[SerializeField] Button m_OptionButton;
    //[SerializeField] Button m_SkinButton;
    // ==================== SerializeField ===========================================

    [SerializeField] GameObject m_ShowObj = null;
    [SerializeField] CUIText m_CurLevel = null;
    [SerializeField] CUIText m_CurLevelShowdo = null;

    
    // ==================== SerializeField ===========================================

    //bool m_CloseShowUI = false;


    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {

    }


    public void ShowWindowUI()
    {
        m_ShowObj.SetActive(true);

        CSaveManager lTempCSaveManager = CSaveManager.SharedInstance;
        if (lTempCSaveManager)
        {
            string lTempstring = (SceneManager.GetActiveScene().buildIndex).ToString() + "\n" + "STAGE";
            m_CurLevel.SetText(lTempstring);
            m_CurLevelShowdo.SetText(lTempstring);
        }
    }

    public bool GetShow() { return m_ShowObj.activeSelf; }

    public void CloseShowUI()
    {
        //if (m_CloseShowUI)
        //    return;

        //m_CloseShowUI = true;
        m_ShowObj.SetActive(false);
    }

    
}
