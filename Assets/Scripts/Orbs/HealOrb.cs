using UnityEngine;

public class HealOrb : OrbBase
{
    [SerializeField]
    [Min(1)]
    private int heal;

    private void Reset()
    {
        heal = 1;
    }

    protected override void OnCollected(Collider2D collision)
    {
        PlayerManager player = collision.GetComponent<PlayerManager>();
        Debug.Log(player.Damage(-heal));
        base.OnCollected(collision);
    }
}
