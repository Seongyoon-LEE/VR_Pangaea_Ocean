using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit;

public class Puzzle : MonoBehaviour
{
    private readonly int hashIsOpen = Animator.StringToHash("isOpen");
    private XRKnob knob;
    public List<GameObject> _light;
    public int curQuestion = 0;
    private int answerCount = 0;
    InteractionLayerMask originLayerMask;
    void Start()
    {
        knob = GetComponent<XRKnob>();
        knob.selectExited.AddListener(SelectColor);
        originLayerMask = knob.interactionLayers;
    }

    private void SelectColor(SelectExitEventArgs args)
    {
        knob.interactionLayers = 0;
        knob.value %= 1.0f;
        if (knob.value < 0)
            knob.value += 1;
        Answer();
        StartCoroutine(SelectClear());
    }

    private void Answer()
    {
        // 여기 문제에 맞게 수정해야함
        if (knob.value >= 0.5 && knob.value < 0.7)
        {
            print("정답");
            _light[curQuestion++].GetComponent<PuzzleMaterial>().OnPuzzleCorrect();
            answerCount++;
        }
        else
        {
            print("오답");
            _light[curQuestion++].GetComponent<PuzzleMaterial>().OnPuzzleIncorrect();
        }

        if (answerCount == 3)
        {
            // 완성되면 나중에 변수로 미리 받아두기
            GameObject.Find("Vorota").GetComponentInChildren<Animator>().SetBool(hashIsOpen, true);
        }
    }
    IEnumerator SelectClear()
    {
        while (knob.value > 0)
        {
            knob.value -= 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        knob.value = Mathf.Clamp(knob.value, 0.0f, 1.0f);
        if (curQuestion < 3)
            knob.interactionLayers = originLayerMask;
    }
}
