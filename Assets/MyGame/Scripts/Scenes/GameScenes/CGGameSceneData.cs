using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MYgame.Scripts.Scenes.GameScenes.Data;



[System.Serializable]
public class CCompleteBuilding
{
    public GameObject m_Model = null;
    public Sprite m_UISprite = null;
}


[System.Serializable]
public class TweenColorEaseCurve
{
    [SerializeField]
    [Tooltip("The ease curve")]
    private AnimationCurve _curve;

    [SerializeField]
    [Tooltip("The duration of this ease curve")]
    private float _duration;

    [SerializeField]
    [ColorUsage(true)]
    [Tooltip("The start value of the tween")]
    private Color _startValue;

    [SerializeField]
    [ColorUsage(true)]
    [Tooltip("The end value of the tween")]
    private Color _endValue;


    public AnimationCurve curve => _curve;
    public float duration => _duration;
    public Color StartValue => _startValue;
    public Color endValue => _endValue;
}

public class CGGameSceneData : CSingletonMonoBehaviour<CGGameSceneData>
{

    public enum EAllFXType
    {
        eScared     = 0,
        eHappy      = 1,
        eBullet     = 2,
        eRubberBand = 3,
        eMax,
    };

    public enum EOtherObj
    {
        eBulletCam = 0,
        eMax,
    };

    public enum EUIPrefab
    {
        eReadyGameWindow    = 0,
        eMax,
    };

    public enum EExpressionSpriteType
    {
        eScared = 0,
        eWeep = 1,
    };


    public enum EPlayerTrailerType
    {
        ePotoType   = 0,
        eTruck2     = 1,
        eTruck3     = 2,
        eTruck4     = 3,
        eTruck5     = 4,
        eTruck6     = 5,
        eMax,
    };

    [SerializeField]  public    GameObject[]                m_AllFX                     = null;

    [VarRename(CGGameSceneData.EOtherObj.eMax)]
    [SerializeField]  public    GameObject[]                m_AllOtherObj               = null;
    [SerializeField]  public    GameObject[]                m_UIObj                     = null;
    [SerializeField]  public    Sprite[]                    m_AllExpressionSprite          = null;

    [SerializeField]  public    StageData[]                 m_AllStageData              = null;
    [SerializeField]  public    GameObject                  m_PrefabEventSystem         = null;
    [SerializeField]  public    GameObject                  m_SaveManager               = null;
    [SerializeField]  public    GameObject                  m_AudioManager              = null;
    
    private void Awake()
    {

    }

    public StageData LevelToStageData(int levelindex)
    {
        if (levelindex < 0)
            return null;

        return m_AllStageData[levelindex];
    }
}
