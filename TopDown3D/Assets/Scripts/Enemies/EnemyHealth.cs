using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System;

public class EnemyHealth : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Animator anim = null;
    [SerializeField] CapsuleCollider collider = null;
    [SerializeField] NavMeshAgent agent = null;

    [Header("Health and Item Settings")]
    [SerializeField] ItemDrop drop = null;
    [SerializeField] float maxHealth = 100;
    [SerializeField] float currentHealth;

    [Header("Effects")]
    Light light = null;
    [SerializeField] RawImage healthBar = null;

    [Header("Experience Settings")]
    [SerializeField] int xpOnDeath;
    [SerializeField] XPSpawn xpGainObject = null;

    private void Start()
    {
        //Initialize
        currentHealth = maxHealth;
        light = GetComponentInChildren<Light>();

        healthBar.rectTransform.localScale = new Vector3(currentHealth / maxHealth, 1, 1);

        //Subscribe to player event
        FindObjectOfType<PlayerExperience>().GetComponent<PlayerExperience>().LevelIncrease.AddListener(OnPlayerLevelUp);
    }

    //Damage and death
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        healthBar.rectTransform.localScale = new Vector3(currentHealth / maxHealth, 1, 1);

        GetComponent<EnemyAI>().DetectIfPlayerAttacks();
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        //Disable everything
        drop.EnableDrop();
        collider.enabled = false;
        agent.enabled = false;
        light.enabled = false;
        healthBar.transform.parent.gameObject.SetActive(false);

        //Experience for kill
        XPSpawn newXPSpawn = Instantiate(xpGainObject, transform.position, transform.rotation);
        newXPSpawn.SetXP(xpOnDeath);
        Destroy(newXPSpawn, 2);

        anim.Play("Death");
        Destroy(gameObject, 50);
    }

    public float GetEnemyHealth()
    {
        return currentHealth;
    }

    //Difficulty scaling
    private void OnPlayerLevelUp(int level)
    {

    }
}
