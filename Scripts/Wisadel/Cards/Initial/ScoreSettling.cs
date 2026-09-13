// 定点清算
// 造成6点伤害。本回合每打出过一张攻击牌，其费用-1。
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;
using STS2RitsuLib.Scaffolding.Content;

namespace Wisadel.Scripts;

[RegisterCard(typeof(WRedCardPool))]
[RegisterCharacterStarterCard(typeof(WisadelCharacter), 1)]
public class ScoreSettling : ModCardTemplate
{
	private const int energyCost = 3;
	private const CardType type = CardType.Attack;
	private const CardRarity rarity = CardRarity.Basic;
	private const TargetType targetType = TargetType.AnyEnemy;
	private const bool shouldShowInCardLibrary = true;
	public override CardAssetProfile AssetProfile => new(
		PortraitPath: $"res://wisadel/images/cards/{GetType().Name}.png"
	);
	protected override IEnumerable<DynamicVar> CanonicalVars => [
		new DamageVar(4, ValueProp.Move),
		new RepeatVar(3)
	];

	public ScoreSettling() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		var target = cardPlay.Target!;
		//造成主伤害
         await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this, cardPlay)
            .Targeting(target)
            .WithHitCount(DynamicVars.Repeat.IntValue)
            .Execute(choiceContext);
	}

	protected override void OnUpgrade()
	{
		DynamicVars.Damage.UpgradeValueBy(1);
		base.EnergyCost.UpgradeBy(-1);
	}

	//每打出一张攻击牌，本回合费用 -1
	public override Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
	{
		if (cardPlay.Card.Owner != base.Owner)
			return Task.CompletedTask;
		if (cardPlay.Card.Type != CardType.Attack)
			return Task.CompletedTask;

		base.EnergyCost.AddThisTurn(-1);
		return Task.CompletedTask;
	}
}
