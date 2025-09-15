using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwimMovement : MonoBehaviour
{
    Transform cameraTr;
    CharacterController controller;
    public float swimSpeed = 5f;
    public float walkSpeed = 3f;

    [SerializeField] InputActionProperty leftMoveAction; // 왼쪽 조이스틱
    [SerializeField] InputActionProperty rightMoveAction; // 오른쪽 조이스틱 

    // 중력 처리를 위한 변수
    Vector3 playerVelocity;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        cameraTr = Camera.main.transform;
    }

    void Update()
    {
        // 현재 위치가 수면(y=0)보다 아래인지 위인지 분리
        if(transform.position.y < 0)
        {
            // 물속에 있을때 (수영)
            HandleSwimming();
        }
        else
        {
            // 물밖에 있을때 (걷기)
            HandleWalking();
        }
    }
    void HandleSwimming()
    {
        Vector2 leftStickInput = leftMoveAction.action.ReadValue<Vector2>();
        Vector2 rightStickInput = rightMoveAction.action.ReadValue<Vector2>();

        if (leftStickInput == Vector2.zero && rightStickInput == Vector2.zero) return;

        // 왼쪽 조이스틱으로 카메라 기준 이동
        Vector3 forwardMove = cameraTr.forward * leftStickInput.y;
        Vector3 rightMove = cameraTr.right * leftStickInput.x;

        // 오른쪽 조이스틱으로 수직 상승/하강
        Vector3 verticalMove = Vector3.up * rightStickInput.y;

        Vector3 moveDir = forwardMove + rightMove + verticalMove;

        // 대각선 이동 속도 보정(정규화)
        if (moveDir.magnitude > 1)
            moveDir.Normalize();

        controller.Move(moveDir * swimSpeed * Time.deltaTime);
    }

    void HandleWalking()
    {
        // CharacterController가 땅에 붙었있는지 확인
        bool isGrounded = controller.isGrounded;
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f; // 땅에 붙어있으면 중력 초기화
        }
        Vector2 leftStickInput = leftMoveAction.action.ReadValue<Vector2>();
        Vector2 rightStickInput = rightMoveAction.action.ReadValue<Vector2>();
        if (leftStickInput == Vector2.zero && rightStickInput == Vector2.zero) return;
        // 보트 위에서는 수평 이동만 가능
        Vector3 forward = cameraTr.forward;
        Vector3 right = cameraTr.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        // 왼쪽 조이스틱으로 앞뒤, 좌우 수평 이동
        Vector3 moveDir = (forward * leftStickInput.y) + (right * leftStickInput.x);

        // 물 밖에서는 오른쪽 조이스틱 아래 입력만 허용
        if (rightStickInput.y < 0)
        {
            // 아래로 내려가는 힘만 추가 (다시 잠수하기 위해)
            moveDir += Vector3.up * rightStickInput.y; // 수직 하강
        }

        controller.Move(moveDir * walkSpeed * Time.deltaTime);

        // 중력 적용
        playerVelocity.y += Physics.gravity.y * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}
