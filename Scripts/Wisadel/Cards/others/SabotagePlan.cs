using MegaCrit.Sts2.Core.Entities.Cards;
using STS2RitsuLib.Scaffolding.Content;
using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Wisadel.Scripts;

[RegisterCard(typeof(WRedCardPool))]
public sealed class SabotagePlanNotes: ModCardTemplate
{
    public override CardAssetProfile AssetProfile => 
    new 
    (
	PortraitPath: $"res://wisadel/images/cards/{GetType().Name}.png"
	);
    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
	{
        new DynamicVar("Power", 2m),
        new CardsVar(2)
	};
    public SabotagePlanNotes() : base(1, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
    }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
        int amount = base.DynamicVars["Power"].IntValue;
        await PowerCmd.Apply<MachineLearningPower>(choiceContext,  base.Owner.Creature, -amount, base.Owner.Creature, this, false);
        await PowerCmd.Apply<SabotagePower>(choiceContext,  base.Owner.Creature, base.DynamicVars.Cards.BaseValue, base.Owner.Creature, this, false);
	}
    protected override void OnUpgrade()
	{
		base.DynamicVars["Power"].UpgradeValueBy(-1m);
	}
}