using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CtestGamebase : CGameObjBas
{
    public override EObjType ObjType() { return EObjType.etestGamebase; }

    public override void ReceiveMessage(DataMessage data)
    {
        switch (data.m_MessageType)
        {
            case EMessageType.eUptest:
                transform.Translate(Vector3.up * Time.deltaTime * 2.0f);
                break;
        }

        base.ReceiveMessage(data);
    }

}
