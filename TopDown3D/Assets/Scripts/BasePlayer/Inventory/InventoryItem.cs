using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] InventorySlot[] slots = null;
    [SerializeField] InventorySlot currentSlot = null;
    RectTransform rectTransform;

    [SerializeField] bool isWeapon;
    [SerializeField] Weapon weapon = null;
    [SerializeField] int currentClip;
    bool isEquipped;

    [SerializeField] AudioClip equipSound = null;
    [SerializeField] AudioClip inventoryMove = null;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        slots = GameObject.FindObjectsOfType<InventorySlot>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        currentSlot = GetComponentInParent<InventorySlot>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta;
        rectTransform.parent = GameObject.Find("Inventory").transform;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        InventorySlot newSlot = currentSlot;

        foreach (InventorySlot s in slots)
        {
            if (Vector2.Distance(s.GetComponent<RectTransform>().position, rectTransform.position) < 50f)
            {
                newSlot = s;
            }
        }

        if (isWeapon)
        {
            EquipWeapon equipSlot = GameObject.FindObjectOfType<EquipWeapon>();
            DropWeapon dropSlot = GameObject.FindObjectOfType<DropWeapon>();

            if (Vector2.Distance(equipSlot.GetComponent<RectTransform>().position, rectTransform.position) < 50f)
            {
                EquipWeaponInventory(equipSlot);
            }
            else if (Vector2.Distance(dropSlot.GetComponent<RectTransform>().position, rectTransform.position) < 50f)
            {
                DropWeapon(dropSlot);
            }
            else
            {
                currentSlot.SetFull(false);
                rectTransform.position = newSlot.GetComponent<RectTransform>().position;
                rectTransform.parent = newSlot.GetComponent<RectTransform>();
                newSlot.SetFull(true);

                //Play the sound effect
                GameObject.Find("Effects").GetComponent<AudioSource>().PlayOneShot(inventoryMove);
            }
        }  
        else
        {

        }
    }

    public void EquipWeaponInventory(EquipWeapon equipSlot)
    {
        if (equipSlot.transform.GetChild(0).Find("EquippedSlot").childCount != 0)
        {
            GameObject equippedWeapon = equipSlot.transform.GetChild(0).Find("EquippedSlot").GetChild(0).gameObject;
            equippedWeapon.GetComponent<InventoryItem>().enabled = true;
            equippedWeapon.GetComponent<RectTransform>().position = currentSlot.GetComponent<RectTransform>().position;
            equippedWeapon.transform.parent = currentSlot.transform;
            equippedWeapon.transform.parent.GetComponent<InventorySlot>().SetFull(true);
            equippedWeapon.transform.parent.GetComponent<InventorySlot>().SetFilledWeapon(equippedWeapon.GetComponent<InventoryItem>().weapon);

            if (equippedWeapon.GetComponent<InventoryItem>().weapon.gun)
            {
                equippedWeapon.GetComponent<InventoryItem>().GetCurrentClip(GameObject.FindObjectOfType<AmmoInventory>().GetCurrentClip());
            }
        }
        else
        {
            currentSlot.SetFull(false);
        }

        equipSlot.EquipNewWeapon(weapon, currentClip);
        rectTransform.position = equipSlot.transform.GetChild(0).Find("EquippedSlot").GetComponent<RectTransform>().position;
        rectTransform.parent = equipSlot.transform.GetChild(0).Find("EquippedSlot").GetComponent<RectTransform>();
        this.enabled = false;        

        //Play the sound effect
        GameObject.Find("Effects").GetComponent<AudioSource>().PlayOneShot(equipSound);
    }

    public void DropWeapon(DropWeapon dropSlot)
    {
        dropSlot.Drop(weapon, currentClip);
        currentSlot.SetFilledWeapon(null);
        currentSlot.SetFull(false);


        Destroy(gameObject);
    }

    public void GetCurrentClip(int newClip)
    {
        currentClip = newClip;
    }
}
