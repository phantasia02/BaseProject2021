using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CChangeDirTag : MonoBehaviour
{
    [SerializeField] protected Vector3 m_AddForce = Vector3.zero;
    protected Vector3 m_WordForce = Vector3.zero;

    public Vector3 Force => m_WordForce;

    private void Awake()
    {
        m_WordForce = this.transform.up * m_AddForce.y + this.transform.forward * m_AddForce.z + this.transform.right * m_AddForce.x;
    }
}
