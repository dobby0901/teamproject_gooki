using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("이동 설정")]
    [SerializeField] private float moveSpeed = 5f;       // 이동 속도
    [SerializeField] private float rotationSpeed = 15f;  // 회전 속도

    [Header("참조")]
    [SerializeField] private Camera mainCamera;          // 마우스 위치 계산용 카메라
    [SerializeField] private Animator animator;         // 애니메이터 참조

    private CharacterController controller;              // 이동용 컴포넌트
    private Vector2 moveInput;                           // WASD 입력값

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    private void Update()
    {
        Move();     // 이동 처리
        Rotate();   // 마우스 방향 회전
    }

    // PlayerInput의 Behavior가 Send Messages일 때 호출됨
    public void OnMove(InputValue value)
    {
        // Move 액션에서 들어온 Vector2 값을 읽음
        moveInput = value.Get<Vector2>();
    }

    private void Move()
    {
        // 탑뷰 기준 이동 방향
        Vector3 moveDir = new Vector3(moveInput.x, 0f, moveInput.y);

        // 대각선 이동이 빨라지는 것 방지
        if (moveDir.magnitude > 1f)
            moveDir.Normalize();

        // CharacterController로 이동
        controller.Move(moveDir * moveSpeed * Time.deltaTime);

        // 애니메이션 속도 전달
        float speed = moveDir.magnitude; // 0 or 1
        animator.SetFloat("Speed", speed);
    }

    private void Rotate()
    {
        if (mainCamera == null) return;
        if (Mouse.current == null) return;

        Vector2 mousePos = Mouse.current.position.ReadValue();

        Ray ray = mainCamera.ScreenPointToRay(mousePos);

        Plane plane = new Plane(Vector3.up, transform.position);

        if (plane.Raycast(ray, out float distance))
        {
            Vector3 hitPoint = ray.GetPoint(distance);

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
}