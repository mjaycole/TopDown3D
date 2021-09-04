using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootChest : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Transform spawnLocation = null;
    [SerializeField] Animator anim = null;
    [SerializeField] AudioSource aud = null;
    [SerializeField] AudioClip clip1 = null;
    [SerializeField] AudioClip clip2 = null;
    [SerializeField] AudioClip clip3 = null;

    [Header("Items")]
    [SerializeField] Weapon[] allWeapons = null;
    [SerializeField] AmmoDrop[] allAmmo = null;
    [SerializeField] int currencyDropAmounts;
    [SerializeField] GameObject currencyDrop = null;
    [SerializeField] HealthPickup health = null;

    bool triggered = false;

    private void OnMouseUp()
    {
        if (triggered) { return; }

        if (Vector3.Distance(GameObject.FindObjectOfType<PlayerMovement>().transform.position, transform.position) < 3)
        {
            EnableDrop();
            anim.SetTrigger("Open");
            triggered = true;
        }
    }

    public void EnableDrop()
    {
        foreach (Weapon w in allWeapons)
        {
            float chance = Random.value;

            if (chance < w.spawnChance)
            {
                Instantiate(w.GetWeaponDrop(), new Vector3(spawnLocation.position.x + Random.Range(-1.5f, 1.5f), spawnLocation.position.y + 1, spawnLocation.position.z + Random.Range(-1.5f, 1.5f)), Quaternion.identity);
            }
        }

        foreach (AmmoDrop a in allAmmo)
        {
            float chance = Random.value;

            if (chance < a.spawnChance)
            {
                Instantiate(a, new Vector3(spawnLocation.position.x + Random.Range(-1.5f, 1.5f), spawnLocation.position.y + 1, spawnLocation.position.z + Random.Range(-1.5f, 1.5f)), Quaternion.identity);
            }
        }

        int d = Random.Range(0, currencyDropAmounts);

        for (int i = 0; i < d; i++)
        {
            Instantiate(currencyDrop, new Vector3(spawnLocation.position.x + Random.Range(-1.5f, 1.5f), spawnLocation.position.y + 1, spawnLocation.position.z + Random.Range(-1.5f, 1.5f)), Quaternion.identity);
        }

        float h = Random.value;

        if (h < health.dropChance)
        {
            Instantiate(health, new Vector3(transform.position.x + Random.Range(-1.5f, 1.5f), transform.position.y + 1, transform.position.z + Random.Range(-1.5f, 1.5f)), Quaternion.identity);
        }
    }

    public void PlayClipOne()
    {
        aud.PlayOneShot(clip1, 1f);
    }
    public void PlayClipTwo()
    {
        aud.PlayOneShot(clip2, 1f);
    }
    public void PlayClipThree()
    {
        aud.PlayOneShot(clip3);
    }
}
