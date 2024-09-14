using UnityEngine;

public class OrbBase : MonoBehaviour
{
    protected const float DEATH_ANIM_TIME = 0.5f;

    [SerializeField]
    [Tooltip("Time in seconds")]
    [Min(0f)]
    private float initialTimeToCollect;

    [SerializeField]
    [Tooltip("Color when orb has been collected")]
    protected Color targetColor;

    protected float currentTime;
    private float timeToCollect;

    protected SpriteRenderer spriteRenderer;
    protected Color initialColor;
    protected Color tempColor;
    private float completionAmt;
    private OrbSpawner orbSpawner;

    // temp inefficient way until we get sprite anims down
    protected bool hasOrbDuration;
    private bool isDying;
    private float deathAnimTimeCountdown;
    private new Transform transform;

    protected OrbBase(bool hasOrbDuration)
    {
        this.hasOrbDuration = hasOrbDuration;
    }

    protected void Start()
    {
        currentTime = 0f;
        timeToCollect = initialTimeToCollect;
        spriteRenderer = GetComponent<SpriteRenderer>();
        tempColor = spriteRenderer.color;
        initialColor = spriteRenderer.color;
        orbSpawner = GetComponentInParent<OrbSpawner>();

        isDying = false;
        deathAnimTimeCountdown = DEATH_ANIM_TIME;
        transform = GetComponent<Transform>();
    }

    protected virtual void Update()
    {
        if (isDying)
        {
            deathAnimTimeCountdown -= Time.deltaTime;
            float newScale = Mathf.Lerp(2, 0, deathAnimTimeCountdown);
            Vector3 vector3 = new Vector3(newScale, newScale, newScale);
            transform.localScale = vector3;

            tempColor.a = Mathf.Lerp(0, 1, deathAnimTimeCountdown);
            spriteRenderer.color = tempColor;

            if (deathAnimTimeCountdown <= 0)
            {
                orbSpawner.DecreaseOrbs();
                if (hasOrbDuration)
                {
                    enabled = false;
                } else
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player")
        {
            return;
        }

        PlayerManager player = collision.GetComponent<PlayerManager>();
        timeToCollect = (1 - player.CollectSpeed) * initialTimeToCollect;
    }

    protected virtual void OnTriggerStay2D(Collider2D collision)
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

    public float InitialTimeToCollect
    {
        get { return initialTimeToCollect; }
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
