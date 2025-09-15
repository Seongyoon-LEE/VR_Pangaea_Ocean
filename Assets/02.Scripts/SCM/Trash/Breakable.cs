using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    public bool isBreak = false;
    private List<GameObject> breakablePieces = new List<GameObject>(); // 드래그 앤 드롭
    public int minCount = 3; // 인스펙터
    public int maxCount = 6; // 인스펙터
    private int breakableCount;
    private Transform possiblePiece;
    
    void Start()
    {
        possiblePiece = transform.GetChild(0);
        breakableCount = Random.Range(minCount, maxCount + 1);
        while (breakablePieces.Count < breakableCount)
        {
            GameObject piece = possiblePiece.GetChild(Random.Range(0, possiblePiece.childCount)).gameObject;
            if (!breakablePieces.Contains(piece))
            {
                breakablePieces.Add(piece);
            }
        }
    }

    
    void Update()
    {
        // 테스트용 인스펙터에서 부수기
        if (isBreak)
        {
            Break();
        }
    }

    public void Break()
    {
        foreach (var piece in breakablePieces)
        {
            piece.transform.parent = null;
            piece.GetComponent<Rigidbody>().useGravity = true;
            piece.GetComponent<MeshCollider>().enabled = true;
            GameManager.Instance.StartCoroutine(PieceY(piece));
        }
        gameObject.SetActive(false);
    }

    IEnumerator PieceY(GameObject piece)
    {
        Rigidbody rb = piece.GetComponent<Rigidbody>();

        while(true)
        {
            print(piece.transform.position.y);
            if (piece.transform.position.y >= 0 && rb.velocity.y > 0)
                rb.velocity = Vector3.zero;

            yield return new WaitForSeconds(0.3f);
        }
    }
}
