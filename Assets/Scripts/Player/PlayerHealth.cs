using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("참조")]
    [SerializeField] private PlayerStats stats; // PlayerStats 참조 (최대 체력, 방어력 가져오기)

    [Header("현재 체력")]
    [SerializeField] private float currentHP;   // 현재 체력 (실제 게임에서 줄어드는 값)

    public float CurrentHP => currentHP;        // 외부에서 현재 체력 읽기용
    public float MaxHP => stats != null ? stats.MaxHP : 0f; // 최대 체력 (Stats에서 가져옴)

    public bool IsDead { get; private set; }    // 사망 여부 체크
    public bool IsInvincible { get; private set; } // 무적 상태 (회피 시 사용)

    public event Action<float, float> OnHpChanged; // 체력 변경 이벤트 (UI 업데이트용)
    public event Action OnDie;                     // 사망 이벤트

    private void Awake()
    {
        // PlayerStats가 연결 안됐을 경우 자동으로 가져옴
        if (stats == null)
            stats = GetComponent<PlayerStats>();

        // 시작할 때 체력을 최대 체력으로 설정
        currentHP = MaxHP;

        IsDead = false;
        IsInvincible = false;
    }

    public void TakeDamage(float damage)
    {
        // 이미 죽었으면 데미지 무시
        if (IsDead) return;

        // 무적 상태면 데미지 무시 (구르기 등)
        if (IsInvincible) return;

        // 방어력 가져오기 (없으면 0)
        float defense = stats != null ? stats.Defense : 0f;

        // 최종 데미지 계산 (최소 1 보장)
        float finalDamage = Mathf.Max(damage - defense, 1f);

        // 체력 감소
        currentHP -= finalDamage;

        // 체력 범위 제한 (0 ~ MaxHP)
        currentHP = Mathf.Clamp(currentHP, 0f, MaxHP);

        // 체력 UI 업데이트용 이벤트 호출
        OnHpChanged?.Invoke(currentHP, MaxHP);

        // 체력이 0이면 사망 처리
        if (currentHP <= 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        // 죽었으면 회복 불가
        if (IsDead) return;

        // 0 이하 회복 무시
        if (amount <= 0f) return;

        // 체력 증가
        currentHP += amount;

        // 최대 체력 넘지 않게 제한
        currentHP = Mathf.Clamp(currentHP, 0f, MaxHP);

        // UI 갱신
        OnHpChanged?.Invoke(currentHP, MaxHP);
    }

    public void SetInvincible(bool value)
    {
        // 무적 상태 설정 (회피 시작/끝에서 사용)
        IsInvincible = value;
    }

    public void RestoreFullHP()
    {
        // 풀피 회복 (리스폰, 체크포인트 등)
        if (IsDead) return;

        currentHP = MaxHP;
        OnHpChanged?.Invoke(currentHP, MaxHP);
    }

    private void Die()
    {
        // 중복 실행 방지
        if (IsDead) return;

        IsDead = true;
        currentHP = 0f;

        // 사망 이벤트 호출 (애니메이션, UI, 리스폰 등에서 사용)
        OnDie?.Invoke();

        Debug.Log("플레이어 사망");
    }
}