using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public enum TrashStatus // 쓰레기 상태를 나타내는 eNum
{
    Clean = 0,
    Dirty = 1,
    Damaged = 2
}

public class TrashData : MonoBehaviour
{
    [SerializeField] // 테스트 끝나면 지워도됨
    private TrashInfo _info;
    public TrashInfo Info
    {
        get
        {
            return this._info;
        }
        set
        {
            this._info = value;

            this.Init(); // 종류에 따라 다른 초기화
        }
    }
    public void Clean()
    {
        this.Info.status = (int)TrashStatus.Clean; // 쓰레기 상태를 청소로 변경
    }
    protected virtual void Init()
    {
        this.transform.position = new Vector3(this.Info.posX, this.Info.posY, this.Info.posZ);
        this.transform.rotation = Quaternion.Euler(this.Info.rotX, this.Info.rotY, this.Info.rotZ);
        this.transform.Translate(0, this.Info.height, 0,Space.World); // 높이값을 적용
    }
    public virtual void DisActivate()
    {
        this.gameObject.SetActive(false); // 쓰레기 오브젝트 비활성화
    }
}