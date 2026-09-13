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
public class Joker : ModCardTemplate
{
	protected override IEnumerable<IHoverTip> AdditionalHoverTips => 
	[		
		HoverTipFactory.FromPower<JokerBoxPower>()
	];
	public override IEnumerable<CardKeyword> CanonicalKeywords =>
	[
		CardKeyword.Retain,
		CardKeyword.Exhaust
	];
	private const int energyCost = 2;
	private const CardType type = CardType.Skill;
	private const CardRarity rarity = CardRarity.Rare;
	private const TargetType targetType = TargetType.Self;
	private const bool shouldShowInCardLibrary = true;
	public override CardAssetProfile AssetProfile => new(
		PortraitPath: $"res://wisadel/images/cards/{GetType().Name}.png"
	);
	public Joker() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		for (int i = 0; i < Random.Shared.Next(0, 6); i++)
		{
			await PowerCmd.Apply<JokerBoxPower>(choiceContext,  base.Owner.Creature, Random.Shared.Next(1, 5), base.Owner.Creature, this, false);    
		}
	}
	protected override void OnUpgrade()
	{
		base.EnergyCost.UpgradeBy(-1);
	}
}