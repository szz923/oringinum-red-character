using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Scaffolding.Content;
using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace Wisadel.Scripts;

[RegisterCard(typeof(WRedCardPool))]
public sealed class CherryBomb : ModCardTemplate
{
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
		new DamageVar(8, ValueProp.Move),
        new RepeatVar(2)
	};
	private const CardType type = CardType.Attack;
	private const CardRarity rarity = CardRarity.Uncommon;
	private const TargetType targetType = TargetType.AllEnemies;
	private const bool shouldShowInCardLibrary = true;
	public CherryBomb() : base(2, type, rarity, targetType, shouldShowInCardLibrary)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		var target = cardPlay.Target!;
		await DamageCmd
        .Attack(base.DynamicVars.Damage.BaseValue)
        .WithHitCount(DynamicVars.Repeat.IntValue)
        .FromCard(this, cardPlay)
        .TargetingAllOpponents(base.CombatState)
		.Execute(choiceContext);
	}
	protected override void OnUpgrade()
	{
		DynamicVars.Damage.UpgradeValueBy(4);
	}
}