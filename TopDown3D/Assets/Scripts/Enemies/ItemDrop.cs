using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [Header("Items")]
    [SerializeField] Weapon[] allWeapons = null;
    [SerializeField] AmmoDrop[] allAmmo = null;
    [SerializeField] HealthPickup health = null;    

    [Header("Currency")]
    [SerializeField] int currencyDropAmounts;
    [SerializeField] LootPickup currencyDrop = null;


    public void EnableDrop()
    {
        foreach (Weapon w in allWeapons)
        {
            float chance = Random.value;

            if (chance < w.spawnChance)
            {
                Instantiate(w.GetWeaponDrop(), new Vector3(transform.position.x + Random.Range(-1.5f, 1.5f), transform.position.y + 1, transform.position.z + Random.Range(-1.5f, 1.5f)), Quaternion.identity);
            }
        }

        foreach (AmmoDrop a in allAmmo)
        {
            float chance = Random.value;

            if (chance < a.spawnChance)
            {
                Instantiate(a, new Vector3(transform.position.x + Random.Range(-1.5f, 1.5f), transform.position.y + 1, transform.position.z + Random.Range(-1.5f, 1.5f)), Quaternion.identity);
            }
        }

        int d = Random.Range(0, currencyDropAmounts);

        for (int i = 0; i < d; i++)
        {
            Instantiate(currencyDrop, new Vector3(transform.position.x + Random.Range(-1.5f, 1.5f), transform.position.y + 1, transform.position.z + Random.Range(-1.5f, 1.5f)), Quaternion.identity);
        }

        float h = Random.value;

        if (h < health.dropChance)
        {
            Instantiate(health, new Vector3(transform.position.x + Random.Range(-1.5f, 1.5f), transform.position.y + 1, transform.position.z + Random.Range(-1.5f, 1.5f)), Quaternion.identity);
        }
    }
}
