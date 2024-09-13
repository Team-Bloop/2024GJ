using UnityEngine;

public class SpeedOrb : OrbBase
{
    [SerializeField]
    [Tooltip("Movement Speed multiplier")]
    [Min(1f)]
    private float movementSpeed;

    [SerializeField]
    [Tooltip("Collect Speed percentage between 0 and 1 (ie: 0.9 = 90%; 1 = instant collect)")]
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
