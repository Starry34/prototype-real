using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Target : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anim;
    public Transform[] targets;
    int targetindex = 0;
    float reachDistance = 1.2f;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float distance2target = Vector3.Distance(transform.position, targets[targetindex].position);
        agent.destination = targets[targetindex].position;
        Debug.Log(distance2target);
        if (distance2target > reachDistance)
        {
            anim.SetBool("Walk", true);
        }
        else
        {
            anim.SetBool("Walk",false);
            targetindex++;
            if (targetindex > targets.Length - 1)
            {
                targetindex = 0;
            }
        }
    }
}
