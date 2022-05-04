using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    public float timeBetweenShots;
    public float range;
    public float boltSpeed;
    public int pierce;
    public Transform blasterBoltPrefab;
    float lastShotTime = 0;
    public float stunTime;
    float invincibilityTime = 0.5f;
    float lastDamageTime = 0;
    public PlayerStats playerStats;
    public PlayerLifeTracker playerTrailTracker;
    private AudioSource source;
    public AudioClip laserSound;
    public AudioClip hurtSound;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();        
    }

    public void TakeDamage(float damageTaken) {
        if (lastDamageTime + invincibilityTime <= Time.time) {
            playerStats.PlayerHealth -= damageTaken;
            lastDamageTime = Time.time;

            if (playerStats.PlayerHealth <= 0) {
                playerTrailTracker.alive = false;
            } else {
                source.PlayOneShot(hurtSound, 1.0f);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetMouseButton(0) && lastShotTime + timeBetweenShots < Time.time && GetComponent<PlayerLifeTracker>().alive) {
            // Find where bolt should go
            Vector3 target = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 boltVelocity = (target - transform.position).normalized * boltSpeed;
            // Create and initialize bolt
            Transform newBolt = Instantiate(blasterBoltPrefab, transform.position, Quaternion.identity);
            newBolt.GetComponent<Rigidbody2D>().velocity = boltVelocity;
            newBolt.GetComponent<BlasterBoltController>().playerCombatController = this;
            newBolt.GetComponent<BlasterBoltController>().firePosition = transform.position;
            source.PlayOneShot(laserSound, 0.1f);
            // Reset for next shot
            lastShotTime = Time.time;
        }
    }
    
}
