using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashData : MonoBehaviour
{
    [SerializeField] // 테스트 끝나면 지워도됨
    private TrashInfo _info;
    public Action cleanAction;
    public TrashInfo Info
    {
        get
        {
            return this._info;
        }
        set
        {
            this._info = value;
            this.transform.position = new Vector3(value.posX, value.posY, value.posZ);
            this.transform.rotation = Quaternion.Euler(value.rotX, value.rotY, value.rotZ);
            //status(상태값)에 따라 변화하는 로직
        }
    }
    public void Clean()
    {
        this.cleanAction();
    }
}