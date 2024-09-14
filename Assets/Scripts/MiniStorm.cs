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
    private float sizeLimit = 100;
    [SerializeField]
    private float expandRate = 0.01f;
    [SerializeField]
    private float pulseRange = 0.8f;
    [SerializeField]
    private float pulseRate = 2;
    [SerializeField]
    private bool pulsing = true;

    [SerializeField]
    private int minChaseLevel = 2;
    [SerializeField]
    private float chaseRate = 0.5f;

    public SpriteRenderer SelectedIndicator;
    public GameObject Player;

    Coroutine stormDamage;
    PlayerManager playerManager;

    bool playerDetected = false;
    bool stormDamageActive = false;

    bool shrinking = true;
    bool chasePlayer = true;
    Vector3 currentScale;
    Vector3 shrinkScale;
    Vector3 expandScale;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == Player)
        {
            if (stormDamage != null)
                StopCoroutine(stormDamage);
            stormDamageActive = false;
            playerDetected = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject == Player)
        {
            playerDetected = true;
        }
    }

    private void Start()
    {
        playerManager = Player.GetComponent<PlayerManager>();

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

        if (playerManager.Level > minChaseLevel && chasePlayer)
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
        Destroy(this.gameObject);
    }

    public void Indicate(bool indicate)
    {
        SelectedIndicator.enabled = indicate;
    }

    private void ChasePlayer()
    {
        float distance = Vector3.Distance(transform.position, Player.transform.position);
        if (distance < 0.01f)
            return;
        transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, chaseRate * Time.deltaTime);
    }
}

/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MiniStorm : MonoBehaviour
{
    [SerializeField]
    private float stormDamageValue = 1f;
    [SerializeField]
    private float stormDamageRate = 1f;

    [SerializeField]
    private float sizeLimit = 100;
    [SerializeField]
    private float expandRate = 0.01f;
    [SerializeField]
    private float pulseRange = 0.8f;
    [SerializeField]
    private float pulseRate = 2;
    [SerializeField]
    private bool pulsing = true;

    [SerializeField]
    private int minChaseLevel = 2;
    [SerializeField]
    private float chaseRate = 0.1f;

    public SpriteRenderer SelectedIndicator;
    public GameObject Player;

    Coroutine stormDamage;
    PlayerManager playerManager;

    bool playerDetected = false;
    bool stormDamageActive = false;

    bool shrinking = true;
    bool chasePlayer = true;
    Vector3 currentScale;
    Vector3 shrinkScale;
    Vector3 expandScale;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject == Player)
        {
            playerDetected = true;
        }
    }

    private void Start()
    {
        playerManager = Player.GetComponent<PlayerManager>();

        currentScale = transform.localScale;
        shrinkScale = currentScale * pulseRange;
        expandScale = currentScale;
    }

    private void Update()
    {
        if (playerDetected && !stormDamageActive)
        {
            Debug.Log("Player Detected by: " + this.GetInstanceID());
            stormDamage = StartCoroutine(StormContactDamage());
        } else if (stormDamage != null)
        {
            StopCoroutine(stormDamage);
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

        if (playerManager.Level > minChaseLevel && chasePlayer)
            ChasePlayer();

        playerDetected = false;
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
            Debug.Log("Player Damaged by Mini Storm: " + this.GetInstanceID());
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
        Destroy(this.gameObject);
    }

    public void Indicate(bool indicate)
    {
        SelectedIndicator.enabled = indicate;
    }

    private void ChasePlayer()
    {
        float distance = Vector3.Distance(transform.position, Player.transform.position);
        if (distance < 0.01f)
            return;
        transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, chaseRate * Time.deltaTime);
    }
}*/