using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToTarget : MonoBehaviour
{

    private NavMeshAgent navMeshAgent;
    private GameObject target;
    private Rigidbody targetRB;


    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        target = GameObject.Find("Player");
        if(target != null)
        {
            targetRB = target.GetComponent<Rigidbody>();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        MoveTowardsTarget();
    }

    private void MoveTowardsTarget()
    {
        navMeshAgent.SetDestination(targetRB.transform.position);
    }
}
