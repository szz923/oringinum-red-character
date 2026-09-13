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
public class MimicVictim : ModCardTemplate
{
	protected override IEnumerable<IHoverTip> AdditionalHoverTips => 
	[		
		HoverTipFactory.FromPower<ThornsPower>()
	];
	public override IEnumerable<CardKeyword> CanonicalKeywords =>
	[
		CardKeyword.Exhaust
	];
	protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
	{
		new GoldVar(50),
        new DynamicVar("Power", 1m)
	};
	private const int energyCost = 0;
	private const CardType type = CardType.Skill;
	private const CardRarity rarity = CardRarity.Uncommon;
	private const TargetType targetType = TargetType.AnyEnemy;
	private const bool shouldShowInCardLibrary = true;
	public override CardAssetProfile AssetProfile => 
	new
	(
	    PortraitPath: $"res://wisadel/images/cards/{GetType().Name}.png"
	);
	public MimicVictim() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		int amount = base.DynamicVars["Power"].IntValue;
		await PlayerCmd.GainGold(base.DynamicVars["Gold"].IntValue, base.Owner);
        await PowerCmd.Apply<ThornsPower>(choiceContext, cardPlay.Target, amount, base.Owner.Creature, this, false);
	}
	protected override void OnUpgrade()
	{
		base.DynamicVars.Gold.UpgradeValueBy(150);
		base.DynamicVars["Power"].UpgradeValueBy(2m);
	}
}