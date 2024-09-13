using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormBorderManager : MonoBehaviour
{
    [SerializeField]
    PlayerManager playerManager;

    [SerializeField]
    StormWarning stormWarning;

    [SerializeField]
    float shrinkRate = 0.01f;
    [SerializeField]
    float borderDamageValue = 1;
    [SerializeField]
    float borderDamageRate = 2f;

    bool playerDetected = true;
    bool borderDamageActive = false;

    Coroutine damageCoroutine;

    private void Update()
    {
        if (playerManager.GetCurrentLevel() > 2)
            Shrink();

        if (playerDetected && !borderDamageActive)
        {
            damageCoroutine = StartCoroutine(OutOfBorderDamage());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (damageCoroutine != null)
                StopCoroutine(damageCoroutine);
            playerDetected = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerDetected = false;
        }
    }

    IEnumerator OutOfBorderDamage()
    {
        borderDamageActive = true;
        while (playerDetected)
        {
            Debug.Log("Player Damaged by Mini Storm");
            playerManager.Damage(borderDamageValue);
            yield return new WaitForSeconds(borderDamageRate);
        }
        borderDamageActive = false;
    }

    /// <summary>
    /// Shrinks the border bit by bit and activates warning flash when at at scale 0.2
    /// </summary>
    public void Shrink()
    {
        if (transform.localScale.x > 0.001f)
            transform.localScale -= transform.localScale * shrinkRate * Time.deltaTime;
        if (transform.localScale.x < 0.2)
            stormWarning.FlashSwitch(true);
        else
            stormWarning.FlashSwitch(false);
    }
}
