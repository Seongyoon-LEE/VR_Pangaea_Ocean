using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(AudioSource))]
public class SwimMovement : MonoBehaviour
{
    // --- 변수 선언 ---
    [Header("이동 속도")]
    public float swimSpeed = 10f;
    public float walkSpeed = 3f;

    [Header("입력 액션")]
    [SerializeField] private InputActionProperty leftMoveAction;
    [SerializeField] private InputActionProperty rightMoveAction;

    [Header("사운드 설정")]
    [SerializeField] private AudioClip splashInSound;
    [SerializeField] private AudioClip splashOutSound;

    // --- ? 아래 코드 추가! ---
    [Tooltip("물에 들어가거나 나온 후, 몇 초 동안 다시 소리가 나지 않게 할지 설정")]
    [SerializeField] private float splashSoundCooldown = 2f; // 2초의 쿨다운

    [Header("수면 경계 설정")]
    [SerializeField] private float enterWaterY = -0.2f;
    [SerializeField] private float exitWaterY = 0.2f;

    // --- 내부 변수들 ---
    private Transform cameraTr;
    private CharacterController controller;
    private Vector3 playerVelocity;
    private AudioSource audioSource;
    private bool wasUnderwaterLastFrame;
    public bool IsUnderwater { get; private set; }

    // --- ? 아래 코드 추가! ---
    private float lastSplashTime = -999f; // 마지막으로 첨벙 소리를 낸 시간을 기록


    // --- 유니티 생명주기 함수 ---
    void Start()
    {
        controller = GetComponent<CharacterController>();
        cameraTr = Camera.main.transform;
        audioSource = GetComponent<AudioSource>();

        wasUnderwaterLastFrame = transform.position.y < enterWaterY;
        IsUnderwater = wasUnderwaterLastFrame;
    }

    void Update()
    {
        UpdateWaterStatus();
        CheckAndPlaySplashSounds(); // 사운드 체크 함수 호출

        if (IsUnderwater)
        {
            HandleSwimming();
        }
        else
        {
            HandleWalking();
        }

        wasUnderwaterLastFrame = IsUnderwater;
    }

    // --- 로직 함수들 ---

    void UpdateWaterStatus()
    {
        // ... (이전과 동일)
        float playerY = transform.position.y;

        if (wasUnderwaterLastFrame)
        {
            if (playerY > exitWaterY) IsUnderwater = false;
        }
        else
        {
            if (playerY < enterWaterY) IsUnderwater = true;
        }
    }

    // --- ? 사운드 재생 함수 수정! ---
    void CheckAndPlaySplashSounds()
    {
        // 1. 마지막으로 소리를 낸 지 쿨다운(2초) 시간이 지났는지 먼저 확인!
        if (Time.time < lastSplashTime + splashSoundCooldown)
        {
            return; // 아직 쿨다운 중이면 아무것도 안 함
        }

        // 2. 쿨다운이 끝났을 때만 상태 변화를 확인
        bool playedSound = false; // 소리를 재생했는지 확인하는 스위치

        if (wasUnderwaterLastFrame && !IsUnderwater)
        {
            // 물 밖으로 나옴
            if (splashOutSound != null)
                audioSource.PlayOneShot(splashOutSound);
            playedSound = true;
        }
        else if (!wasUnderwaterLastFrame && IsUnderwater)
        {
            // 물 속으로 들어감
            if (splashInSound != null)
                audioSource.PlayOneShot(splashInSound);
            playedSound = true;
        }

        // 3. 만약 소리를 재생했다면, 지금 시간을 기록!
        if (playedSound)
        {
            lastSplashTime = Time.time;
        }
    }
    void HandleSwimming()
    {
        Vector2 leftStickInput = leftMoveAction.action.ReadValue<Vector2>();
        Vector2 rightStickInput = rightMoveAction.action.ReadValue<Vector2>();

        Vector3 forwardMove = cameraTr.forward * leftStickInput.y;
        Vector3 rightMove = cameraTr.right * leftStickInput.x;
        Vector3 verticalMove = Vector3.up * rightStickInput.y;
        Vector3 moveDir = forwardMove + rightMove + verticalMove;

        if (moveDir.magnitude > 1) moveDir.Normalize();
        controller.Move(moveDir * swimSpeed * Time.deltaTime);
    }

    void HandleWalking()
    {
        bool isGrounded = controller.isGrounded;
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }
        Vector2 leftStickInput = leftMoveAction.action.ReadValue<Vector2>();
        Vector2 rightStickInput = rightMoveAction.action.ReadValue<Vector2>();

        Vector3 forward = cameraTr.forward;
        Vector3 right = cameraTr.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();
        Vector3 moveDir = (forward * leftStickInput.y) + (right * leftStickInput.x);

        if (rightStickInput.y < 0)
        {
            moveDir += Vector3.up * rightStickInput.y;
        }

        controller.Move(moveDir * walkSpeed * Time.deltaTime);

        playerVelocity.y += Physics.gravity.y * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}