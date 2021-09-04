using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoDrop : MonoBehaviour
{
    [SerializeField] float pickupRange = 3;
    public float spawnChance;
    [SerializeField] int ammoType;
    [SerializeField] int minAmount;
    [SerializeField] int maxAmount;
    [SerializeField] int amount;
    [SerializeField] string displayText;
    [SerializeField] Transform modelParent = null;
    [SerializeField] TMP_Text ammoTypeText = null;

    [SerializeField] AudioClip pickUpGood = null;
    [SerializeField] AudioClip pickUpError = null;


    private void Awake()
    {
        amount = Random.Range(minAmount, maxAmount);
        ammoTypeText.text = amount.ToString() + " Rounds " + displayText;        
    }
    private void OnMouseDown()
    {
        AmmoInventory invenMan = GameObject.FindObjectOfType<AmmoInventory>();

        if (Vector3.Distance(GameObject.FindObjectOfType<PlayerMovement>().transform.position, transform.position) < 3)
        {
            if (invenMan.CanStoreMoreAmmo(ammoType))
            {
                invenMan.PickupAmmo(ammoType, amount);
                GameObject.Find("Effects").GetComponent<AudioSource>().PlayOneShot(pickUpGood);
                Destroy(gameObject);

            }
            else
            {
                GameObject.Find("Effects").GetComponent<AudioSource>().PlayOneShot(pickUpError, .5f);
            }
        }
    }
}
