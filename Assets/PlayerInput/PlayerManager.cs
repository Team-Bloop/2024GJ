using System;
using UnityEngine;
using GeneralUtility;
using UI;

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
    private float collectSpeed; // 0 - 1, ie: 0.9 -> 90% faster
    private int exp;
    private int level; // at the moment each level will only require 20 exp
    private int maxExpPerLevel = 20;

    [SerializeField]
    private GameObject EXPUI;

    void Start()
    {
        currentHealth = maxHealth;
        collectSpeed = 0f;
        exp = 0;
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
        get { return exp; }
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

    public int IncreaseEXP(int amt)
    {
        if (amt < 0)
        {
            Utility.Quit();
            throw new ArgumentException("IncreaseLevel() arg cannot be less than 0");
        }

        exp += amt;
        EXPUI.GetComponent<EXPUI>().updateEXPUI(GetEXPPercentage(), getCurrentLevel());
        //Debug.Log($"CURRENT EXP: {exp}");
        return exp;
    }

    public float GetEXPPercentage() {
        float currentlvlExp = exp % maxExpPerLevel;
        //Debug.Log($"LEVEL EXP: " + currentlvlExp);
        return currentlvlExp / maxExpPerLevel;
    }

    public int getCurrentLevel() {
        level = exp / maxExpPerLevel + 1;
        //Debug.Log($"CURRENT LEVEL: {level}");
        return level;
    }

    private void Die()
    {
        Debug.Log("PLAYER DED\nShould prob do smth here");
    }
}

