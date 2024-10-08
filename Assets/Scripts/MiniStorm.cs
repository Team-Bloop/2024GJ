using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MiniStorm : MonoBehaviour
{
    [SerializeField]
    private float stormDamageValue = 20f;
    [SerializeField]
    private float stormDamageRate = 1f;

    [SerializeField]
    private float sizeLimit = 30;
    [SerializeField]
    private float expandRate = 0.01f;
    [SerializeField]
    private float pulseRange = 0.8f;
    [SerializeField]
    private float pulseRate = 2;
    [SerializeField]
    private bool pulsing = true;

/*    [SerializeField]
    private int minChaseLevel = 2;*/
    [SerializeField]
    private float chaseRate = 0.1f;
    [SerializeField]
    private float maxSpeed = 2f;

    public SpriteRenderer SelectedIndicator;

    private GameObject player;
    PlayerManager playerManager;

    bool playerDetected = false;
    Coroutine stormDamage;
    bool stormDamageActive = false;

    bool shrinking = true;
    Vector3 currentScale;
    Vector3 shrinkScale;
    Vector3 expandScale;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            if (stormDamage != null)
                StopCoroutine(stormDamage);
            stormDamageActive = false;
            playerDetected = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            playerDetected = true;
        }
    }

    private void Start()
    {
        player = GameObject.Find("Player");
        playerManager = player.GetComponent<PlayerManager>();

        currentScale = transform.localScale;
        shrinkScale = currentScale * pulseRange;
        expandScale = currentScale;
    }

    private void Update()
    {
        if (playerDetected && !stormDamageActive)
        {
            stormDamage = StartCoroutine(StormContactDamage());
        }

        Expand();
        if (pulsing)
            Pulse();
        else
            transform.localScale = currentScale;
        if (transform.localScale.x < 0.2f)
        {
            Collapse();
        }

        //if (playerManager.Level > minChaseLevel)
        ChasePlayer();
    }

    /// <summary>
    /// Storm Damage method
    /// </summary>
    /// <returns></returns>
    IEnumerator StormContactDamage()
    {
        stormDamageActive = true;
        if (playerDetected)
        {
            Debug.Log("Player Damaged by Mini Storm");
            playerManager.Damage(stormDamageValue);
            yield return new WaitForSeconds(stormDamageRate);
        }
        stormDamageActive = false;
    }

    /// <summary>
    /// Increases the size of the storm over time, adjusted by ExpandRate
    /// </summary>
    public void Expand()
    {
        if (currentScale.x < sizeLimit)
            currentScale += currentScale * expandRate * Time.deltaTime;
    }

    public void SetSize(Vector3 newSize)
    {
        currentScale = newSize;
    }

    /// <summary>
    /// Makes the storm pulse in size, the
    ///     - frequency can be adjusted based on PulseRate
    ///     - size range can be adjusted based on PulseRange
    /// </summary>
    public void Pulse()
    {
        float scaleX;
        float scaleY;
        float scaleZ;

        expandScale = currentScale;
        shrinkScale = currentScale * pulseRange;
        if (shrinking)
        {
            scaleX = Mathf.Lerp(transform.localScale.x, shrinkScale.x, pulseRate * Time.deltaTime);
            scaleY = Mathf.Lerp(transform.localScale.y, shrinkScale.y, pulseRate * Time.deltaTime);
            scaleZ = Mathf.Lerp(transform.localScale.z, shrinkScale.z, pulseRate * Time.deltaTime);
        }
        else
        {
            scaleX = Mathf.Lerp(transform.localScale.x, expandScale.x, pulseRate * Time.deltaTime);
            scaleY = Mathf.Lerp(transform.localScale.y, expandScale.y, pulseRate * Time.deltaTime);
            scaleZ = Mathf.Lerp(transform.localScale.z, expandScale.z, pulseRate * Time.deltaTime);
        }

        transform.localScale = new Vector3(scaleX, scaleY, scaleZ);

        if (shrinkScale.x / transform.localScale.x > 0.95)
        {
            shrinking = false;

        }
        else if (transform.localScale.x / expandScale.x > 0.95)
        {
            shrinking = true;

        }
    }

    /// <summary>
    /// Percentage of storm decreased
    /// </summary>
    /// <param name="rate"></param>
    public void Shrink(float rate)
    {
        transform.localScale *= rate;
    }

    public void Collapse()
    {
        if (stormDamage != null)
            StopCoroutine(stormDamage);
        MiniStormSpawner.StormCount--;
        Destroy(this.gameObject);
    }

    public void Indicate(bool indicate)
    {
        SelectedIndicator.enabled = indicate;
    }

    private void ChasePlayer()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance < 0.01f)
            return;
        float speed = chaseRate * playerManager.GetCurrentLevel();
        if (speed > maxSpeed)
        {
            speed = maxSpeed;
        }
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }
}