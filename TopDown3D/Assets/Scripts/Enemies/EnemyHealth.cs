using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] Animator anim = null;
    [SerializeField] CapsuleCollider collider = null;
    [SerializeField] NavMeshAgent agent = null;
    [SerializeField] ItemDrop drop = null;
    [SerializeField] float maxHealth = 100;
    [SerializeField] float currentHealth;

    [Header("Effects")]
    Light light = null;
    [SerializeField] RawImage healthBar = null;

    private void Start()
    {
        currentHealth = maxHealth;
        light = GetComponentInChildren<Light>();

        healthBar.rectTransform.localScale = new Vector3(currentHealth / maxHealth, 1, 1);
    }

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
        drop.EnableDrop();
        collider.enabled = false;
        agent.enabled = false;
        light.enabled = false;
        healthBar.transform.parent.gameObject.SetActive(false);
        anim.Play("Death");
    }


    public float GetEnemyHealth()
    {
        return currentHealth;
    }
}
