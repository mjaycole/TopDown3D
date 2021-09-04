using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] bool playerBullet = false;
    [SerializeField] float moveSpeed = 5;
    [SerializeField] float damage = 10;
    [SerializeField] AudioSource aud = null;
    [SerializeField] AudioClip impactSound = null;
    [SerializeField] GameObject impactParticles = null;


    public void SetBulletSpeed(float amount)
    {
        moveSpeed = amount;
    }
    public void SetBulletDmaage(float amount)
    {
        damage = amount;
    }

    void Update()
    {
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (playerBullet)
        {
            if (collision.gameObject.layer == 7)
            {
                if (collision.GetComponent<EnemyHealth>() != null)
                    collision.GetComponent<EnemyHealth>().TakeDamage(damage);
            }
        }
        else
        {
            if (collision.gameObject.layer == 9)
            {
                if (collision.GetComponent<PlayerHealth>() != null)
                    collision.GetComponent<PlayerHealth>().TakeDamage(damage);
            }
        }

        aud.PlayOneShot(impactSound);
        GameObject flash = Instantiate(impactParticles, transform.position, transform.rotation);
        foreach (Transform t in transform.GetComponentInChildren<Transform>())
        {
            t.gameObject.SetActive(false);
        }

        Destroy(flash, .25f);
        Destroy(gameObject, .15f);
    }
}
