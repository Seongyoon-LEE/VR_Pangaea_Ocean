using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    public bool isBreak = false;
    public List<GameObject> breakablePieces;
    void Start()
    {
        foreach (var piece in breakablePieces)
        {
            //piece.gameObject.SetActive(false);
        }
    }

    
    void Update()
    {
        // 테스트용
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
