using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Animator anim = null;
    [SerializeField] NavMeshAgent agent = null;
    [SerializeField] Transform weaponParent = null;
    [SerializeField] EnemyWeapon enemyWeapon = null;
    [SerializeField] AudioSource aud = null;

    [Header("Sight and Attack Variables")]
    public bool hasGun;
    public bool hasMelee;
    [SerializeField] float viewDistance = 15;
    [SerializeField] float attackRange = 7;
    [SerializeField] Transform player = null;
    [SerializeField] LayerMask worldLayer;
    [SerializeField] float accuracyModifier;
    bool underAttack = false;
    bool seePlayer = false;
    bool inAttackRange = false;

    int clipSize;
    int currentClip;

    bool reloading = false;
    float cooldown;

    private enum State
    {
        Idle,
        Chasing,
        Attacking
    }

    private State state;

    //Audio
    [SerializeField] float footstepsCooldown;
    [SerializeField] AudioClip[] footsteps;
    float footTimer;

    private void Start()
    {
        player = GameObject.FindObjectOfType<PlayerMovement>().transform.GetChild(0);
        state = State.Idle;

        clipSize = enemyWeapon.clipSize;
        currentClip = clipSize;

        if (hasGun)
        {
            enemyWeapon.InitializeAmmo();
        }

        cooldown = 0;
    }

    private void Update()
    {
        if (GetComponent<EnemyHealth>().GetEnemyHealth() <= 0)
        {
            anim.SetLayerWeight(1, 0);
            anim.SetLayerWeight(2, 0);

            return; 
        }

        //Set shooting cooldown
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }

        StateController();
        MovementAnimations();
        inAttackRange = InAttackRange();
        seePlayer = CanSeePlayer();

        if (seePlayer && !inAttackRange)
        {
            state = State.Chasing;
        }
        else if (seePlayer && inAttackRange)
        {
            state = State.Attacking;
            RotateToTarget();
        }
        else
        {
            state = State.Idle;
        }

        if (currentClip <= 0 && !reloading)
        {
            StartCoroutine(Reload());
        }


        //Audio
        footTimer -= Time.deltaTime;
        if (agent.velocity != Vector3.zero && footTimer <= 0)
        {
            PlayFootstep();
        }
    }

    #region State and Animation Controllers
    private void StateController()
    {
        if (state == State.Idle)
        {
            SetDestination(transform.position);
        }
        else if (state == State.Chasing)
        {
            cooldown = .1f;

            SetDestination(player.position);
        }
        else if (state == State.Attacking)
        {
            agent.SetDestination(transform.position);
            //Play the animations
            anim.SetLayerWeight(1, 1);
            anim.Play("Shoot", 1);

            if (hasGun)
            {
                if (CanShoot())
                {
                    Shoot();
                }
            }
            else if (hasMelee)
            {

            }
        }
    }

    private void MovementAnimations()
    {
        if (state != State.Attacking)
        {
            if (Vector3.Distance(transform.position, agent.destination) > .5f)
            {
                anim.SetBool("Run", true);

                //Set weights
                anim.SetLayerWeight(1, 0);
                anim.Play("Default", 1);
            }
            else
            {
                anim.SetBool("Run", false);

                anim.SetLayerWeight(1, 0);
                anim.Play("Default", 1);
            }
        }
        else if (state == State.Attacking && reloading)
        {
            if (Vector3.Distance(transform.position, agent.destination) > .5f)
            {
                anim.SetBool("Run", true);
            }
            else
            {
                anim.SetBool("Run", false);
            }
        }
    }
    #endregion

    #region DetectPlayerVariables
    private bool CanSeePlayer()
    {
        if (Vector3.Distance(transform.position, player.position) < viewDistance)
        {
            Debug.DrawLine(player.position, transform.position, Color.green);
            
            if (!Physics.Linecast(transform.position, player.position, worldLayer))
            {
                return true;
            }

            else
            {
                return false;
            }
        }
        else if (underAttack)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool InAttackRange()
    {
        if (Vector3.Distance(transform.position, player.position) < attackRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void DetectIfPlayerAttacks()
    {
        underAttack = true;
    }
    #endregion

    #region Attacking Variables
    private void RotateToTarget()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        transform.rotation = lookRotation;
    }
    private bool CanShoot()
    {
        if (currentClip > 0 && !reloading && cooldown <= 0 && player.GetComponentInParent<PlayerHealth>().GetPlayerHealth() > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Shoot()
    {
        //Set the cooldown
        cooldown = enemyWeapon.GetTimeBetweenShots();

        //Create the bullet
        Vector3 headPos = new Vector3(player.position.x + player.GetComponentInParent<NavMeshAgent>().velocity.x * accuracyModifier, player.position.y + .75f, player.position.z + player.GetComponentInParent<NavMeshAgent>().velocity.z * accuracyModifier);
        Vector3 direction = (headPos - weaponParent.GetChild(0).Find("Barrel").transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        GameObject newBullet = Instantiate(enemyWeapon.bulletPrefab, weaponParent.GetChild(0).Find("Barrel").transform.position, lookRotation);
        newBullet.GetComponent<Bullet>().SetBulletDmaage(enemyWeapon.bulletDamage);
        newBullet.GetComponent<Bullet>().SetBulletSpeed(enemyWeapon.bulletSpeed);
        Destroy(newBullet, 5);

        //Fire the weapon
        currentClip--;

        //Effects
        aud.PlayOneShot(enemyWeapon.GetUseSound());

        GameObject flash = Instantiate(enemyWeapon.muzzleFlash, weaponParent.GetChild(0).Find("Barrel").transform.position, Quaternion.LookRotation(-direction));
        Destroy(flash, 1);
    }

    IEnumerator Reload()
    {
        reloading = true;

        //Set the animations
        anim.SetLayerWeight(1, 0);
        anim.SetLayerWeight(2, 1);        
        anim.Play("Reload", 2);

        yield return new WaitForSeconds(enemyWeapon.GetReloadTime());

        //Reset animations
        anim.SetLayerWeight(2, 0);
        anim.Play("Default", 2);

        //Reload on script
        currentClip = clipSize;

        reloading = false;
    }
    #endregion

    private void SetDestination(Vector3 point)
    {
        agent.isStopped = false;
        agent.SetDestination(point);
    }

    private void PlayFootstep()
    {
        float oldPitch = aud.pitch;
        footTimer = footstepsCooldown;

        aud.pitch = UnityEngine.Random.Range(0.1f, 0.6f);
        aud.PlayOneShot(footsteps[UnityEngine.Random.Range(0, footsteps.Length)], 1f);

        aud.pitch = oldPitch;
    }
}
