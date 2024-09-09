using System;
using UnityEngine;
using GeneralUtility;

public class Orb : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Time in seconds")]
    [Min(0f)]
    private float initialTimeToCollect;

    [SerializeField]
    [Tooltip("Level(s) given to player when collected")]
    [Min(1)]
    private int level;

    [SerializeField]
    [Tooltip("Color when orb has been collected")]
    private Color targetColor;

    private float currentTime = 0f;
    private float timeToCollect;

    private SpriteRenderer spriteRenderer;
    private Color initialColor;
    private Color tempColor;
    private float completionAmt;

    private void Start()
    {
        timeToCollect = initialTimeToCollect;
        spriteRenderer = GetComponent<SpriteRenderer>();
        tempColor = spriteRenderer.color;
        initialColor = spriteRenderer.color;
    }

    private void Reset()
    {
        initialTimeToCollect = 0f;
        level = 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerManager player = collision.GetComponent<PlayerManager>();
        timeToCollect = (1 - player.CollectSpeed) * initialTimeToCollect;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        currentTime += Time.deltaTime;

        if (timeToCollect == 0)
        {
            completionAmt = 0;
        }
        else
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

        if (currentTime > timeToCollect)
        {
            OnCollected(collision);
        }
    }

    public float CompletionAmt
    {
        get { return completionAmt; }
    }

    private void OnCollected(Collider2D collision)
    {
        PlayerManager player = collision.GetComponent<PlayerManager>();
        Debug.Log(player.IncreaseLevel(level));
        Destroy(gameObject);
    }
}
