using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class BoatMoveCtrl : MonoBehaviour
{
    public InputActionProperty moveInput; // 이동 - 드래그 앤 드롭
    public InputActionProperty activateInput; // 트리거 - 드래그 앤 드롭
    private BoatCockpit cockpit;
    private float moveSpeed = 5f;
    private float turnSpeed = 20f;
    void Start()
    {
        cockpit = transform.GetChild(2).GetComponent<BoatCockpit>();
    }

    void Update()
    {
        if (!cockpit.isCockpit) return;

        Vector2 input = moveInput.action.ReadValue<Vector2>();
        Vector3 dir = transform.forward * input.y * moveSpeed * Time.deltaTime;
        transform.position += dir;
        transform.Rotate(Vector3.up * input.x * turnSpeed *  Time.deltaTime);

        if (activateInput.action.WasPressedThisFrame())
        {
            cockpit.PlayerEnable(true);
        }
    }
}
