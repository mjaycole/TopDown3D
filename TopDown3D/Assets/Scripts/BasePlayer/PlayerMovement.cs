using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent = null;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Animator anim = null;
    [SerializeField] GameObject clickEffect = null;
    bool moving = false;

    [SerializeField] AudioSource aud = null;
    [SerializeField] AudioClip selectAud = null;
    [SerializeField] AudioClip[] footsteps = null;
    [SerializeField] float footstepsCooldown;
    float cooldown;
    bool clickedUI;

    private void Update()
    {
        if (GetComponent<PlayerHealth>().GetPlayerHealth() <= 0) { return; }

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
        {
            AttemptSetDestination();
        }

        PlayAnimations();

        cooldown -= Time.deltaTime;

        if (agent.velocity != Vector3.zero && cooldown <= 0)
        {
            PlayFootstep();
        }
    }

    private void AttemptSetDestination()
    {
        if (clickedUI) { return; }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000f, groundLayer))
        {
            if (GetComponent<InventoryManager>().GetInventoryOpen()) { return; }

            SetDestination(hit.point);
            if (Input.GetMouseButtonDown(0))
            {
                if (hit.transform.gameObject.layer == 6)
                {
                    GameObject newEffect = Instantiate(clickEffect, hit.point, Quaternion.identity);
                    Destroy(newEffect, 1f);

                    aud.PlayOneShot(selectAud, .1f);
                }
            }
        }
    }



    private void SetDestination(Vector3 position)
    {
        if (Vector3.Distance(position, transform.position) > 1)
        {
            agent.isStopped = false;
            agent.SetDestination(position);
        }
    }
    private void PlayAnimations()
    {

        if (Vector3.Distance(transform.position, agent.destination) > .25f)
        {
            anim.SetBool("Run", true);
        }
        else
        {
            anim.SetBool("Run", false);
        }
    }

    private void PlayFootstep()
    {
        float oldPitch = aud.pitch;
        cooldown = footstepsCooldown;

        aud.pitch = Random.Range(0.1f, 0.6f);
        aud.PlayOneShot(footsteps[Random.Range(0, footsteps.Length)], .2f);

        aud.pitch = oldPitch;
    }
}
