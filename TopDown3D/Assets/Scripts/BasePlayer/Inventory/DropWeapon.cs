using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropWeapon : MonoBehaviour
{
    [SerializeField] AudioClip dropSound = null;

    public void Drop(Weapon droppedWeapon, int clipAmount)
    {
        Transform player = GameObject.FindObjectOfType<PlayerHealth>().transform;
        Vector3 dropSpot = new Vector3(player.position.x + Random.Range(-1.5f, 1.5f), player.position.y + 1, player.position.z + Random.Range(-1.5f, 1.5f));
        GameObject dropped = Instantiate(droppedWeapon.weaponDrop, dropSpot, player.rotation);

        if (droppedWeapon.gun)
        {
            dropped.GetComponent<WeaponDrop>().GetCurrentClip(clipAmount);
        }

        //Play the sound effect
        GameObject.Find("Effects").GetComponent<AudioSource>().PlayOneShot(dropSound);
    }
}
