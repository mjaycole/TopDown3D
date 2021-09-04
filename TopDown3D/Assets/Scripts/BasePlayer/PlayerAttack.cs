using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class PlayerAttack : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Transform weaponParent = null;
    [SerializeField] Transform meleeWeaponParent = null;
    [SerializeField] AmmoInventory ammoInventory = null;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] LayerMask wallLayer;
    [SerializeField] NavMeshAgent agent = null;
    [SerializeField] Animator anim = null;
    [SerializeField] AudioSource aud = null;

    [Header("Basic Attacking Variables")]
    [SerializeField] Weapon startingWeapon = null;
    [SerializeField] Weapon currentWeapon = null;
    [SerializeField] float rotationSpeed = 5;
    [SerializeField] EnemyHealth enemy = null;

    //Private variables
    bool hasGun;
    bool hasMelee;

    float cooldown;
    public float dist;
    bool reloading;
    bool attacking = false;
    bool facingTarget = false;
    bool movingToTarget = false;
    Vector3 targetPos;

    private void Start()
    {
        Equip(startingWeapon, startingWeapon.GetClipSize());
    }

    private void Update()
    {
        if (GetComponent<PlayerHealth>().GetPlayerHealth() <= 0) { return; }

        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, enemyLayer))
            {
                movingToTarget = true;
                enemy = hit.transform.gameObject.GetComponent<EnemyHealth>();
                agent.isStopped = false;
            }

            //Cancel attack or didn't click on enemy
            else
            {
                attacking = false;
                enemy = null;
            }
        }

        //Stop attacking if trying to move
        if (Input.GetMouseButtonDown(0))
        {
            StopAttack();
        }

        //Move to target if need to
        if (movingToTarget)
        {
            CheckIfCanAttack();
            MoveToTarget();
        }


        //Attacking variables
        if (attacking)
        {
            if (!facingTarget)
                RotateToTarget(targetPos);
            AttemptAttack();
            DetectEnemyDeath();
        }

        //Cooldown and reloading
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }

        //Reload if gun
        if (currentWeapon != null && currentWeapon.gun)
        {            
            if (!reloading && ammoInventory.GetCurrentClip() <= 0 && ammoInventory.GetAmmoInventory() > 0 || !reloading && Input.GetKeyDown(KeyCode.R) && ammoInventory.GetCurrentClip() < currentWeapon.clipSize && ammoInventory.GetAmmoInventory() > 0)
            {
                StartCoroutine(Reload());                
            }
        }
    }

    public void Equip(Weapon newWeapon, int newCurrentClip)
    {
        //Destroy current weapon mesh if there is one
        if (currentWeapon != null)
        {
            if (currentWeapon.gun)
            {
                Destroy(weaponParent.GetChild(0).gameObject);
            }
            else
            {
                Destroy(meleeWeaponParent.GetChild(0).gameObject);
            }
        }

        //Stop reloading so no reload glitch
        StopCoroutine("Reload");

        //Instantiate the gun to the gun parent
        if (newWeapon.gun)
        {
            GameObject weaponMesh = Instantiate(newWeapon.weaponPrefab, weaponParent.position, weaponParent.rotation, weaponParent);

            //Set the current weapon
            currentWeapon = newWeapon;
            ammoInventory.SetWeapon(currentWeapon, newCurrentClip);

            //Set the animaitons
            anim.SetBool("Melee", false);
        }

        //Otherwise, instantiate the melee to the melee parent
        else if (newWeapon.melee)
        {
            GameObject weaponMesh = Instantiate(newWeapon.weaponPrefab, meleeWeaponParent.position, meleeWeaponParent.rotation, meleeWeaponParent);

            //Set the current weapon
            currentWeapon = newWeapon;
            ammoInventory.SetWeapon(currentWeapon, newCurrentClip);

            //Set the animaitons
            anim.SetBool("Melee", true);
        }
    }

    #region Initiate Attack
    private void CheckIfCanAttack()
    {
        if (currentWeapon != null && enemy != null)
        {
            dist = currentWeapon.GetAttackDistance();
            targetPos = enemy.transform.position;

            if (Vector3.Distance(targetPos, transform.position) > dist || Physics.Linecast(transform.position, targetPos, wallLayer))
            {
                movingToTarget = true;
            }
            else
            {
                movingToTarget = false;
                attacking = true;
                facingTarget = false;
            }
        }
        else if (enemy == null)
        {
            StopAttack();
        }
    }
    private void MoveToTarget()
    {
        if (Vector3.Distance(targetPos, transform.position) > dist || Physics.Linecast(transform.position, targetPos, wallLayer))
        {
            agent.SetDestination(targetPos);
        }        
        else
        {
            agent.isStopped = true;
            movingToTarget = false;
            attacking = true;
            facingTarget = false;
        }
    }
    #endregion

    #region Attacking Variables
    private void RotateToTarget(Vector3 target)
    {
        agent.SetDestination(transform.position);

        Vector3 direction = (target - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        //transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);


        transform.rotation = lookRotation;


        facingTarget = true;
        
    }

    private void AttemptAttack()
    {
        if (currentWeapon != null && facingTarget) //Check to see if facing target and you have a weapon
        {
            if (enemy != null && enemy.GetComponent<EnemyHealth>().GetEnemyHealth() > 0) //Check to see if the enemy still exists and is alive
            {
                //Check to see if weapon is gun or melee
                if (currentWeapon.gun)
                {                    
                    if (ammoInventory.GetCurrentClip() > 0 && !reloading) //Check to see if you have ammo in the clip
                    {
                        if (cooldown <= 0) //Check to see if the cooldown has gone long enough
                        {
                            FireGun();
                        }
                    }
                }
                else if (currentWeapon.melee)
                {
                    if (cooldown <= 0)
                    {
                        SwingMelee();
                    }
                }

            }
            else
            {
                StopAttack();
            }
        }
    }

    private void FireGun()
    {
        //Reset Cooldown
        cooldown = currentWeapon.timeBetweenShots;

        //Instantiate the bullet
        Vector3 direction = (new Vector3(targetPos.x, targetPos.y + 1, targetPos.z) - weaponParent.GetChild(0).Find("Barrel").transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        GameObject newBullet = Instantiate(currentWeapon.bulletPrefab, weaponParent.GetChild(0).Find("Barrel").transform.position, lookRotation);
        newBullet.GetComponent<Bullet>().SetBulletDmaage(currentWeapon.damageAmount);
        newBullet.GetComponent<Bullet>().SetBulletSpeed(currentWeapon.bulletSpeed);
        Destroy(newBullet, 5);

        //Fire on the scriptable object
        ammoInventory.ShootWeapon();

        //Set animation
        anim.SetLayerWeight(1, 1);
        anim.Play("Shoot", 1);

        anim.SetLayerWeight(2, 0);

        //Effects
        aud.PlayOneShot(currentWeapon.useSound, .25f);

        GameObject flash = Instantiate(currentWeapon.muzzleFlash, weaponParent.GetChild(0).Find("Barrel").transform.position, Quaternion.LookRotation(-direction));
        Destroy(flash, 1);
    }

    private void SwingMelee()
    {
        //Reset cooldown
        cooldown = currentWeapon.timeBetweenHits;

        //Set animation
        anim.SetLayerWeight(1, 1);
        anim.Play("Swing", 1);

        anim.SetLayerWeight(2, 0);
    }

    private void PlayMeleeSound()
    {
        //Play sound effects through the animation calling this method
        aud.PlayOneShot(currentWeapon.useSound, .25f);
    }

    private void MeleeDamage()
    {
        if (Physics.Raycast(transform.position, targetPos - transform.position, 1.5f, enemyLayer))
        {
            if (enemy != null)
            {
                enemy.gameObject.GetComponent<EnemyHealth>().TakeDamage(currentWeapon.damageAmount);

                //Play the audio effects
                aud.PlayOneShot(currentWeapon.impactSound, .5f);

                //Instantiate the impact effects
                GameObject newEffect = Instantiate(currentWeapon.impactEffect, new Vector3(targetPos.x, targetPos.y + 1, targetPos.z), transform.rotation);
                Destroy(newEffect, .5f);
            }
        }
    }

    private void DetectEnemyDeath()
    {
        if (enemy != null)
        {
            if (enemy.GetEnemyHealth() <= 0)
            {
                StopAttack();
            }
        }
    }

    public void StopAttack()
    {
        attacking = false;
        enemy = null;
        movingToTarget = false;
        if (anim.GetLayerWeight(1) != 0)
            StartCoroutine(DisableAttackLayer());
    }
    #endregion

    IEnumerator DisableAttackLayer()
    {
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(1).length);
        anim.SetLayerWeight(1, 0);
    }
    IEnumerator Reload()
    {
        reloading = true;
        attacking = false;

        //Set the layer weights for animations
        anim.SetLayerWeight(1, 0);
        anim.SetLayerWeight(2, 1);

        anim.Play("Reload", 2);

        //Play audio
        aud.PlayOneShot(currentWeapon.reloadOne, .25f);

        yield return new WaitForSeconds(currentWeapon.reloadTime / 3);

        //Play audio
        aud.PlayOneShot(currentWeapon.reloadTwo, .25f);

        yield return new WaitForSeconds(currentWeapon.reloadTime / 3);

        //Play audio
        aud.PlayOneShot(currentWeapon.reloadThree, .25f);

        yield return new WaitForSeconds(currentWeapon.reloadTime / 3);

        //Set the layer weights for animations
        anim.SetLayerWeight(2, 0);
        anim.Play("Default", 2);

        reloading = false;
        ammoInventory.Reload();

        CheckIfCanAttack();        
    }

    //Testing inventory
    public Weapon GetStartingWeapon()
    {
        return startingWeapon;
    }
}
