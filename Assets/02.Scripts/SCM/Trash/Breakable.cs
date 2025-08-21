using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    public bool isBreak = false;
    public List<GameObject> breakablePieces; // 드래그 앤 드롭
    void Start()
    {
    }

    
    void Update()
    {
        // 테스트용 인스펙터에서 부수기
        if (isBreak)
        {
            foreach (var piece in breakablePieces)
            {
                piece.transform.parent = null;
                piece.gameObject.GetComponent<Rigidbody>().useGravity = true;
                piece.gameObject.GetComponent<MeshCollider>().enabled = true;
            }
            gameObject.SetActive(false);
        }
    }

    public void Break()
    {
        foreach (var piece in breakablePieces)
        {
            piece.transform.parent = null;
            piece.gameObject.GetComponent<Rigidbody>().useGravity = true;
            piece.gameObject.GetComponent<MeshCollider>().enabled = true;
        }
        gameObject.SetActive(false);
    }
}
