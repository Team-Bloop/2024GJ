using UnityEngine;

public class Boop : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Time in seconds")]
    [Min(0f)]
    private float timeToCollect;

    [SerializeField]
    [Tooltip("Level(s) given to player when collected")]
    [Min(1)]
    private int level;

    [SerializeField]
    [Tooltip("Color when orb has been collected")]
    private Color targetColor;

    private float currentTime = 0f;
    private SpriteRenderer spriteRenderer;

    private Color initialColor;
    private Color tempColor;
    private float completionAmt;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        tempColor = spriteRenderer.color;
        initialColor = spriteRenderer.color;
    }

    private void Reset()
    {
        timeToCollect = 0f;
        level = 1;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        currentTime += Time.deltaTime;
        completionAmt = currentTime / timeToCollect;

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
