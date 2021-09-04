using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class WeaponDrop : MonoBehaviour
{
    [SerializeField] float pickupRange = 3;
    [SerializeField] Weapon weapon = null;
    [SerializeField] Transform modelParent = null;
    [SerializeField] TMP_Text weaponName = null;
    [SerializeField] int currentClip;

    [SerializeField] AudioClip pickUpGood = null;
    [SerializeField] AudioClip pickUpError = null;


    private void Awake()
    {
        Instantiate(weapon.weaponPrefab, modelParent.position, modelParent.rotation, modelParent);
        weaponName.text = weapon.weaponName;
        currentClip = weapon.GetClipSize();
    }

    public void GetCurrentClip(int newClip)
    {
        currentClip = newClip;
    }

    private void OnMouseDown()
    {
        InventoryManager invenMan = GameObject.FindObjectOfType<InventoryManager>();

        if (invenMan.OpenWeaponSlot())
        {
            if (Vector3.Distance(GameObject.FindObjectOfType<PlayerMovement>().transform.position, transform.position) < 3)
            {
                invenMan.CollectWeapon(weapon, currentClip);
                GameObject.Find("Effects").GetComponent<AudioSource>().PlayOneShot(pickUpGood);
                Destroy(gameObject);
            }
        }
        else
        {
            GameObject.Find("Effects").GetComponent<AudioSource>().PlayOneShot(pickUpError, .5f);
        }
    }
}
