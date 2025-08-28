using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit;

public class Puzzle : MonoBehaviour
{
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
    private TMP_Text valueTxt;
    private Coroutine valueUpdate;
    void Start()
    {
        knob = GetComponent<XRKnob>();
        knob.selectEntered.AddListener(x =>
        {
            valueUpdate = StartCoroutine(ValueView());
        });
        knob.selectExited.AddListener(SelectColor);
        originLayerMask = knob.interactionLayers;
        vorota = GameObject.Find("Vorota").GetComponentInChildren<Animator>();
        valueTxt = GetComponentInChildren<TMP_Text>();
    }

    private void SelectColor(SelectExitEventArgs args)
    {
        StopCoroutine(valueUpdate);
        ValueResult(); // value값 0~1 사이값으로 변환
        knob.interactionLayers = 0; // 레이어 Nothing으로 설정
        
        Answer(); // 정답 확인
        StartCoroutine(SelectClear()); // 처음 위치로
    }

    private void Answer()
    {
        // 여기 문제에 맞게 수정해야함
        
        if (knob.value >= 0.5 && knob.value < 0.7)
        {
            print("정답");
            _light[curQuestion++].MaterialSetting((int)State.TRUE);
            answerCount++;
        }
        else
        {
            print("오답");
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
        // value값이 0이 되게 하기
        knob.value = Mathf.Clamp(knob.value, 0.0f, 1.0f);
        // 다시 레이어를 설정하여 작동할 수 있게 하기
        if (curQuestion < 3)
            knob.interactionLayers = originLayerMask;
    }

    // 레버를 내렸을 때 조명 초기화
    public void LightClear()
    {
        foreach (var m in _light)
        {
            m.MaterialSetting((int)State.NOMAL);
        }
        if (knob.interactionLayers == 0)
            knob.interactionLayers = originLayerMask;
        curQuestion = answerCount = 0; // 현재 문제, 맞춘 갯수 초기화
        vorota.SetBool(hashIsOpen, false);
    }
    private float ValueResult()
    {
        knob.value %= 1.0f; // value 는 0~1 사이값
        if (knob.value < 0) // 음수일 때 정수로 변환
            knob.value += 1;
        return knob.value;
    }
    IEnumerator ValueView()
    {
        while(true)
        {
            valueTxt.text = Mathf.Floor(ValueResult() * 10) switch
            {
                0 => "0",
                1 => "1",
                2 => "2",
                3 => "3",
                4 => "4",
                5 => "5",
                6 => "6",
                7 => "7",
                8 => "8",
                9 => "9",
                _ => "10"
            };
            yield return new WaitForSeconds(0.1f);
        }
    }
}
