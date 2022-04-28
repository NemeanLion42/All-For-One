using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthUI : MonoBehaviour
{
    // Start is called before the first frame update
    public Image batteryEmpty;
    public Image batteryPartial;
    public Image batteryFull;

    public PlayerStats playerStats;

    int spriteWidth = 100; // for the rect

    float currentHealth;
    float maxHealth;

    public float CurrentHealth {
        set {
            float use_health = Mathf.Clamp(value, 0.0f, maxHealth);
            currentHealth = use_health;

            float numOfFull = Mathf.Floor(currentHealth);
            float numOfHalves = numOfFull + (currentHealth - numOfFull) * 2f;

            batteryPartial.rectTransform.sizeDelta = new Vector2(numOfHalves*spriteWidth, spriteWidth);
            batteryFull.rectTransform.sizeDelta = new Vector2(numOfFull*spriteWidth, spriteWidth);


        } 
        get{
            return currentHealth;
        }
    }

    public float MaxHealth {
        set {
            float use_health = Mathf.Max(1.0f, value);
            maxHealth = use_health;

            Debug.Log("Setting max health to be "+use_health.ToString());

            batteryEmpty.rectTransform.sizeDelta = new Vector2(maxHealth*spriteWidth, spriteWidth);
        }
        get {
            return maxHealth;
        }
    }

    void Start()
    {
        // subscribe to player health event
        PlayerStats.OnCurrentHealthUpdate += OnPlayerHealthUpdate;
        PlayerStats.OnMaxHealthUpdate += OnPlayerMaxHealthUpdate;

        OnPlayerMaxHealthUpdate(playerStats.PlayerMaxHealth);
        OnPlayerHealthUpdate(playerStats.PlayerHealth);
        // playerStats.PlayerMaxHealth = playerStats.startingPlayerMaxHealth;
        // playerStats.PlayerHealth = playerStats.startingPlayerHealth;
    }

    void OnDisable() {
        PlayerStats.OnCurrentHealthUpdate -= OnPlayerHealthUpdate;
        PlayerStats.OnMaxHealthUpdate -= OnPlayerMaxHealthUpdate;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnPlayerHealthUpdate(float newHealth) {
        CurrentHealth = newHealth;
    }

    void OnPlayerMaxHealthUpdate(float newMaxHealth) {
        MaxHealth = newMaxHealth;
    }
}
