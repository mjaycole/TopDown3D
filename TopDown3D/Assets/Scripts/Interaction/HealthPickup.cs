using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] float minHealth;
    [SerializeField] float maxHealth;
    [SerializeField] float currentHealth;
    [SerializeField] TMP_Text dropText = null;
    [SerializeField] AudioClip pickupClip = null;
    public float dropChance;

    private void Start()
    {
        currentHealth = Random.Range(minHealth, maxHealth);
        dropText.text = currentHealth.ToString("F0") + " Health";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.GetComponent<PlayerHealth>().GetPlayerHealth() >= other.GetComponent<PlayerHealth>().GetMaxHealth()) { return; }


            other.GetComponent<PlayerHealth>().AddHealth(currentHealth);

            //Play the sound effect
            GameObject.Find("Effects").GetComponent<AudioSource>().PlayOneShot(pickupClip, .15f);

            Destroy(gameObject);
        }
    }
}
