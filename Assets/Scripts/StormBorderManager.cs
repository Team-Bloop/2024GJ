using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormBorderManager : MonoBehaviour
{
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
        if (collision.gameObject.tag == "Player")
            playerDetected = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            playerDetected = true;
    }

    IEnumerator OutOfBorderDamage()
    {
        borderDamageActive = true;
        yield return new WaitForSeconds(borderDamageRate);
        if (!playerDetected)
        {
            playerManager.Damage(borderDamageValue);
        }
        borderDamageActive = false;
    }

    /// <summary>
    /// 
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
