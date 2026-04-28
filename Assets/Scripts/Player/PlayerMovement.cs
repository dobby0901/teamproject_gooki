using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Move Settings")]
    [SerializeField] private float moveSpeed = 5f;       // 평상시 이동 속도
    [SerializeField] private float rotationSpeed = 15f;  // 마우스 방향 회전 속도

    [Header("Dodge Settings")]
    [SerializeField] private float dodgeSpeed = 10f;     // 구르기 중 이동 속도
    [SerializeField] private float dodgeDuration = 0.35f; // 구르기 지속 시간
    [SerializeField] private float dodgeCooldown = 0.6f;  // 구르기 쿨타임

    [Header("Reference")]
    [SerializeField] private Camera mainCamera;          // 마우스 위치 계산용 카메라
    [SerializeField] private Animator animator;          // 애니메이션 제어용
    [SerializeField] private PlayerHealth playerHealth;  // 무적 처리용

    private CharacterController controller;              // 이동용 컴포넌트
    private Vector2 moveInput;                           // WASD 입력값
    private WeaponVisibleController weaponVisibleController; //무기 숨기기

    private bool isDodging;                              // 현재 구르기 중인지 체크
    private bool canDodge = true;                        // 구르기 가능 여부
    private Vector3 dodgeDirection;                      // 구르기 방향 저장

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        // 인스펙터에 안 넣었을 경우 자동 연결
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (animator == null)
            animator = GetComponent<Animator>();

        if (playerHealth == null)
            playerHealth = GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        // 구르기 중이면 일반 이동 대신 구르기 이동만 처리
        if (isDodging)
        {
            DodgeMove();
            return;
        }

        Move();     // 일반 이동 처리
        Rotate();   // 마우스 방향 회전 처리
    }

    // PlayerInput의 Move 액션에서 호출됨
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>(); // WASD 입력값 저장
    }

    // PlayerInput의 Dodge 액션에서 호출됨
    public void OnDodge(InputValue value)
    {
        // 버튼이 눌린 순간이 아니면 무시
        if (!value.isPressed) return;

        // 구르기 불가능 상태면 무시
        if (!canDodge) return;

        // 이미 구르기 중이면 무시
        if (isDodging) return;

        StartCoroutine(DodgeRoutine());
    }

    private void Move()
    {
        // 캐릭터 기준 이동 방향 
        Vector3 moveDir = transform.forward * moveInput.y + transform.right * moveInput.x;

        // 대각선 이동 속도 보정
        if (moveDir.magnitude > 1f)
            moveDir.Normalize();

        // CharacterController로 이동
        controller.Move(moveDir * moveSpeed * Time.deltaTime);

        // 이동 애니메이션 Speed 전달
        if (animator != null)
            animator.SetFloat("Speed", moveDir.magnitude, 0.1f, Time.deltaTime);
    }

    private void Rotate()
    {
        if (mainCamera == null) return;
        if (Mouse.current == null) return;

        // 마우스 스크린 좌표 가져오기
        Vector2 mousePos = Mouse.current.position.ReadValue();

        // 마우스 위치를 월드 Ray로 변환
        Ray ray = mainCamera.ScreenPointToRay(mousePos);

        // 플레이어 높이 기준 가상 바닥 생성
        Plane plane = new Plane(Vector3.up, transform.position);

        // Ray와 바닥이 만나는 지점 계산
        if (plane.Raycast(ray, out float distance))
        {
            Vector3 hitPoint = ray.GetPoint(distance);

            // 플레이어에서 마우스 위치까지의 방향
            Vector3 direction = hitPoint - transform.position;
            direction.y = 0f;

            if (direction.sqrMagnitude < 0.001f)
                return;

            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    private IEnumerator DodgeRoutine()
    {
        canDodge = false;   // 쿨타임 시작
        isDodging = true;   // 구르기 상태 시작

        // 캐릭터 기준 구르기 방향
        dodgeDirection = transform.forward * moveInput.y + transform.right * moveInput.x;


        // 입력이 없으면 현재 바라보는 방향으로 구르기
        if (dodgeDirection.sqrMagnitude < 0.001f)
            dodgeDirection = transform.forward;

        // 대각선 구르기 속도 보정
        dodgeDirection.Normalize();

        // 구르기 중 무적 시작
        if (playerHealth != null)
            playerHealth.SetInvincible(true);

        // 구르기 애니메이션 실행
        if (animator != null)
        {
            animator.SetBool("IsDodging", true);
            animator.SetTrigger("Dodge");
            animator.SetFloat("Speed", 0f);
        }

        float timer = 0f;

        // dodgeDuration 동안 구르기 이동
        while (timer < dodgeDuration)
        {
            DodgeMove();
            timer += Time.deltaTime;
            yield return null;
        }

        // 구르기 종료
        isDodging = false;


        // 무적 종료
        if (playerHealth != null)
            playerHealth.SetInvincible(false);

        if (animator != null)
            animator.SetBool("IsDodging", false);

        // 남은 쿨타임 대기
        yield return new WaitForSeconds(dodgeCooldown);

        canDodge = true;
    }

    private void DodgeMove()
    {
        // 저장된 구르기 방향으로 빠르게 이동
        controller.Move(dodgeDirection * dodgeSpeed * Time.deltaTime);

    }
}