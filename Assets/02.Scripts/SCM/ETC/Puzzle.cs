using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit;

public class Puzzle : MonoBehaviour
{
    private readonly string[] puzzleStr = { "Puzzle1", "Puzzle2", "Puzzle3" };
    private enum State
    {
        NOMAL, TRUE, FALSE
    }
    private readonly int hashIsOpen = Animator.StringToHash("isOpen");
    private XRKnob knob;
    public List<LightMaterial> _light; // 드래그 앤 드롭
    public int curQuestion = 0; // 현재 진행도
    private int answerCount = 0; // 맞은 갯수
    InteractionLayerMask originLayerMask; // 레이어를 변경해 원래 자리로 돌아갈 때 작동을 막기
    private Animator vorota; // Gate Animator
    private Image valueImage;
    private Coroutine valueUpdate;
    
    IEnumerator Start()
    {
        while (!DataManager.Instance.IsLoadingFinish)
        {
            yield return null;
        }

        knob = GetComponent<XRKnob>();
        knob.selectEntered.AddListener(x =>
        {
            // 현재 선택된 색상을 보여주는 코루틴
            // StopCoroutine을 위해 변수에 저장
            valueUpdate = StartCoroutine(ValueView()); 
            
        });
        knob.selectExited.AddListener(SelectColor);
        originLayerMask = knob.interactionLayers; // 현재 레이어 저장
        vorota = GameObject.Find("Vorota").GetComponentInChildren<Animator>(); // 성문을 열기 위한 애니메이터
        valueImage = GetComponentInChildren<Image>(); // 현재 선택된 색을 보여주기 위한 이미지

        for (int i = 0; i < puzzleStr.Length; i++)
        {
            // 키가 있으면 값을 가져오기
            if (DataManager.Instance.dicPuzzle.TryGetValue(puzzleStr[i], out bool value))
            {
                print(puzzleStr[i]);
                // True일때 파랑색, False일때 빨강색으로 바꿔준다.
                _light[i].MaterialSetting(value ? (int)State.TRUE : (int)State.FALSE);
            }
        }
    }

    // 정답 선택
    private void SelectColor(SelectExitEventArgs args)
    {
        StopCoroutine(valueUpdate); // 선택된 색상을 보여주기 위한 코루틴 종료
        knob.interactionLayers = 0; // 레이어 Nothing으로 설정
        
        Answer(); // 정답 확인
        StartCoroutine(SelectClear()); // 처음 위치로
    }

    private void Answer()
    {
        float select = Mathf.Floor(ValueResult() * 10); // 현재 값을 0~9로 변환
        if (select == GameManager.Instance.answerArr[curQuestion])
        {
            print("정답");
            // 퍼즐 정보 갱신
            if (DataManager.Instance.dicPuzzle.ContainsKey(puzzleStr[curQuestion])) // 이미 키가 존재하면 값만 변경
                DataManager.Instance.dicPuzzle[puzzleStr[curQuestion]] = true;
            else  // 키가 존재하지 않으면 Add 하여 키와 값 넣기
                DataManager.Instance.dicPuzzle.Add(puzzleStr[curQuestion], true);
            _light[curQuestion++].MaterialSetting((int)State.TRUE);
            answerCount++;
        }
        else
        {
            print("오답");
            if (DataManager.Instance.dicPuzzle.ContainsKey(puzzleStr[curQuestion]))
                DataManager.Instance.dicPuzzle[puzzleStr[curQuestion]] = false;
            else
                DataManager.Instance.dicPuzzle.Add(puzzleStr[curQuestion], false);
            _light[curQuestion++].MaterialSetting((int)State.FALSE);
        }

        // 3문제 맞추면 Gate Open
        if (answerCount == 3)
        {
            vorota.SetBool(hashIsOpen, true);
        }
    }
    IEnumerator SelectClear()
    {
        // wheel 처음 위치로 돌리기
        while (knob.value > 0)
        {
            knob.value -= 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        // value값이 0보다 작아지는걸 막기위한 코드
        knob.value = Mathf.Clamp(knob.value, 0.0f, 1.0f);
        // 다시 레이어를 설정하여 작동할 수 있게 하기
        if (curQuestion < 3)
            knob.interactionLayers = originLayerMask;
    }

    // 레버를 내렸을 때 조명 초기화
    public void LightClear()
    {
        // 조명 처음 상태로 되돌리기
        foreach (var m in _light)
        {
            m.MaterialSetting((int)State.NOMAL);
        }

        // 퍼즐 데이터 삭제
        GameManager.Instance.puzzleDicReset("Puzzle");

        // 레이어를 다시 원래 상태로 되돌리기
        if (knob.interactionLayers == 0)
            knob.interactionLayers = originLayerMask;
        curQuestion = answerCount = 0; // 현재 문제, 맞춘 갯수 초기화
        vorota.SetBool(hashIsOpen, false); // 성문 닫기
        print("초기화");
        foreach (var dic in DataManager.Instance.dicPuzzle)
        {
            print($"{dic.Key} : {dic.Value}");
        }
    }
    private float ValueResult()
    {
        knob.value %= 1.0f; // value 는 0~1 사이값
        if (knob.value < 0) // 역방향으로 돌리면 음수가 나오기 때문에 양수로 변환
            knob.value += 1;
        return knob.value;
    }
    IEnumerator ValueView()
    {
        // 이미지 색상 변환 하는 코루틴
        // StopCoroutine으로 종료
        while (true)
        {
            valueImage.color = Mathf.Floor(ValueResult() * 10) switch
            {
                0 => new Color32(255, 0, 0, 255), // 빨강
                1 => new Color32(0, 255, 0, 255), // 초록
                2 => new Color32(0, 0, 255, 255), // 블루
                3 => new Color32(255, 255, 0, 255), // 노랑
                4 => new Color32(0, 255, 255, 255), // 시안
                5 => new Color32(255, 0, 255, 255), // 마젠타
                6 => new Color32(255, 127, 0, 255), // 오렌지
                7 => new Color32(255, 255, 255, 255), // 흰색
                8 => new Color32(255, 105, 180, 255), // 분홍
                9 => new Color32(0, 0, 0, 255), // 검정
                _ => new Color32(0, 0, 0, 0) // 클리어
            };
            yield return new WaitForSeconds(0.1f);
        }
    }
}
