using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] private GameObject[] WeaponLogic;
    
    public void EnableWeapon()
    {
        foreach (GameObject weapon in WeaponLogic)
        {
            weapon.SetActive(true);
        }
    }

    public void DisableWeapon()
    {
        foreach (GameObject weapon in WeaponLogic)
        {
            weapon.SetActive(false);
        }
    }
}
