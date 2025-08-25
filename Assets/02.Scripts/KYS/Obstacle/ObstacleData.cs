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

        } 
    }
}
