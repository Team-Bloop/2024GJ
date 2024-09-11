using System;
using UnityEngine;
using GeneralUtility;

public class PlayerManager : MonoBehaviour
{
    private const float MIN_MAX_HEALTH = 1f;
    private const int MAX_CHARGES = 8;

    [SerializeField]
    [Min(MIN_MAX_HEALTH)]
    private float maxHealth;

    [SerializeField]
    [Min(MAX_CHARGES)]
    private float maxCharges;

    [SerializeField]
    [Min(0f)]
    private float movementSpeed;

    private float currentHealth;
    private float collectSpeed; // 0 - 1, ie: 0.9 -> 90% faster
    private int level;
    private int charges;

    void Start()
    {
        currentHealth = maxHealth;
        collectSpeed = 0f;
        level = 0;
    }

    private void Reset()
    {
        maxHealth = MIN_MAX_HEALTH;
        maxCharges = MAX_CHARGES;
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

    public float CollectSpeed
    {
        get { return collectSpeed; }
        set
        {
            if (value < 0 || value > 1)
            {
                Utility.Quit();
                throw new ArgumentException("CollectSpeed value needs to be between 0 and 1 (both inclusive)");
            }

            collectSpeed = value;
        }
    }

    public int Level
    {
        get { return level; }
    }

    public int Charges
    {
        get { return charges; }
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
        else if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
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

    // Use negative to decrease charges
    public int IncreaseCharges(int amt)
    {
        if (amt == 0)
        {
            Debug.LogWarning("IncreaseCharges() amt is 0 which does nothing");
        }

        charges += amt;

        if (charges > MAX_CHARGES)
        {
            charges = MAX_CHARGES; 
        }

        return charges;
    }

    private void Die()
    {
        Debug.Log("PLAYER DED\nShould prob do smth here");
    }
}

