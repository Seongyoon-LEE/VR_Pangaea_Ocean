using UnityEngine;
using UnityEngine.InputSystem;

// AudioSource 컴포넌트가 꼭 필요하다고 미리 알려주자! (없으면 자동으로 추가됨)
[RequireComponent(typeof(AudioSource))]
public class SwimMovement : MonoBehaviour
{
    // --- 변수 선언 ---
    [Header("이동 속도")]
    public float swimSpeed = 10f;
    public float walkSpeed = 3f;

    [Header("입력 액션")]
    [SerializeField] private InputActionProperty leftMoveAction; // 왼쪽 조이스틱
    [SerializeField] private InputActionProperty rightMoveAction; // 오른쪽 조이스틱 

    [Header("사운드 설정")]
    [SerializeField] private AudioClip splashInSound;  // 물에 들어갈 때 사운드
    [SerializeField] private AudioClip splashOutSound; // 물에서 나올 때 사운드

    [Header("수면 경계 설정")]
    [Tooltip("이 y값보다 아래로 내려가야 '물 속'으로 판정")]
    [SerializeField] private float enterWaterY = -0.2f;
    [Tooltip("이 y값보다 위로 올라와야 '물 밖'으로 판정")]
    [SerializeField] private float exitWaterY = 0.2f;

    // --- 내부 변수들 ---
    private Transform cameraTr;
    private CharacterController controller;
    private Vector3 playerVelocity;
    private AudioSource audioSource;
    private bool wasUnderwaterLastFrame;
    public bool IsUnderwater { get; private set; }


    // --- 유니티 생명주기 함수 ---
    void Start()
    {
        controller = GetComponent<CharacterController>();
        cameraTr = Camera.main.transform;
        audioSource = GetComponent<AudioSource>();

        // 게임 시작 시, 현재 물 속에 있는지 아닌지 미리 기억해둔다
        wasUnderwaterLastFrame = transform.position.y < enterWaterY;
        IsUnderwater = wasUnderwaterLastFrame;
    }

    void Update()
    {
        // 1. '지금' 상태 결정하기 (여유 공간 적용)
        UpdateWaterStatus();

        // 2. 상태 변화 감지하고 사운드 재생하기
        CheckAndPlaySplashSounds();

        // 3. 상태에 따라 다른 이동 로직 실행
        if (IsUnderwater)
        {
            HandleSwimming();
        }
        else
        {
            HandleWalking();
        }

        // 4. 다음 프레임을 위해 '지금' 상태를 '지난 프레임' 상태로 저장
        wasUnderwaterLastFrame = IsUnderwater;
    }


    // --- 로직 함수들 ---

    void UpdateWaterStatus()
    {
        float playerY = transform.position.y;

        if (wasUnderwaterLastFrame)
        {
            // 이전에 물 속에 있었다면 -> 물 밖으로 나가는지만 확인
            if (playerY > exitWaterY)
            {
                IsUnderwater = false;
            }
        }
        else
        {
            // 이전에 물 밖에 있었다면 -> 물 속으로 들어가는지만 확인
            if (playerY < enterWaterY)
            {
                IsUnderwater = true;
            }
        }
    }

    void CheckAndPlaySplashSounds()
    {
        // 물 밖으로 나왔을 때
        if (wasUnderwaterLastFrame && !IsUnderwater)
        {
            if (splashOutSound != null)
                audioSource.PlayOneShot(splashOutSound);
        }
        // 물 속으로 들어갔을 때
        else if (!wasUnderwaterLastFrame && IsUnderwater)
        {
            if (splashInSound != null)
                audioSource.PlayOneShot(splashInSound);
        }
    }

    void HandleSwimming()
    {
        Vector2 leftStickInput = leftMoveAction.action.ReadValue<Vector2>();
        Vector2 rightStickInput = rightMoveAction.action.ReadValue<Vector2>();

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
        // 땅에 붙어있는지 확인
        bool isGrounded = controller.isGrounded;
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f; // 중력 초기화
        }

        Vector2 leftStickInput = leftMoveAction.action.ReadValue<Vector2>();
        Vector2 rightStickInput = rightMoveAction.action.ReadValue<Vector2>();

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
            moveDir += Vector3.up * rightStickInput.y;
        }

        controller.Move(moveDir * walkSpeed * Time.deltaTime);

        // 중력 적용
        playerVelocity.y += Physics.gravity.y * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}