using UnityEngine;

public class PlayerStats : MonoBehaviour
{ 
    // 기본 스탯 (플레이어 능력치)
    [Header("스탯")]
    [SerializeField] private float maxHP;     // 최대 체력 (현재 체력의 기준값)
    [SerializeField] private float maxMP;      // 최대 마나 (스킬 사용에 사용)
    [SerializeField] private float attack; // 공격력 (데미지 계산에 사용)
    [SerializeField] private float defense;     // 방어력 (피해 감소에 사용)


    // 외부에서 읽기 전용
    public float MaxHP => maxHP;           // 최대 체력 반환
    public float MaxMP => maxMP;           // 최대 마나 반환
    public float Attack => attack;    // 공격력 반환
    public float Defense => defense;       // 방어력 반환
     
    //최대 체력 설정
    public void SetMaxHP(float value) 
    {
        // 최대 체력은 최소 1 이상으로 제한 (0 이하 방지)
        maxHP = Mathf.Max(1f, value);
    }

    //최대 마나 설정
    public void SetMaxMP(float value)
    {
        // 마나는 0 이상으로 제한
        maxMP = Mathf.Max(0f, value);
    }

    //공격력 설정
    public void SetAttack(float value)
    {
        // 공격력은 음수가 되면 안됨
        attack = Mathf.Max(0f, value);
    }

    //방어력 설정
    public void SetDefense(float value)
    {
        // 방어력도 음수 방지
        defense = Mathf.Max(0f, value);
    }
 
    // 값 추가 (버프 / 장비 / 레벨업)
   
    //최대 체력 추가
    public void AddMaxHP(float value)
    {
        // 현재 maxHP에 value를 더함 (장비 착용, 레벨업 등)
        maxHP = Mathf.Max(1f, maxHP + value);
    }

    //최대 마나 추가
    public void AddMaxMP(float value)
    {
        // 현재 maxMP에 value를 더함
        maxMP = Mathf.Max(0f, maxMP + value);
    }

    //공격력 추가
    public void AddAttack(float value)
    {
        // 공격력 증가 (버프, 무기 등)
        attack = Mathf.Max(0f, attack + value);
    }

    //방어력 추가
    public void AddDefense(float value)
    {
        // 방어력 증가 (방어구 등)
        defense = Mathf.Max(0f, defense + value);
    }
}