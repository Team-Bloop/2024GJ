using System;
using UnityEngine;
using GeneralUtility;
using System.Collections;

public class FakeOrb : OrbBase
{
    [SerializeField]
    private Color fakeTargetColor;

    [SerializeField]
    private Color[] colorImitations;

    [SerializeField]
    [Min(0.1f)]
    private float damage;

    [SerializeField]
    [Min(0.1f)]
    private float explosionForce;

    [SerializeField]
    [Tooltip("Percentage of time spent faking vs warning color transition during OnTriggerStay2D")]
    [Range(0, 1)]
    private float warningTimeRatio;

    [SerializeField]
    [Tooltip("Time in seconds before orb returns to a fake color")]
    [Min(0f)]
    private float pauseBeforeRevert;

    [SerializeField]
    [Tooltip("Time in seconds to revert to a fake color")]
    [Min(0f)]
    private float timeToRevert;

    private float fakeTimeRatio;
    private float fakeTime;
    private float warningTime;
    private float totalTime;
    private float currentWarningTime;
    private bool isWarning;
    private bool canRevert;

    private Rigidbody2D rb;
    private GameObject playerGameObject;

    protected FakeOrb() : base(false) { }

    private new void Start()
    {
        if (colorImitations.Length == 0)
        {
            Utility.Quit();
            throw new ArgumentException("FakeOrb needs at least 1 element in Color Imitations");
        }

        base.Start();

        int num = UnityEngine.Random.Range(0, colorImitations.Length);
        spriteRenderer.color = colorImitations[num];
        tempColor = spriteRenderer.color;
        initialColor = spriteRenderer.color;
        fakeTimeRatio = 1 - warningTimeRatio;
        currentWarningTime = 0f;
        isWarning = false;

        playerGameObject = GameObject.FindGameObjectWithTag("Player");
        rb = playerGameObject.GetComponent<Rigidbody2D>();
        enabled = false;
        canRevert = false;
    }

    private void Reset()
    {
        damage = 10f;
        warningTimeRatio = 0.5f;
        explosionForce = 1000f;
        pauseBeforeRevert = 0.5f;
        timeToRevert = 0.5f;
    }

    protected override void Update()
    {
        base.Update();
        if (canRevert && !GetComponent<Renderer>().isVisible) 
        {
            Debug.Log("asdjkaosdjlkas");
            enabled = false;
            canRevert = false;
        }
    }

    private void FixedUpdate()
    {
        if (!canRevert)
        {
            rb.AddForce((playerGameObject.transform.position - transform.position).normalized * explosionForce);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player")
        {
            return;
        }

        canRevert = false;
        PlayerManager player = collision.GetComponent<PlayerManager>();
        fakeTime = (1 - player.CollectSpeed) * InitialTimeToCollect * fakeTimeRatio;
        warningTime = (1 - player.CollectSpeed) * InitialTimeToCollect * warningTimeRatio;
        totalTime = fakeTime + warningTime;
    }

    protected override void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag != "Player")
        {
            return;
        }

        currentTime += Time.deltaTime;

        if (currentTime <= fakeTime)
        {
            ColorChange(currentTime, fakeTime, fakeTargetColor);
        } else if (currentTime <= totalTime)
        {
            if (!isWarning)
            {
                initialColor = spriteRenderer.color;
                isWarning = true;
            }

            currentWarningTime += Time.deltaTime;
            ColorChange(currentWarningTime, warningTime, targetColor);
        } else
        {
            OnCollected(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        enabled = true;
        canRevert = true;
    }

    protected override void OnCollected(Collider2D collision)
    {
        PlayerManager player = collision.GetComponent<PlayerManager>();
        player.Damage(damage);
        enabled = true;
        canRevert = false;
        base.OnCollected(collision);
    }

    private void ColorChange(float currentTime, float timeToCollect, Color targetColor)
    {
        float completionAmt = 0f;

        if (timeToCollect != 0)
        {
            completionAmt = currentTime / timeToCollect;
        }

        if (completionAmt >= 0 && completionAmt <= 1)
        {
            tempColor.r = Mathf.Lerp(initialColor.r, targetColor.r, completionAmt);
        }

        if (completionAmt >= 0 && completionAmt <= 1)
        {
            tempColor.g = Mathf.Lerp(initialColor.g, targetColor.g, completionAmt);
        }

        if (completionAmt >= 0 && completionAmt <= 1)
        {
            tempColor.b = Mathf.Lerp(initialColor.b, targetColor.b, completionAmt);
        }

        spriteRenderer.color = tempColor;
    }
}
