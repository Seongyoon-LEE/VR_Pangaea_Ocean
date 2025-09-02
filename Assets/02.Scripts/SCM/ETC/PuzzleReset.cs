using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit;

public class PuzzleReset : MonoBehaviour
{
    private XRLever lever;
    [SerializeField]private Puzzle puzzle;
    InteractionLayerMask originLayerMask; // 레이어 설정
    void Start()
    {
        lever = GetComponent<XRLever>();
        originLayerMask = lever.interactionLayers;
        puzzle = GameObject.Find("PuzzleWheel").GetComponent<Puzzle>(); // 조명 초기화 메서드 가져오기 위해서
        lever.selectExited.AddListener(x => LightClear());
    }

    void LightClear()
    {
        if (lever.value) // true일때 
        {
            lever.interactionLayers = 0; // 레이어 Nothing으로
            puzzle.LightClear(); // 조명 초기화
            StartCoroutine(LeverReset()); // 잠시 대기
        }
    }

    IEnumerator LeverReset()
    {
        yield return new WaitForSeconds(0.5f);
        lever.value = false; // 레버 원위치로
        lever.interactionLayers = originLayerMask; // 레이어 다시 설정
    }
}
