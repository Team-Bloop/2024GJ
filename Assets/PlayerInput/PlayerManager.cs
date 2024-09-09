using System;
using UnityEngine;
using GeneralUtility;

public class PlayerManager : MonoBehaviour
{
    private const float MIN_MAX_HEALTH = 1f;

    [SerializeField]
    [Min(MIN_MAX_HEALTH)]
    private float maxHealth;

    [SerializeField]
    [Min(0f)]
    private float movementSpeed;

    private float currentHealth;
    private int level;

    void Start()
    {
        currentHealth = maxHealth;
        level = 0;
    }

    private void Reset()
    {
        maxHealth = MIN_MAX_HEALTH;
    }

    public float MaxHealth
    {
        get { return maxHealth; }
        set 
        { 
            if (value < MIN_MAX_HEALTH)
            {
                Utility.Quit();
                throw new ArgumentException($"Max Health cannot be less than {MIN_MAX_HEALTH}");
            }
            maxHealth = value; 
        }
    }

    public float MovementSpeed
    {
        get { return movementSpeed; }
        set
        {
            if (value < 0f)
            {
                Utility.Quit();
                throw new ArgumentException("Movement Speed cannot be less than 0");
            }
            movementSpeed = value;
        }
    }

    public float CurrentHealth
    {
        get { return currentHealth; }
    }

    public int Level
    {
        get { return level; }
    }

    // Use negative amt to "heal"
    public float Damage(float amt)
    {
        if (amt == 0)
        {
            Debug.LogWarning("Damage() arg is 0 which does nothing.");
        }
        
        currentHealth -= amt;
        if (currentHealth <= 0f)
        {
            Die();
        }
        return currentHealth;
    }

    public float GetHealthPercentage()
    {
        return currentHealth / maxHealth;   
    }

    public int IncreaseLevel(int amt)
    {
        if (amt < 0)
        {
            Utility.Quit();
            throw new ArgumentException("IncreaseLevel() arg cannot be less than 0");
        }

        level += amt;
        return level;
    }

    private void Die()
    {
        Debug.Log("PLAYER DED\nShould prob do smth here");
    }
}

