using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("����")]
    [SerializeField] private PlayerStats stats; // PlayerStats ���� (�ִ� ü��, ���� ��������)

    [Header("���� ü��")]
    [SerializeField] private float currentHP;   // ���� ü�� (���� ���ӿ��� �پ��� ��)

    public float CurrentHP => currentHP;        // �ܺο��� ���� ü�� �б��
    public float MaxHP => stats != null ? stats.MaxHP : 0f; // �ִ� ü�� (Stats���� ������)

    public bool IsDead { get; private set; }    // ��� ���� üũ
    public bool IsInvincible { get; private set; } // ���� ���� (ȸ�� �� ���)

    public event Action<float, float> OnHpChanged; // ü�� ���� �̺�Ʈ (UI ������Ʈ��)
    public event Action OnDie;                     // ��� �̺�Ʈ

    private void Awake()
    {
        // PlayerStats�� ���� �ȵ��� ��� �ڵ����� ������
        if (stats == null)
            stats = GetComponent<PlayerStats>();

        // ������ �� ü���� �ִ� ü������ ����
        currentHP = MaxHP;

        IsDead = false;
        IsInvincible = false;
    }

    public void TakeDamage(float damage)
    {
        // �̹� �׾����� ������ ����
        if (IsDead) return;

        // ���� ���¸� ������ ���� (������ ��)
        if (IsInvincible) return;

        // ���� �������� (������ 0)
        float defense = stats != null ? stats.Defense : 0f;

        // ���� ������ ��� (�ּ� 1 ����)
        float finalDamage = Mathf.Max(damage - defense, 1f);

        // ü�� ����
        currentHP -= finalDamage;

        // ü�� ���� ���� (0 ~ MaxHP)
        currentHP = Mathf.Clamp(currentHP, 0f, MaxHP);

        // ü�� UI ������Ʈ�� �̺�Ʈ ȣ��
        OnHpChanged?.Invoke(currentHP, MaxHP);

        // ü���� 0�̸� ��� ó��
        if (currentHP <= 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        // �׾����� ȸ�� �Ұ�
        if (IsDead) return;

        // 0 ���� ȸ�� ����
        if (amount <= 0f) return;

        // ü�� ����
        currentHP += amount;

        // �ִ� ü�� ���� �ʰ� ����
        currentHP = Mathf.Clamp(currentHP, 0f, MaxHP);

        // UI ����
        OnHpChanged?.Invoke(currentHP, MaxHP);
    }

    public void SetInvincible(bool value)
    {
        // ���� ���� ���� (ȸ�� ����/������ ���)
        IsInvincible = value;
    }

    public void RestoreFullHP()
    {
        // Ǯ�� ȸ�� (������, üũ����Ʈ ��)
        if (IsDead) return;

        currentHP = MaxHP;
        OnHpChanged?.Invoke(currentHP, MaxHP);
    }

    private void Die()
    {
        // �ߺ� ���� ����
        if (IsDead) return;

        IsDead = true;
        currentHP = 0f;

        // ��� �̺�Ʈ ȣ�� (�ִϸ��̼�, UI, ������ ��� ���)
        OnDie?.Invoke();

        Debug.Log("�÷��̾� ���");
    }
}