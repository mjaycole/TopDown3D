using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipWeapon : MonoBehaviour
{


    public void EquipNewWeapon(Weapon newWeapon, int currentClip)
    {
        gameObject.transform.root.GetComponent<PlayerAttack>().Equip(newWeapon, currentClip);
    }
}
