using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] Animator anim = null;
    [SerializeField] CapsuleCollider collider = null;
    [SerializeField] NavMeshAgent agent = null;
    [SerializeField] float maxHealth = 100;
    [SerializeField] float currentHealth;
    [SerializeField] AudioSource aud = null;
    [SerializeField] AudioClip[] takeDamageSounds = null;
    [SerializeField] RawImage healthBar = null;
    [SerializeField] GameObject regenPrefab = null;

    private void Start()
    {
        currentHealth = maxHealth;

        healthBar.rectTransform.localScale = new Vector3(currentHealth / maxHealth, healthBar.rectTransform.localScale.y, healthBar.rectTransform.localScale.z);
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        healthBar.rectTransform.localScale = new Vector3(currentHealth / maxHealth, healthBar.rectTransform.localScale.y, healthBar.rectTransform.localScale.z);

        aud.PlayOneShot(takeDamageSounds[Random.Range(0, takeDamageSounds.Length)]);

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        healthBar.rectTransform.localScale = new Vector3(0, 1, 1);

        collider.enabled = false;
        agent.enabled = false;
        anim.Play("Default", 1);
        anim.Play("Default", 2);
        anim.Play("Death");
    }

    public void AddHealth(float amount)
    {
        currentHealth += amount;

        GameObject healEffects = Instantiate(regenPrefab, transform.position, transform.rotation, transform);
        Destroy(healEffects, 1);


        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }

        healthBar.rectTransform.localScale = new Vector3(currentHealth / maxHealth, healthBar.rectTransform.localScale.y, healthBar.rectTransform.localScale.z);
    }

    public float GetPlayerHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }
}
