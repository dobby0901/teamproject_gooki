using UnityEngine;
using UnityEngine.InputSystem;
using KimKS;

public class PlayerSkillInput : MonoBehaviour
{
    [SerializeField] private SpellCaster spellCaster;

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            spellCaster.CastRandomSpell(); 
        }
    }
}