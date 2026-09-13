using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Wisadel.Scripts;

[RegisterCard(typeof(WRedCardPool))]
public class MercenaryTorment : ModCardTemplate
{
	protected override IEnumerable<IHoverTip> AdditionalHoverTips => 
	[		
		HoverTipFactory.FromPower<DebilitatePower>(),
		HoverTipFactory.FromPower<VulnerablePower>(),
		HoverTipFactory.FromPower<WeakPower>()
	];
	public override IEnumerable<CardKeyword> CanonicalKeywords =>
	[
		CardKeyword.Exhaust
	];
	private const int energyCost = 1;
	private const CardType type = CardType.Skill;
	private const CardRarity rarity = CardRarity.Common;
	private const TargetType targetType = TargetType.AnyEnemy;
	private const bool shouldShowInCardLibrary = true;
	public override CardAssetProfile AssetProfile => new(
		PortraitPath: $"res://wisadel/images/cards/{GetType().Name}.png"
	);
    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
	{
        new DynamicVar("Power", 1m)
	};
	public MercenaryTorment () : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
        int amount = base.DynamicVars["Power"].IntValue;
        await PowerCmd.Apply<DebilitatePower>(choiceContext, cardPlay.Target, amount, Owner.Creature, this, false);
		await PowerCmd.Apply<VulnerablePower>(choiceContext, cardPlay.Target, amount, Owner.Creature, this, false);
        await PowerCmd.Apply<WeakPower>(choiceContext, cardPlay.Target, amount, Owner.Creature, this, false);
	}
	protected override void OnUpgrade()
	{
		base.EnergyCost.UpgradeBy(-1);
		RemoveKeyword(CardKeyword.Exhaust);
	}
}