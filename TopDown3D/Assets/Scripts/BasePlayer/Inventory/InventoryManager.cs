using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] GameObject inventoryParent = null;
    [SerializeField] GameObject[] allInventoryPanels = null;
    [SerializeField] Image[] allTabs = null;
    [SerializeField] GameObject characterPanel = null;
    [SerializeField] GameObject weaponsPanel = null;
    [SerializeField] Color deactivatedColor;
    bool inventoryOpen = false;

    [Header("Weapon Inventory")]
    [SerializeField] InventorySlot[] weaponSlots = null;
    [SerializeField] List<Weapon> playerWeapons = new List<Weapon>();
    [SerializeField] Transform currentWeapon = null;

    [SerializeField] Weapon testExtraWeapon = null;

    [Header("Effects")]
    [SerializeField] AudioSource aud = null;
    [SerializeField] AudioClip openInventoryClip = null;
    [SerializeField] AudioClip closeInventoryClip = null;

    private void Start()
    {        
        CollectWeapon(transform.root.GetComponent<PlayerAttack>().GetStartingWeapon(), transform.root.GetComponent<PlayerAttack>().GetStartingWeapon().GetClipSize());
        inventoryParent.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            OpenInventory();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            CollectWeapon(testExtraWeapon, testExtraWeapon.GetClipSize());
        }
    }

    private void OpenInventory()
    {
        if (inventoryOpen)
        {
            aud.PlayOneShot(closeInventoryClip, .5f);
            inventoryParent.SetActive(false);
            inventoryOpen = false;
        }
        else
        {
            aud.PlayOneShot(openInventoryClip, .5f);
            inventoryParent.SetActive(true);
            inventoryOpen = true;
        }
    }

    private void OpenNewPanel(GameObject newPanel)
    {
        foreach (GameObject o in allInventoryPanels)
        {
            o.SetActive(false);
        }

        foreach (Image i in allTabs)
        {
            i.color = deactivatedColor;
        }

        newPanel.SetActive(true);
        newPanel.transform.parent.GetComponent<Image>().color = new Color(255, 255, 255, 255);
    }

    public void OpenCharacterManager()
    {
        OpenNewPanel(characterPanel);
    }

    public void OpenWeaponsManager()
    {
        OpenNewPanel(weaponsPanel);
    }

    #region Weapon Inventory
    public void CollectWeapon(Weapon newWeapon, int currentClip)
    {
        if (OpenWeaponSlot())
        {

            if (currentWeapon.childCount <= 0)
            {
                GameObject newCurrentWeapon = Instantiate(newWeapon.weaponInventory, currentWeapon.position, currentWeapon.rotation, currentWeapon);
                newCurrentWeapon.GetComponent<InventoryItem>().enabled = false;
                playerWeapons.Add(transform.root.GetComponent<PlayerAttack>().GetStartingWeapon());
            }
            else
            {
                for (int i = 0; i < weaponSlots.Length; i++)
                {
                    if (!weaponSlots[i].GetIsFull())
                    {
                        GameObject newCard = Instantiate(newWeapon.weaponInventory, weaponSlots[i].transform.position, weaponSlots[i].transform.rotation, weaponSlots[i].transform);
                        newCard.GetComponent<InventoryItem>().GetCurrentClip(currentClip);
                        weaponSlots[i].SetFull(true);
                        weaponSlots[i].SetFilledWeapon(newWeapon);
                        playerWeapons.Add(transform.root.GetComponent<PlayerAttack>().GetStartingWeapon());
                        return;
                    }
                }
            }
        }
    }

    public bool OpenWeaponSlot()
    {
        bool available = false;

        for (int i = 0; i < weaponSlots.Length; i++)
        {
            if (!weaponSlots[i].GetIsFull())
            {
                available = true;
            }
        }

        return available;
    }
    #endregion

    public bool GetInventoryOpen()
    {
        return inventoryOpen;
    }
}
