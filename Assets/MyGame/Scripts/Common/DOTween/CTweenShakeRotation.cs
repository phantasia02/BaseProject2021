using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CTweenShakeRotation : MonoBehaviour
{

    // ==================== SerializeField ===========================================

    [SerializeField] protected float    m_DurationTime  = 1.0f;
    [SerializeField] protected float    m_Strength      = 10.0f;
    [SerializeField] protected int      m_Vibrato       = 10;
    [SerializeField] protected float    m_Randomness    = 90.0f;
    [SerializeField] protected bool     m_FadeOut       = true;
    [SerializeField] protected int      m_LoopCount     = -1;



    // ==================== SerializeField ===========================================

    private void Awake()
    {
        this.transform.DOShakeRotation(m_DurationTime, m_Strength, m_Vibrato, m_Randomness, m_FadeOut).SetLoops(m_LoopCount).SetId(this);
    }

    private void OnDestroy()
    {
        DOTween.Kill(this);
    }
}
