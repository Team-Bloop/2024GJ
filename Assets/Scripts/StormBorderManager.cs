using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormBorderManager : MonoBehaviour
{
    [SerializeField]
    GameObject player;
    PlayerManager playerManager;

/*    [SerializeField]
    StormWarning stormWarning;*/

    [SerializeField]
    float shrinkRate = 0.01f;
    [SerializeField]
    float borderDamageValue = 1;
    [SerializeField]
    float borderDamageRate = 2f;
    [SerializeField]
    float borderClosedLevel = 30;

    bool playerDetected = false;
    Coroutine damageCoroutine;
    bool borderDamageActive = false;

    private void Start()
    {
        playerManager = player.GetComponent<PlayerManager>();
    }
    private void Update()
    {
        if (1f - playerManager.GetCurrentLevel() / borderClosedLevel <= transform.localScale.x && transform.localScale != Vector3.zero)
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

    private void OnTriggerStay2D(Collider2D collision)
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
            Debug.Log("Player Damaged by Border Storm");
            playerManager.Damage(borderDamageValue);
            yield return new WaitForSeconds(borderDamageRate);
        }
        borderDamageActive = false;
    }

    /// <summary>
    /// Shrinks the border bit by bit and activates warning flash when at at scale 0.2
    /// basically 20% of size before game starts
    /// </summary>
    public void Shrink()
    {
        if (this.GetComponent<Renderer>().bounds.size.x > 2)
        {
            transform.localScale -= transform.localScale * shrinkRate * Time.deltaTime;
        }
        else
        {
            playerDetected = true;
            StartCoroutine(OutOfBorderDamage());
            transform.localScale = Vector3.zero;
        }

        //Debug.Log("Border size: " + this.GetComponent<Renderer>().bounds.size);

        /*if (transform.localScale.x < 0.2)
            stormWarning.FlashSwitch(true);
        else
            stormWarning.FlashSwitch(false);*/
    }
}
