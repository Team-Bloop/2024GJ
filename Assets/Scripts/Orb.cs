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

    private float currentTime = 0f;

    private void Reset()
    {
        timeToCollect = 0f;
        level = 1;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        currentTime += Time.deltaTime;

        if (currentTime > timeToCollect)
        {
            OnCollected(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        currentTime = 0f;
    }

    private void OnCollected(Collider2D collision)
    {
        PlayerManager player = collision.GetComponent<PlayerManager>();
        Debug.Log(player.IncreaseLevel(level));
        Destroy(gameObject);
    }
}
