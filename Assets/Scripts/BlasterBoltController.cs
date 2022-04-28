using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlasterBoltController : MonoBehaviour
{
    public PlayerCombatController playerCombatController;
    public Vector3 firePosition;
    int enemiesHit = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if ((transform.position - firePosition).magnitude > playerCombatController.range) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        EnemyController enemyHit = other.gameObject.GetComponent<EnemyController>();
        if (!other.isTrigger && enemyHit != null) {
            enemyHit.health--;
            enemyHit.state = EnemyController.EnemyState.STUNNED;
            enemyHit.timeToUnstun = Time.time + playerCombatController.stunTime;
            if (enemyHit.health <= 0) {
                Destroy(enemyHit.gameObject);
            }

            enemiesHit++;
            if (enemiesHit > playerCombatController.pierce && gameObject != null) {
                Destroy(gameObject);
            }
        } else if (other.name == "Walls" || other.name == "Doors") {
            if (gameObject != null) {
                Destroy(gameObject);
            }
        }
    }
}
