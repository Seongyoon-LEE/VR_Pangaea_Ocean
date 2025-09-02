using UnityEngine;
using UnityEngine.InputSystem; // Input System을 사용하려면 필수!

public class ScooterController : MonoBehaviour
{
    [Header("스쿠터 설정")]
    [SerializeField] private float scooterSpeed = 20f; // 스쿠터 부스터 속도
    [SerializeField] private InputActionProperty rightGrabAction; // 인스펙터에서 오른쪽 그랩 액션을 연결

    [Header("플레이어 참조")]
    // 인스펙터에서 플레이어의 SwimMovement 스크립트를 연결
    [SerializeField] private SwimMovement swimMovement;

    private CharacterController playerController;
    private Transform cameraTransform;
    private bool isBoosting = false; // 현재 부스터가 켜져 있는지 확인하는 스위치

    void Awake()
    {
        // Null 체크는 항상 좋은 습관!
        if (swimMovement != null)
        {
            playerController = swimMovement.GetComponent<CharacterController>();
        }

        // VR 카메라의 Transform 정보를 가져온다.
        if (Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        // 필요한 정보가 없으면 실행하지 않음 (안전장치)
        if (playerController == null || cameraTransform == null || swimMovement == null)
        {
            return;
        }

        // 오른쪽 그랩 버튼을 얼마나 눌렀는지 값을 읽어온다 (0.0 ~ 1.0)
        float grabValue = rightGrabAction.action.ReadValue<float>();

        // 1. 그랩 버튼을 꾹 누르고 있을 때 (부스터 켜기 & 유지)
        if (grabValue > 0.1f)
        {
            // 만약 부스터가 꺼져 있었다면, 켜주는 처리를 한다.
            if (!isBoosting)
            {
                isBoosting = true;
                swimMovement.enabled = false; // 기본 수영 기능 비활성화
                Debug.Log("부스터 ON!");
            }

            // 부스터가 켜져 있는 동안 계속 실행
            // 카메라가 바라보는 정면 방향으로
            Vector3 moveDirection = cameraTransform.forward;
            // 플레이어를 스쿠터 속도에 맞춰 이동!
            playerController.Move(moveDirection * scooterSpeed * Time.deltaTime);
        }
        // 2. 그랩 버튼에서 손을 뗐을 때 (부스터 끄기)
        else
        {
            // 만약 부스터가 켜져 있었다면, 꺼주는 처리를 한다.
            if (isBoosting)
            {
                isBoosting = false;
                swimMovement.enabled = true; // 기본 수영 기능 다시 활성화
                Debug.Log("부스터 OFF!");
            }
        }
    }
}