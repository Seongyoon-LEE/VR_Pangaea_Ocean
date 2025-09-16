using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Turtle : MonoBehaviour
{
    private readonly string[] turtleStr = { "Turtle1", "Turtle2", "Turtle3" };
    private GameObject grass;
    private GameObject key;
    private XRGrabInteractable keyGrab;
    private InteractionLayerMask originLayerMask;
    public int stage = 0; // 스테이지를 인스펙터에서 수정
    public bool test = false;
    [SerializeField] private MeshRenderer meshRenderer;
    public List<Material> materials; // 머터리얼 넣을 때 퍼즐 정답 순서랑 맞춰서 넣기
    private Material[] originMaterials;
    
    public int idx = 0; // 거북이 순서에 맞춰 인스펙터에서 수정 (0~2)
    IEnumerator Start()
    {
        while (!DataManager.Instance.IsLoadingFinish)
        {
            yield return null;
        }

        grass = transform.GetChild(1).gameObject;
        key = transform.GetChild(2).gameObject;
        keyGrab = key.GetComponent<XRGrabInteractable>();
        originLayerMask = keyGrab.interactionLayers;
        keyGrab.interactionLayers = 0; // 레이어 Nothing으로
        meshRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
        
        // 3스테이지에서만 보이게
        if (stage != 3)
            key.SetActive(false);

        // 현재 거북이 상태값에 대한 키가 존재하는 지 확인하고 그 값을 가져온다.
        if (DataManager.Instance.dicPuzzle.TryGetValue(turtleStr[idx], out bool value))
        {
            if (value) // true라면 거북이가 해초에서 구해진 상태로 바꾼다.
                GrassDisable();
        }
    }

    // 테스트용
    private void Update()
    {
        if (test)
            GrassDisable();
    }
    public void GrassDisable()
    {
        if (!grass.activeSelf) return;

        transform.GetComponent<BoxCollider>().enabled = false;
        grass.SetActive(false);

        if (DataManager.Instance.dicPuzzle.ContainsKey(turtleStr[idx])) // 키가 존재한다면 값만 변경
            DataManager.Instance.dicPuzzle[turtleStr[idx]] = true;
        else // 키가 존재하지 않으면 키와 값을 넣기
            DataManager.Instance.dicPuzzle.Add(turtleStr[idx], true);

        // 무게량 증가 (중첩 가능)
        // 스테이지 이동시 초기화
        // 사망시 초기화
        GameManager.Instance.BuffApp();

        // 3스테이지 일때 키 생성과 거북이 등껍질 색상 변경
        if (stage == 3)
        {
            KeySetting(); // 키 활성화 및 레이어 변경
            ChangeMaterials(); // 거북이 등껍질 변경
        }
    }
    
    private void KeySetting()
    {
        key.transform.parent = null; // 거북이 밖으로 꺼내기
        key.tag = "Key"; // 키 태그 변경
        key.GetComponent<BoxCollider>().isTrigger = false;
        keyGrab.interactionLayers = originLayerMask; // 원래 레이어로 변경
    }
    private void ChangeMaterials()
    {
        originMaterials = meshRenderer.materials; // 변경하기 위해서 복사

        if (meshRenderer == null || materials == null || originMaterials == null) return;

        originMaterials[3] = materials[GameManager.Instance.answerArr[idx]]; // 등껍질 변경
        meshRenderer.materials = originMaterials; // 머터리얼 넣기
    }
}
