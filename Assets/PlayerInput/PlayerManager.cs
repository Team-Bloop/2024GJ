using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using GeneralUtility;
using UI;

public class PlayerManager : MonoBehaviour
{
    private const float MIN_MAX_HEALTH = 1f;
    private const int MAX_CHARGES = 8;

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
            PlayerController playerController = GetComponent<PlayerController>();
            playerController.MoveSpeed = value;
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
            if (value > 1)
            {
                collectSpeed = 1;
                Debug.LogWarning("Collect Speed value more than 1, variable has been set to 1");
            } else if (value < 0)
            {
                collectSpeed = 0;
                Debug.LogWarning("Collect Speed value less than 0, variable has been set to 0");
            } else
            {
                collectSpeed = value;
            }
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
        EXP_UI.GetComponent<EXPUI>().updateEXPUI(GetEXPPercentage(), GetCurrentLevel());
        return exp;
    }

    public float GetEXPPercentage() {
        float currentlvlExp = exp % maxExpPerLevel;
        Debug.Log($"LEVEL EXP: " + currentlvlExp);
        return currentlvlExp / maxExpPerLevel;
    }

    public int GetCurrentLevel() {
        level = exp / maxExpPerLevel + 1;
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

        AP_Sprite.GetComponent<AP_UI>().updateSprite(charges);
        return charges;
    }

    private void Die()
    {
        Debug.Log("PLAYER DED\nShould prob do smth here");
        SceneManager.LoadScene("GAME_OVER");
    }
}

