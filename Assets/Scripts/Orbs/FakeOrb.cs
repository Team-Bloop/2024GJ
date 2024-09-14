using System;
using UnityEngine;
using GeneralUtility;

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

    private float fakeTimeRatio;
    private float fakeTime;
    private float warningTime;
    private float totalTime;
    private float currentWarningTime;
    private bool isWarning;

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

        rb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        playerGameObject = GameObject.FindGameObjectWithTag("Player");
        enabled = false;
    }

    private void Reset()
    {
        damage = 10f;
        warningTimeRatio = 0.5f;
        explosionForce = 1000f;
    }

    private void FixedUpdate()
    {
        rb.AddForce((playerGameObject.transform.position - transform.position).normalized * explosionForce);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player")
        {
            return;
        }

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

    protected override void OnCollected(Collider2D collision)
    {
        PlayerManager player = collision.GetComponent<PlayerManager>();
        player.Damage(damage);
        enabled = true;
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
