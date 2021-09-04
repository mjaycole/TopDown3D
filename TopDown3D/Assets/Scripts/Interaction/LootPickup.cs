using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LootPickup : MonoBehaviour
{
    [SerializeField] int minLoot;
    [SerializeField] int maxLoot;
    [SerializeField] int currentLoot;
    [SerializeField] TMP_Text dropText = null;
    [SerializeField] AudioClip pickupClip = null;

    private void Start()
    {
        currentLoot = Random.Range(minLoot, maxLoot);
        dropText.text = currentLoot.ToString() + " Credits";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<CurrencyInventory>().AddCurrency(currentLoot);

            //Play the sound effect
            GameObject.Find("Effects").GetComponent<AudioSource>().PlayOneShot(pickupClip);

            Destroy(gameObject);
        }
    }
}
