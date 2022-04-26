using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshAgentController : MonoBehaviour
{
    public Transform target;
    NavMeshAgent agent;
    private Vector3 spawnPoint;
    public enum EnemyState {UNAWARE, PURSUIT, ATTACKING}
    public EnemyState state;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        state = EnemyState.UNAWARE;
        transform.SetPositionAndRotation(transform.position, Quaternion.identity);
        spawnPoint = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (state) {
            case EnemyState.UNAWARE:
                agent.isStopped = false;
                agent.SetDestination(spawnPoint);
                break;
            case EnemyState.PURSUIT:
                agent.isStopped = false;
                agent.SetDestination(target.position);
                break;
            case EnemyState.ATTACKING:
                agent.isStopped = true;
                agent.ResetPath();
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject == target.gameObject) {
            state = EnemyState.ATTACKING;
            // play sound for a hit
            // should trigger battery reduction
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject == target.gameObject) {
            state = EnemyState.PURSUIT;
        }
    }
}
