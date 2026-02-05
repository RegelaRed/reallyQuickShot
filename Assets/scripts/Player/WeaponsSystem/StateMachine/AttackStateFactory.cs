public class AttackStateFactory
{
    private PlayerWeaponsManager _ctx;
    public AttackStateFactory(PlayerWeaponsManager ctx) { _ctx = ctx; }

    public AttackStatebase Idle() { return new Idle(_ctx, this); }
    public AttackStatebase Melee() { return new Melee(_ctx, this); }
    public AttackStatebase MeleeCharged() { return new MeleCharged(_ctx, this); }

    public AttackStatebase Ranged() { return new Ranged(_ctx, this); }
    public AttackStatebase RangedCharged() { return new RangedCharged(_ctx, this); }
}
