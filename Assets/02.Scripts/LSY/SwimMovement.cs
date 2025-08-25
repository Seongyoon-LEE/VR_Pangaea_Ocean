using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwimMovement : MonoBehaviour
{
    Transform cameraTr;
    CharacterController controller;
    [SerializeField] float swimSpeed = 5f;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        cameraTr = Camera.main.transform;
    }

    void Update()
    {
        Vector3 moveDir = Vector3.zero;
        if(Input.GetKey(KeyCode.W))
        {
            // W키를 누르면 카메라가 바라보는 방향으로 이동
            moveDir += cameraTr.forward;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            // 스페이스바를 누르면 위로 이동
            moveDir += Vector3.up;
        }
        if(Input.GetKey(KeyCode.LeftControl))
        {
            // 왼쪽 컨트롤키를 누르면 아래로 이동
            moveDir += Vector3.down;
        }
        controller.Move(moveDir * swimSpeed * Time.deltaTime);
    }
}
