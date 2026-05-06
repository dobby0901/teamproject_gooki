using UnityEngine;

public class Meteor : MonoBehaviour
{
    // 메테오가 떨어지는 속도
    [SerializeField] private float fallSpeed = 20f;

    [Header("자동 삭제 시간")]

    // 메테오가 충돌하지 않아도
    // 일정 시간이 지나면 자동으로 삭제되도록 하는 시간
    [SerializeField] private float lifeTime = 5f;

    private void Start()
    {
        // 생성된 후 lifeTime 초 뒤 자동 삭제
        // 메테오가 맵 어딘가에 남는 현상 방지
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        // 매 프레임마다 아래 방향으로 이동
        // Time.deltaTime을 사용해서 프레임에 상관없이 일정 속도 유지
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 무언가와 충돌했을 경우 메테오 삭제
        // 현재는 플레이어 / 바닥 구분 없이 삭제됨

        // 나중에 여기서:
        // 플레이어 데미지
        // 폭발 이펙트
        // 화면 흔들림
        // 사운드
        // 등을 추가 가능

        Destroy(gameObject);
    }
}