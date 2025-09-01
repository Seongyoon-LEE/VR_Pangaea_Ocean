using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class JetPackCtrl : MonoBehaviour
{
    [SerializeField] InputActionProperty rightGrabAction; // 오른쪽 그랩

    [SerializeField] SwimMovement swimMovement;
    [SerializeField] XRSimpleInteractable interactable;
    [SerializeField] float originSwimSpeed;

    public bool isBoosterOn;
    Vector3 orginPos;
    private void OnEnable()
    {
        
            interactable.selectEntered.AddListener(OnGrab);
        
        interactable.selectExited.AddListener(OnRelease);
    }
    private void OnDisable()
    {
        interactable.selectEntered.RemoveListener(OnGrab);
        interactable.selectExited.RemoveListener(OnRelease);
    }
    private void OnGrab(SelectEnterEventArgs args)
    {
        print(interactable.interactorsSelecting.Count);
        // 양손, 즉 2개일 때만 부스터를 켠다
        if (interactable.interactorsSelecting.Count == 2) // 양손 그랩 
        {
            Debug.Log("양손 그랩! 부스터 ON!");
            swimMovement.swimSpeed = 15f;
            isBoosterOn = true;
        }
    }

    // 누군가 이 오브젝트를 놓았을 때 호출될 함수
    private void OnRelease(SelectExitEventArgs args)
    {
        // 부스터가 켜져 있었다면, 원래 속도로 되돌린다.
        if (isBoosterOn)
        {
            Debug.Log("그랩 해제! 부스터 OFF!");
            swimMovement.swimSpeed = originSwimSpeed;
            StartCoroutine(ReleaseRoutine());

        }
    }
    IEnumerator ReleaseRoutine()
    {
        while(this.transform.parent == null)
        {
            yield return null;
        }
        this.transform.localPosition = orginPos;
        this.transform.localRotation = Quaternion.identity;
        isBoosterOn = false;
    }
    private void Awake()
    {
        swimMovement = GameObject.FindObjectOfType<SwimMovement>();
        if (swimMovement != null)
        {
            originSwimSpeed = swimMovement.swimSpeed;
        }
        orginPos = transform.localPosition;
        interactable = GetComponent <XRSimpleInteractable>();
    }

    void Update()
    {
        print(swimMovement.swimSpeed);
    }
}
