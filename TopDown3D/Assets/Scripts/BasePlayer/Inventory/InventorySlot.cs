using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] bool isFull;
    [SerializeField] Weapon filledWeapon;
    

    public void SetFull(bool t)
    {
        isFull = t;
    }
    public void SetFilledWeapon(Weapon newWeapon)
    {
        filledWeapon = newWeapon;
    }


    public bool GetIsFull()
    {
        return isFull;
    }
    public Weapon GetWeapon()
    {
        return filledWeapon;
    }
}
