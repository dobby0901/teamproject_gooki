using System.Collections;
using UnityEngine;

public class MeteorPatternManager : MonoBehaviour
{
    [Header("플레이어")]

    // 메테오 패턴의 기준이 되는 플레이어
    // 플레이어 주변 랜덤 위치를 계산할 때 사용
    [SerializeField] private Transform player;

    [Header("메테오 설정")]

    // 실제 떨어지는 메테오 프리팹
    [SerializeField] private GameObject meteorPrefab;

    // 메테오가 떨어질 위치를 미리 보여주는 경고 프리팹
    [SerializeField] private GameObject warningPrefab;

    // 메테오 패턴이 반복되는 시간 간격
    // 15초마다 패턴 실행
    [SerializeField] private float patternInterval = 15f;

    // 한 번의 패턴에서 생성되는 메테오 개수
    [SerializeField] private int meteorCount = 6;

    [Header("떨어지는 위치 설정")]

    // 플레이어와 너무 가까운 위치는 제외하기 위한 최소 거리
    [SerializeField] private float minRadius = 1.5f;

    // 플레이어 주변 어디까지 메테오가 떨어질 수 있는지 최대 거리
    [SerializeField] private float maxRadius = 5f;

    [Header("경고 시간")]

    // 메테오가 떨어지기 전 경고를 보여주는 시간
    // 플레이어가 피할 수 있도록 유예 시간 제공
    [SerializeField] private float warningTime = 2f;

    [Header("메테오 생성 높이")]

    // 메테오가 공중 어느 높이에서 생성될지 설정
    [SerializeField] private float meteorSpawnHeight = 15f;

    [Header("패턴 정지 상태")]

    // true면 메테오 패턴 중지
    // 나중에 제단 기믹과 연결 예정
    [SerializeField] private bool isStopped = false;

    // 현재 실행 중인 패턴 코루틴 저장용
    private Coroutine patternCoroutine;

    private void Start()
    {
        // 게임 시작 시 메테오 패턴 루프 시작
        patternCoroutine = StartCoroutine(MeteorPatternLoop());
    }

    private IEnumerator MeteorPatternLoop()
    {
        // 무한 반복 패턴
        while (true)
        {
            // 설정된 시간만큼 대기
            yield return new WaitForSeconds(patternInterval);

            // 패턴이 중지 상태가 아닐 경우 실행
            if (!isStopped)
            {
                StartCoroutine(SpawnMeteorPattern());
            }
        }
    }

    private IEnumerator SpawnMeteorPattern()
    {
        // 메테오가 떨어질 위치 저장 배열
        Vector3[] targetPositions = new Vector3[meteorCount];

        // 생성된 경고 오브젝트 저장 배열
        GameObject[] warnings = new GameObject[meteorCount];

        // 메테오 개수만큼 반복
        for (int i = 0; i < meteorCount; i++)
        {
            // 플레이어 주변 랜덤 위치 계산
            Vector3 randomPos = GetRandomPositionAroundPlayer();

            // 위치 저장
            targetPositions[i] = randomPos;

            // 경고 오브젝트 생성
            warnings[i] = Instantiate(warningPrefab, randomPos, Quaternion.identity);
        }

        // warningTime 동안 경고 표시 유지
        yield return new WaitForSeconds(warningTime);

        // 메테오 생성
        for (int i = 0; i < meteorCount; i++)
        {
            // 경고 오브젝트 삭제
            if (warnings[i] != null)
            {
                Destroy(warnings[i]);
            }

            // 공중 생성 위치 계산
            Vector3 spawnPos = targetPositions[i] + Vector3.up * meteorSpawnHeight;

            // 메테오 생성
            Instantiate(meteorPrefab, spawnPos, Quaternion.identity);
        }
    }

    private Vector3 GetRandomPositionAroundPlayer()
    {
        // 원 내부의 랜덤 방향 계산
        Vector2 randomCircle = Random.insideUnitCircle.normalized;

        // 최소~최대 거리 사이 랜덤 거리 계산
        float randomDistance = Random.Range(minRadius, maxRadius);

        // XZ 평면 기준 랜덤 위치 생성
        Vector3 randomOffset = new Vector3(
            randomCircle.x,
            0f,
            randomCircle.y
        ) * randomDistance;

        // 플레이어 위치 기준으로 최종 좌표 반환
        return player.position + randomOffset;
    }

    public void StopMeteorPattern(float stopTime)
    {
        // 외부에서 호출 시 메테오 패턴 잠시 중지
        // 예: 제단 기믹 사용 성공 시
        StartCoroutine(StopPatternCoroutine(stopTime));
    }

    private IEnumerator StopPatternCoroutine(float stopTime)
    {
        // 패턴 정지
        isStopped = true;

        // 설정된 시간만큼 대기
        yield return new WaitForSeconds(stopTime);

        // 다시 패턴 활성화
        isStopped = false;
    }
}