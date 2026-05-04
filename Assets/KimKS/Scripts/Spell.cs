using System.Collections;
using UnityEngine;

namespace KimKS
{
    public class Spell : MonoBehaviour
    {
        /* 
         * 데미지
         * 프리팹(GameObject)
         * 차일드 오브젝트
         * 범위
         * 추적 여부

         *  * 시작 위치
         *  * 방향
         *  * 속도
         *  * 지속시간
         */
        [SerializeField]
        protected int m_SpellDamage = 0;

        [SerializeField]
        protected GameObject[] m_ChildPF;

        [SerializeField]
        protected float m_Range = 0f;

        protected bool IsTrackable = false;
        protected Vector3 m_startPosition = Vector3.zero;
        protected Vector3 m_direction = Vector3.zero;
        protected float m_projectileSpeed = 0f;
        protected float m_spellDuration = 0f;

        /// <summary>
        /// 투사체 쏘는 마법/스킬의 초기화.
        /// </summary>
        /// <param name="_start">마법 시전하는 위치 = 캐릭터 위치</param>
        /// <param name="_dir">마법 발사할 방향</param>
        /// <param name="_speed">투사체 속도</param>
        /// <param name="_dps">마법 데미지</param>
        public void InitSpell(Vector3 _start, Vector3 _dir, float _speed, int _dps)
        {
            m_startPosition = _start;
            m_direction = _dir.normalized;
            m_projectileSpeed = _speed;
            m_SpellDamage = _dps;
        }

        /// <summary>
        /// 캐릭터 기준으로 주변 일정 반경 내 영향 주는 마법/스킬. 
        /// </summary>
        /// <param name="_start">마법 시전하는 위치 = 캐릭터 위치</param>
        /// <param name="_dps">마법 데미지</param>
        /// <param name="_range">마법 영향 미치는 반경</param>
        /// <param name="_duration">마법 지속 시간(초 단위)</param>
        public void InitSpell(Vector3 _start, int _dps, float _range, float _duration)
        {
            m_startPosition = _start;
            m_SpellDamage = _dps;
            m_Range = _range;
            m_spellDuration = _duration;
        }

        /// <summary>
        /// 마법/스킬의 모든 멤버 변수 초기화 하는 메소드
        /// </summary>
        /// <param name="_start">마법 시전하는 위치 = 캐릭터 위치</param>
        /// <param name="_dir">마법 발사할 방향 = 캐릭터가 바라보는 방향</param>
        /// <param name="_speed"></param>
        /// <param name="_dps">마법 데미지</param>
        /// <param name="_range">마법 영향 미치는 반경</param>
        /// <param name="_duration">마법 지속 시간(초 단위)</param>
        public void InitSpell(Vector3 _start, Vector3 _dir, float _speed, int _dps, float _range, float _duration, bool _trackable)
        {
            m_startPosition = _start;
            m_direction = _dir.normalized;
            m_projectileSpeed = _speed;
            m_SpellDamage = _dps;
            m_Range = _range;
            m_spellDuration = _duration;
            IsTrackable = _trackable;
        }

        /// <summary>
        /// 마법/스킬 쓰는 메소드. 사용하기 전에 InitSpell() 꼭 쓰도록
        /// </summary>
        public virtual void CastSpell()
        {
            StartCoroutine(SpellSequence());
        }

        protected virtual IEnumerator SpellSequence()
        {
            yield return null;
        }

    }
}

