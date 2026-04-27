using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target; // 플레이어

    [Header("Base Offset")]
    [SerializeField] private Vector3 baseOffset = new Vector3(0, 12, -8);

    [Header("Follow Settings")]
    [SerializeField] private float followSpeed = 10f;

    [Header("Mouse Move Settings")]
    [SerializeField] private float maxOffsetDistance = 5f; // 최대 이동 거리
    [SerializeField] private float mouseInfluence = 0.01f; // 마우스 영향도

    private Camera cam;
    private Vector3 currentOffset;

    private void Awake()
    {
        cam = Camera.main;
        currentOffset = baseOffset;
    }

    private void LateUpdate()
    {
        UpdateOffset();
        Follow();
    }

    private void UpdateOffset()
    {
        // 우클릭 누르고 있을 때만 작동
        if (Mouse.current.rightButton.isPressed)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();

            // 화면 중심 기준으로 방향 계산
            Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Vector2 delta = mousePos - screenCenter;

            // 방향 벡터 (카메라 평면 기준)
            Vector3 offsetDir = new Vector3(delta.x, 0f, delta.y) * mouseInfluence;

            // 거리 제한
            offsetDir = Vector3.ClampMagnitude(offsetDir, maxOffsetDistance);

            currentOffset = baseOffset + offsetDir;
        }
        else
        {
            // 우클릭 안하면 원래 위치로 복귀
            currentOffset = Vector3.Lerp(currentOffset, baseOffset, 5f * Time.deltaTime);
        }
    }

    private void Follow()
    {
        Vector3 targetPos = target.position + currentOffset;

        transform.position = Vector3.Lerp(
            transform.position,
            targetPos,
            followSpeed * Time.deltaTime
        );
    }
}