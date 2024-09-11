using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityManager : MonoBehaviour
{
    [SerializeField]
    float HealValue = 1;
    [SerializeField]
    float SpeedIncrement = 1;

    PlayerManager playerManager;
    StormDetect stormDetect;


    private void Start()
    {
        playerManager = GetComponent<PlayerManager>();
    }

    public void Heal()
    {
        playerManager.Damage(-HealValue);
    }

    public void IncreaseSpeed()
    {
        playerManager.MovementSpeed += SpeedIncrement;
    }

    public bool ShrinkStorm(float percentage)
    {
        if (stormDetect.SelectedStorm != null)
        {
            stormDetect.SelectedStorm.GetComponent<MiniStorm>().Shrink(percentage);
            return true;
        }
        return false;
    }

    public bool DestroyStorm()
    {
        if (stormDetect.SelectedStorm != null)
        {
            stormDetect.SelectedStorm.GetComponent<MiniStorm>().Collapse();
            return true;
        }
        return false;
    }
}
