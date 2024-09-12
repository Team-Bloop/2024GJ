using System;
using UnityEngine;
using GeneralUtility;
using UI;

public class PlayerManager : MonoBehaviour
{
    private const float MIN_MAX_HEALTH = 1f;
    private const int MIN_MAX_CHARGES = 8;

    [SerializeField]
    [Min(MIN_MAX_HEALTH)]
    private float maxHealth;

    [SerializeField]
    [Min(MIN_MAX_CHARGES)]
    private int maxCharges;

    [SerializeField]
    [Min(0f)]
    private float movementSpeed;

    private float currentHealth;
    private float collectSpeed; // 0 - 1, ie: 0.9 -> 90% faster

    private int exp;
    private int level; // at the moment each level will only require 20 exp
    private int charges;
    private int maxExpPerLevel = 20;

    [SerializeField]
    private GameObject HP_UI;
    [SerializeField]
    private GameObject EXP_UI;
    [SerializeField]
    private GameObject AP_Sprite;

    void Start()
    {
        currentHealth = maxHealth;
        collectSpeed = 0f;
        exp = 0;
    }

    private void Reset()
    {
        maxHealth = MIN_MAX_HEALTH;
        maxCharges = MIN_MAX_CHARGES;
        movementSpeed = 10;
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
        float percentageDamage = amt / currentHealth;
        HP_UI.GetComponent<HPUI>().changeHPBarPosition(percentageDamage);
        // the line below will damage the player
        // the float is the percetange of total hp lost by the player
        // HP_UI.GetComponent<HPUI>().changeHPBarPosition(0.1f);
        // the line below will recover health for the player
        // the float is the percentage of total hp recovered by the player
        // HP_UI.GetComponent<HPUI>().changeHPBarPosition(-0.1f);
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
        EXP_UI.GetComponent<EXPUI>().updateEXPUI(GetEXPPercentage(), getCurrentLevel());
        //Debug.Log($"CURRENT EXP: {exp}");
        return exp;
    }

    public float GetEXPPercentage() {
        float currentlvlExp = exp % maxExpPerLevel;
        Debug.Log($"LEVEL EXP: " + currentlvlExp);
        return currentlvlExp / maxExpPerLevel;
    }

    public int getCurrentLevel() {
        level = exp / maxExpPerLevel + 1;
        //Debug.Log($"CURRENT LEVEL: {level}");
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

        if (charges > maxCharges)
        {
            charges = maxCharges; 
        }

        AP_Sprite.GetComponent<AP_UI>().updateSprite(charges);
        return charges;
    }

    private void Die()
    {
        Debug.Log("PLAYER DED\nShould prob do smth here");
    }
}

