using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ObstacleKind
{
    Shark = 0,
    Squid = 1,
    Tornado = 2
}
public class ObstacleData : MonoBehaviour
{
    [SerializeField] // 데이터 테스트 끝나면 지워도됨
    ObstacleInfo _info;
    public ObstacleInfo Info 
    {
        get 
        {
            return _info; 
        } 
        set 
        {
            _info = value;
            this.Init();
        }
    }
    protected virtual void Init()
    {
        this.transform.position = new Vector3(this.Info.posX, this.Info.posY, this.Info.posZ);
    }
    public virtual void DisActivate()
    {
        this.gameObject.SetActive(false); // 장애물 오브젝트 비활성화
        // 상어의 경우 내부의 순찰 포인트를 자식오브젝트에서 뺐다가 비활성화시 다시 넣어야함
    }
}
