using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleMaterial : MonoBehaviour
{
    public Material passM;
    public Material unPassM;

    private MeshRenderer cylinderRenderer;

    void Awake()
    {
        cylinderRenderer = GetComponent<MeshRenderer>();
    }

    // 퍼즐이 맞았을 때 호출할 함수
    public void OnPuzzleCorrect()
    {
        if (cylinderRenderer != null)
        {
            cylinderRenderer.material = passM;
        }
    }

    // 퍼즐이 틀렸을 때 호출할 함수
    public void OnPuzzleIncorrect()
    {
        if (cylinderRenderer != null)
        {
            cylinderRenderer.material = unPassM;
        }
    }
}
