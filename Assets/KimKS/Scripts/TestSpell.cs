using System.Collections;
using UnityEngine;

namespace Kim
{
    public class TestSpell : MonoBehaviour
    {
        [SerializeField]
        private GameObject spellParent;

        private GameObject[] spellPF;

        private void Awake()
        {
            spellPF = new GameObject[3];

            for (int i = 0; i < spellPF.Length; i++)
            {
                spellPF[i] = spellParent.transform.GetChild(i).gameObject;
                spellPF[i].SetActive(false);
            }
        }
        private void Start()
        {
            StartCoroutine(SpellSequence());
        }

        private  IEnumerator SpellSequence()
        {
            yield return SpellCast();
            yield return SpellTravel(new Vector3(0f, 0f, 4f));
            yield return SpellHit();
        }

        private IEnumerator SpellCast()
        {
            Quaternion starRot = transform.rotation;
            Quaternion endRot = transform.rotation *  Quaternion.Euler(0f, 0f, 90f);
            
            float duration = 2f;
            float timer = 0f;
            spellPF[0].SetActive(true);

            while (timer < duration) 
            {
                spellPF[0].transform.rotation = Quaternion.Slerp(starRot, endRot, timer/duration);
                timer += Time.deltaTime;
                yield return null;
            }
            spellPF[0].transform.rotation = endRot;
            spellPF[0].SetActive(false);
        }
        private IEnumerator SpellTravel(Vector3 _dir)
        {
            spellPF[1].SetActive(true);
            float duration = 2f;
            float timer = 0f;
            float t = 0.01f;

            while (timer < duration)
            {
                
                spellParent.transform.position = Vector3.Lerp(spellParent.transform.position, _dir, t);
                timer += Time.deltaTime;
                yield return null;
            }
            spellParent.transform.position = _dir;
            spellPF[1].SetActive(false);
        }
        private IEnumerator SpellHit()
        {
            Quaternion starRot = transform.rotation;
            Quaternion endRot = transform.rotation * Quaternion.Euler(0f, 0f, 90f);

            float duration = 2f;
            float timer = 0f;
            spellPF[2].SetActive(true);

            while (timer < duration)
            {
                spellPF[2].transform.rotation = Quaternion.Slerp(starRot, endRot, timer / duration);
                timer += Time.deltaTime;
                yield return null;
            }
            spellPF[2].transform.rotation = endRot;
            spellPF[2].SetActive(false);
        }
    }

}
