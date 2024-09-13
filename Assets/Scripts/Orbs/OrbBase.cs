using UnityEngine;

public class OrbBase : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Time in seconds")]
    [Min(0f)]
    private float initialTimeToCollect;

    [SerializeField]
    [Tooltip("Color when orb has been collected")]
    private Color targetColor;

    private float currentTime;
    private float timeToCollect;

    private SpriteRenderer spriteRenderer;
    private Color initialColor;
    private Color tempColor;
    private float completionAmt;
    private OrbSpawner orbSpawner;

    // temp inefficient way until we get sprite anims down
    private bool isDying;
    private float deathAnimTime;
    private new Transform transform;

    private void Start()
    {
        currentTime = 0f;
        timeToCollect = initialTimeToCollect;
        spriteRenderer = GetComponent<SpriteRenderer>();
        tempColor = spriteRenderer.color;
        initialColor = spriteRenderer.color;
        orbSpawner = GetComponentInParent<OrbSpawner>();

        isDying = false;
        deathAnimTime = 0.5f;
        transform = GetComponent<Transform>();
    }

    private void Reset()
    {
        initialTimeToCollect = 0f;
    }

    private void Update()
    {
        if (isDying)
        {
            deathAnimTime -= Time.deltaTime;
            float newScale = Mathf.Lerp(2, 0, deathAnimTime);
            Vector3 vector3 = new Vector3(newScale, newScale, newScale);
            transform.localScale = vector3;

            tempColor.a = Mathf.Lerp(0, 1, deathAnimTime);
            spriteRenderer.color = tempColor;

            if (deathAnimTime < 0)
            {
                orbSpawner.DecreaseOrbs();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player")
        {
            return;
        }

        PlayerManager player = collision.GetComponent<PlayerManager>();
        timeToCollect = (1 - player.CollectSpeed) * initialTimeToCollect;

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag != "Player")
        {
            return;
        }

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

    protected virtual void OnCollected(Collider2D collision)
    {
        isDying = true;
        foreach (BoxCollider2D collider in GetComponents<BoxCollider2D>())
        {
            collider.enabled = false;
        }
    }
}
