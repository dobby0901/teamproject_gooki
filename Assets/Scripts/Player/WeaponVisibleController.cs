using UnityEngine;

public class WeaponVisibleController : MonoBehaviour
{
    [SerializeField] private GameObject weaponObject;

    public void HideWeapon() //무기 숨기기
    {
        weaponObject.SetActive(false);
    }

    public void ShowWeapon() //무기 보여주기
    {
        weaponObject.SetActive(true);
    }
}