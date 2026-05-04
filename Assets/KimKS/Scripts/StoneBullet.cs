using UnityEngine;
using System.Collections;

namespace KimKS
{
    public class StoneBullet : Spell
    {
        private bool isHit = false; // 중복 충돌 방지 및 이동 제어용

        protected override IEnumerator SpellSequence()
        {
            isHit = false;
            yield return StartCoroutine(Casting());
            yield return StartCoroutine(ThrowSpell());
        }

        private IEnumerator Casting()
        {
            m_ChildPF[0].SetActive(true);
            m_ChildPF[0].GetComponent<ParticleSystem>().Play();
            yield return new WaitForSeconds(0.4f);
        }
        private IEnumerator ThrowSpell()
        {
            m_ChildPF[0].SetActive(false);
            m_ChildPF[1].SetActive(true);
            m_ChildPF[1].GetComponent<ParticleSystem>().Play();

            float timer = 0f;

            while (!isHit)
            {
                float moveDistance = m_projectileSpeed * Time.deltaTime;
                Vector3 nextStep = m_direction * moveDistance;

                RaycastHit hit;

                // 현재 위치에서 다음 프레임에 갈 거리만큼 미리 선을 그어 확인
                if (Physics.Raycast(transform.position, m_direction, out hit, moveDistance))
                {
                    transform.position = hit.point; // 충돌 지점으로 이동
                    OnCollisionHit(hit);
                    yield break;
                }

                transform.position += nextStep;
                timer += Time.deltaTime;
                yield return null;
            }
        }

        private void OnCollisionHit(RaycastHit hit)
        {
            if (isHit)
                return;
            isHit = true;

            //Debug.Log("Raycast Hit: " + hit.collider.name);

            StartCoroutine(SpellHit(hit.point));
        }
        private IEnumerator SpellHit(Vector3 hitPoint)
        {
            m_ChildPF[1].SetActive(false);
            m_ChildPF[2].transform.position = hitPoint;
            m_ChildPF[2].SetActive(true);
            m_ChildPF[2].GetComponent<ParticleSystem>().Play();

            yield return new WaitForSeconds(0.4f);
            SpellFin();
        }

        private void SpellFin()
        {
            Destroy(gameObject);
        }
    }
}

