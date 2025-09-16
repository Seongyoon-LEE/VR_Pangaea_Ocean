using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoPosManager : MonoBehaviour
{
    List<Transform> tornadoPoses = new List<Transform>();
    public void Init() // Init 실행 시 자식 오브젝트로 잡고있을 좌표 전부 가져옴
    {
        GetComponentsInChildren<Transform>(this.tornadoPoses);
        this.tornadoPoses.RemoveAt(0);
    }

    public Vector3 GetTornadoPos()
    {
        int idx = Random.Range(0, tornadoPoses.Count); // idx하나 받아오기
        var pos = tornadoPoses[idx].position; // 좌표를 저장
        this.tornadoPoses.RemoveAt(idx); // 한 좌표당 한번만 나오고 더 안나오도록
        return pos;
    }
}
