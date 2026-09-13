using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Scaffolding.Content;
using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.HoverTips;

namespace Wisadel.Scripts;

[RegisterCard(typeof(WRedCardPool))]
public sealed class GrapeBomb : ModCardTemplate
{
	protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromCard<Grapes>(false)];
	
	//卡牌属性：固有/保留等等
	public override IEnumerable<CardKeyword> CanonicalKeywords => 
	[
		CardKeyword.Exhaust
	];
	public override CardAssetProfile AssetProfile => new
	(
		PortraitPath: $"res://wisadel/images/cards/{GetType().Name}.png"
	);
	protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
	{
		new DamageVar(17, ValueProp.Move),
	};
	private const CardType type = CardType.Attack;
	private const CardRarity rarity = CardRarity.Rare;
	private const TargetType targetType = TargetType.AllEnemies;
	private const bool shouldShowInCardLibrary = true;
	public GrapeBomb() : base(2, type, rarity, targetType, shouldShowInCardLibrary)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await DamageCmd
		.Attack(base.DynamicVars.Damage.BaseValue)
		.FromCard(this, cardPlay)
		.TargetingAllOpponents(base.CombatState)
		.Execute(choiceContext);
		int num = 10 - CardPile.GetCards(base.Owner, PileType.Hand).Count();
		List<CardModel> list = new List<CardModel>();
		for (int i = 0; i < num; i++)
		{
			list.Add(base.CombatState.CreateCard<Grapes>(base.Owner));
		}
		await CardPileCmd.AddGeneratedCardsToCombat(list, PileType.Hand, this.Owner);
	}
	protected override void OnUpgrade()
	{
		DynamicVars.Damage.UpgradeValueBy(4);
	}
}
