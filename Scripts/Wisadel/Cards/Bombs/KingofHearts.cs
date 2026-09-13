//红桃k
//造成7点伤害，引爆炸弹与地雷
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
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Scaffolding.Characters;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Cards;

namespace Wisadel.Scripts;

[RegisterCard(typeof(WRedCardPool))]
public sealed class HeartsK : ModCardTemplate
{
	protected override IEnumerable<IHoverTip> AdditionalHoverTips => 
	[
		HoverTipFactory.FromCard<TheBomb>(false),		
		HoverTipFactory.FromPower<TheBombPower>()
	];
	public override IEnumerable<CardKeyword> CanonicalKeywords => 
	[
		CardKeyword.Retain//保留
	];
	public override CardAssetProfile AssetProfile => new
	(
		PortraitPath: $"res://wisadel/images/cards/{GetType().Name}.png"
	);
	protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
	{
		new DamageVar(15, ValueProp.Move)
	};
	private const CardType type = CardType.Attack;
	private const CardRarity rarity = CardRarity.Rare;
	private const TargetType targetType = TargetType.AnyEnemy;
	private const bool shouldShowInCardLibrary = true;
	public HeartsK() : base(1, type, rarity, targetType, shouldShowInCardLibrary)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		var target = cardPlay.Target!;
		ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
		NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(NFireSmokePuffVfx.Create(target));
		var bombPowers = base.Owner.Creature.GetPowerInstances<TheBombPower>();
		await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
			.FromCard(this, cardPlay)
			.TargetingAllOpponents(base.CombatState)
			.Execute(choiceContext);
		if (bombPowers != null && bombPowers.Any())
		{
			foreach (var bombPower in bombPowers)
			{
				bombPower.SetAmount(1);
			}
		}

	}
	protected override void OnUpgrade()
	{
		DynamicVars.Damage.UpgradeValueBy(7);
	}
}
