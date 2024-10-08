using UnityEngine;

public class BasicOrb : OrbBase
{
    [SerializeField]
    [Tooltip("Exp given to player when collected")]
    [Min(1)]
    private int exp;

    [SerializeField]
    [Tooltip("Charge(s) given to player when collected")]
    [Min(1)]
    private int charge;

    protected BasicOrb() : base(false) { }

    private void Reset()
    {
        exp = 1;
        charge = 1;
    }

    protected override void OnCollected(Collider2D collision)
    {
        PlayerManager player = collision.GetComponent<PlayerManager>();
        /*Debug.Log(player.IncreaseEXP(exp));
        Debug.Log($"Current Charge: {player.IncreaseCharges(charge)}");*/
        player.IncreaseEXP(exp);
        player.IncreaseCharges(charge);
        player.IncreaseScore(Mathf.RoundToInt(0.5f * player.GetCurrentLevel()));
        base.OnCollected(collision);
    }
}
