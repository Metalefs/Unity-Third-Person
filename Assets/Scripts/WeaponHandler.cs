using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] private GameObject WeaponLogic;
    
    public void EnableWeapon()
    {
        WeaponLogic.SetActive(true);
    }

    public void DisableWeapon()
    {
        WeaponLogic.SetActive(false);
    }
}
