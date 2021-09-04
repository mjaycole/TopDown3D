using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentObstacles : MonoBehaviour
{
    [SerializeField] LayerMask wallLayer;
    [SerializeField] Shader transparentShader = null;
    public List<MeshRenderer> rendersInTheWay = new List<MeshRenderer>();
    public List<Shader> defaultShaders = new List<Shader>();
    Transform player;

    private void Start()
    {
        player = GameObject.FindObjectOfType<PlayerHealth>().transform;
    }


    private void FixedUpdate()
    {
        RaycastHit hit;
        Vector3 rayDirection = new Vector3(player.position.x, player.position.y + 1, player.position.z) - transform.position;

        Debug.DrawRay(transform.position, rayDirection);

        if (Physics.Raycast(transform.position, rayDirection, out hit, 1000f, wallLayer))
        {
            foreach (MeshRenderer t in hit.transform.GetComponentsInChildren<MeshRenderer>())
            {
                if (rendersInTheWay.Contains(t))
                {
                    return;
                }

                rendersInTheWay.Add(t);
                defaultShaders.Add(t.material.shader);
                t.material.shader = transparentShader;
            }
        }
        else
        {
            for (int i = 0; i < rendersInTheWay.Count; i++)
            {
                rendersInTheWay[i].material.shader = defaultShaders[i];
                rendersInTheWay.RemoveAt(i);
                defaultShaders.RemoveAt(i);
            }

        }
    }
}
