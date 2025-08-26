using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwimMovement : MonoBehaviour
{
    Transform cameraTr;
    CharacterController controller;
    [SerializeField] float swimSpeed = 5f;

    [SerializeField] InputActionProperty leftMoveAction; // 왼쪽 조이스틱
    [SerializeField] InputActionProperty rightMoveAction; // 오른쪽 조이스틱 
    void Start()
    {
        controller = GetComponent<CharacterController>();
        cameraTr = Camera.main.transform;
    }

    void Update()
    {
        // 왼쪽 오른쪽 조이스틱 입력값 읽어오기 (Vector2)
        Vector2 leftStickInput = leftMoveAction.action.ReadValue<Vector2>();
        Vector2 rightStickInput = rightMoveAction.action.ReadValue<Vector2>();

        if(leftStickInput.magnitude < 0.1f && rightStickInput.magnitude < 0.1f)
        {
            return;
        }

        // 왼쪽 조이스틱 상하 입력 -> 앞뒤 이동 (카메라가 바라보는 방향)
        Vector3 forwardMove = cameraTr.forward * leftStickInput.y;
        // 오른쪽 조이스틱 상하 입력 -> 위아래 이동
        Vector3 verticalMove = Vector3.up * rightStickInput.y;

        // 두 힘을 합쳐서 이동 방향 결정
        Vector3 moveDir = forwardMove + verticalMove;
        //Vector3 moveDir = Vector3.zero;

        if(moveDir.magnitude > 1) // 정규화를 통한 속도 일정하게 유지
        {
            moveDir.Normalize();
        }
        //if (Input.GetKey(KeyCode.Space))
        //{
        //    // 스페이스바를 누르면 위로 이동
        //    moveDir += Vector3.up;
        //}
        //if(Input.GetKey(KeyCode.LeftControl))
        //{
        //    // 왼쪽 컨트롤키를 누르면 아래로 이동
        //    moveDir += Vector3.down;
        //}
        controller.Move(moveDir * swimSpeed * Time.deltaTime);
    }
}
