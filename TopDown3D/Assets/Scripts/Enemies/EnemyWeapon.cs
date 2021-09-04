using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Gun", menuName = "New Enemy Gun")]
public class EnemyWeapon : ScriptableObject
{
    [Header("Weapon Information")]
    public GameObject weaponPrefab = null;
    public GameObject bulletPrefab = null;

    [Header("Variables")]
    public int clipSize = 10;
    public int currentClip;
    public float timeBetweenShots = .25f;
    public float reloadTime = 3;
    public float bulletSpeed;
    public int bulletDamage;

    [Header("Effects")]
    public AudioClip useSound;
    public GameObject muzzleFlash;

    public void InitializeAmmo()
    {
        currentClip = clipSize;
    }

    public void Fire()
    {
        currentClip--;
    }

    public void Reload()
    {
        currentClip = clipSize;
    }

    public int GetCurrentClip()
    {
        return currentClip;
    }
    public float GetTimeBetweenShots()
    {
        return timeBetweenShots;
    }
    public float GetReloadTime()
    {
        return reloadTime;
    }
    public AudioClip GetUseSound()
    {
        return useSound;
    }
}
