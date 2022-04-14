using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshAgentController : MonoBehaviour
{
    public Transform target;
    NavMeshAgent agent;
    public bool shouldPursue;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        shouldPursue = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (shouldPursue) {
            agent.isStopped = false;
            agent.SetDestination(target.position);
        } else {
            agent.isStopped = true;
            agent.ResetPath();
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject == target.gameObject) {
            shouldPursue = false;
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject == target.gameObject) {
            shouldPursue = true;
        }
    }
}
