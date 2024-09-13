using UnityEngine;

public class SpeedOrb : OrbBase
{
    [SerializeField]
    [Tooltip("")]
    [Min(1f)]
    private float movementSpeed;

    [SerializeField]
    [Range(0f, 1f)]
    private float collectSpeed;

    [SerializeField]
    [Min(0f)]
    private float duration;

    private void Reset()
    {
        
    }

    protected override void OnCollected(Collider2D collision)
    {
        PlayerManager player = collision.GetComponent<PlayerManager>();
        base.OnCollected(collision);
    }
}
