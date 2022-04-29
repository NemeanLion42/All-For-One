using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public Transform target;
    public NavMeshAgent agent;
    private Vector3 spawnPoint;
    public enum EnemyState {UNAWARE, PURSUIT, ATTACKING, STUNNED}
    public float timeToUnstun;
    public EnemyState state;
    public int health;
    public EnemyListController enemyListController;
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
        if (target.GetComponent<PlayerLifeTracker>().alive) {
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
                case EnemyState.STUNNED:
                    agent.isStopped = true;
                    agent.ResetPath();
                    if (Time.time > timeToUnstun) {
                        if (Camera.main.GetComponent<CameraController>().currentRoom == transform.parent.parent.gameObject) {
                            state = EnemyState.PURSUIT;
                        } else {
                            state = EnemyState.UNAWARE;
                        }
                    }
                    break;
            }
        } else {
            agent.isStopped = true;
            agent.ResetPath();
        }
        
    }

    private void OnCollisionStay2D(Collision2D other) {
        if (other.gameObject == target.gameObject) {
            target.GetComponent<PlayerCombatController>().TakeDamage(0.5f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject == target.gameObject && state != EnemyState.STUNNED) {
            state = EnemyState.ATTACKING;
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject == target.gameObject && state != EnemyState.STUNNED) {
            state = EnemyState.PURSUIT;
        }
    }
}
