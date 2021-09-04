using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoInventory : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] TMP_Text ammoText = null;
    [SerializeField] TMP_Text[] ammoInventoryText = null;

    [Header("Ammo Variables")]
    [SerializeField] Weapon currentWeapon = null;
    [SerializeField] int ammoType;
    [SerializeField] int currentClip;

    [SerializeField] int[] ammoInventory; //0 for AR, 1 for SMG, 2 for Sniper
    [SerializeField] int[] maxAmmo;

    private void Start()
    {
        for (int i = 0; i < ammoInventory.Length; i++)
        {
            ammoInventoryText[i].text = ammoInventory[i].ToString();
        }
    }

    public void SetWeapon(Weapon newWeapon, int newWeaponClip)
    {
        currentWeapon = newWeapon;
        currentClip = newWeaponClip;
        ammoType = newWeapon.GetAmmoType();

        if (currentWeapon.gun)
        {
            ammoText.text = currentClip.ToString() + " / " + ammoInventory[ammoType].ToString();
        }
        else
        {
            ammoText.text = "--";
        }
    }

    public void ShootWeapon()
    {
        currentClip--;

        ammoText.text = currentClip.ToString() + " / " + ammoInventory[ammoType].ToString();
    }

    public void Reload()
    {
        ammoInventory[ammoType] += currentClip;
        currentClip = Mathf.Min(GetAmmoInventory(), currentWeapon.GetClipSize());
        ammoInventory[ammoType] -= currentClip;

        ammoText.text = currentClip.ToString() + " / " + ammoInventory[ammoType].ToString();
        ammoInventoryText[ammoType].text = ammoInventory[ammoType].ToString();
    }

    public void PickupAmmo(int type, int amount)
    {
        ammoInventory[type] += amount;
        if (ammoInventory[type] > maxAmmo[type])
        {
            ammoInventory[type] = maxAmmo[type];
        }
        ammoInventoryText[type].text = ammoInventory[type].ToString();
        ammoText.text = currentClip.ToString() + " / " + ammoInventory[ammoType].ToString();
    }


    public int GetCurrentClip()
    {
        return currentClip;
    }

    public int GetAmmoInventory()
    {
        return ammoInventory[ammoType];
    }

    public bool CanStoreMoreAmmo(int type)
    {
        if (ammoInventory[type] >= maxAmmo[type])
        {
            return false;
        }
        else
        {
            return true;
        }
    }

}
