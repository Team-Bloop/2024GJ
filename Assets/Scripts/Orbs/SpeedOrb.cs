using System.Collections;
using UnityEngine;

public class SpeedOrb : OrbBase
{
    [SerializeField]
    [Tooltip("Movement Speed multiplier")]
    [Min(1f)]
    private float movementSpeedBuff;

    [SerializeField]
    [Tooltip("Collect Speed percentage between 0 and 1 (ie: 0.9 = 90%; 1 = instant collect)")]
    [Range(0f, 1f)]
    private float collectSpeedBuff;

    [SerializeField]
    [Min(0.1f)]
    private float duration;

    protected SpeedOrb() : base(true) { }

    private void Reset()
    {
        movementSpeedBuff = 1f;
        collectSpeedBuff = 0f;
        duration = 0.1f;
    }

    protected override void OnCollected(Collider2D collision)
    {
        PlayerManager player = collision.GetComponent<PlayerManager>();
        StartCoroutine(SpeedBuff(player));
        base.OnCollected(collision);
    }

    private IEnumerator SpeedBuff(PlayerManager player)
    {
        player.MovementSpeed *= movementSpeedBuff;
        player.CollectSpeed += collectSpeedBuff;
        yield return new WaitForSeconds(duration);

        player.MovementSpeed /= movementSpeedBuff;
        player.CollectSpeed = 0;
        Destroy(gameObject);
    }
}
