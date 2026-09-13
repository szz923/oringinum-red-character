using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Scaffolding.Content;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Wisadel.Scripts;

[RegisterCard(typeof(WRedCardPool))]
public sealed class RelentlessRevenge : ModCardTemplate
{
    public override bool CanBeGeneratedInCombat => false;
    //条件满足，金色高亮
    protected override bool ShouldGlowGoldInternal => base.Owner.PlayerCombatState.Energy >= 4;
    protected override bool HasEnergyCostX => true;
    public override CardAssetProfile AssetProfile => new(
        PortraitPath: $"res://wisadel/images/cards/{GetType().Name}.png"
    );
    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new DamageVar(1, ValueProp.Move),
        new EnergyVar(4)
    };
    public RelentlessRevenge() : base(0, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies)
	{
	}
  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int xValue = ResolveEnergyXValue();
        int hitCount = 2 * xValue;
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
            .WithHitCount(hitCount)
            .FromCard(this, cardPlay)
            .TargetingAllOpponents(base.CombatState)
            .Execute(choiceContext);

        if (xValue > base.DynamicVars.Energy.IntValue)
        {
            int extraHits = hitCount * 2;
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
                .WithHitCount(extraHits)
                .FromCard(this, cardPlay)
                .TargetingRandomOpponents(base.CombatState)
                .Execute(choiceContext);
        }
    }
    protected override void OnUpgrade()
    {
      base.DynamicVars.Damage.UpgradeValueBy(2m);
    }
}