using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormBorderManager : MonoBehaviour
{
    PlayerManager playerManager;

    [SerializeField]
    float ShrinkRate = 0.01f;
    [SerializeField]
    float BorderDamageValue = 1;
    [SerializeField]
    float BorderDamageRate = 2f;

    bool playerDetected = true;
    bool borderDamageActive = false;

    private void Start()
    {
        playerManager = GetComponent<PlayerManager>();
    }
    private void Update()
    {
        Shrink();
        if (!playerDetected && !borderDamageActive)
        {
            StartCoroutine(OutOfBorderDamage());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        playerDetected = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerDetected = true;
    }

    IEnumerator OutOfBorderDamage()
    {
        borderDamageActive = true;
        yield return new WaitForSeconds(BorderDamageRate);
        if (!playerDetected)
        {
            playerManager.Damage(BorderDamageValue);
        }
        borderDamageActive = false;
    }

    /// <summary>
    /// 
    /// </summary>
    public void Shrink()
    {
        transform.localScale -= transform.localScale * ShrinkRate * Time.deltaTime;
    }
}
