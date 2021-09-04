using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Weapon", menuName = "Create New Weapon")]
public class Weapon : ScriptableObject
{
    [Header("Weapon Information")]
    public string weaponName;
    public GameObject weaponPrefab;
    public GameObject weaponInventory;
    public GameObject weaponDrop;
    public float spawnChance = 1;
    public bool gun;
    public bool melee;

    [Header("Shared Variables")]
    public float timeBetweenShots = .25f;
    public float attackRange = 10;
    public int damageAmount = 10;

    [Header("Melee Variables")]
    public float timeBetweenHits = 1;
    public AudioClip impactSound;
    public GameObject impactEffect;

    [Header("Gun Variables")]
    public int ammoType; //0 is AR, 1 is SMG, 2 is Sniper
    public int fireRate = 5; //1 is semi, 3 is burst, 5 is auto
    public int clipSize;
    public GameObject bulletPrefab = null;
    public float bulletSpeed = 15;
    public float reloadTime = 3;

    [Header("Effects")]
    public GameObject muzzleFlash;
    public AudioClip useSound;
    public AudioClip reloadOne;
    public AudioClip reloadTwo;
    public AudioClip reloadThree;


    public int GetClipSize()
    {
        return clipSize;
    }

    public int GetAmmoType()
    {
        return ammoType;
    }

    public float GetAttackDistance()
    {
        return attackRange;
    }
    public GameObject GetWeaponDrop()
    {
        return weaponDrop;
    }
}
