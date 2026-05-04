using System.Collections.Generic;
using UnityEngine;

namespace KimKS
{
    public class SpellCaster : MonoBehaviour
    {
        [SerializeField] private List<Spell> spellList;

        [Header("Cast Point")]
        [SerializeField] private Transform castPoint;

        [Header("Test Setting")]
        [SerializeField] private float spellSpeed = 30f;
        [SerializeField] private int spellDamage = 30;

        public void CastRandomSpell()
        {
            if (spellList == null || spellList.Count == 0)
                return;

           
            int randomIndex = Random.Range(0, spellList.Count);

            Vector3 curPos = castPoint != null ? castPoint.position : transform.position;

            Vector3 dir = transform.forward;
            dir.y = 0f;
            dir.Normalize();

            Spell spellObj = Instantiate(spellList[randomIndex], curPos, Quaternion.LookRotation(dir));

            spellObj.InitSpell(curPos, dir, spellSpeed, spellDamage);
            spellObj.CastSpell();
        }
    }
}