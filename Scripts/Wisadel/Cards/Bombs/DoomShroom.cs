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
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Entities.Creatures;
using System.Linq;

namespace Wisadel.Scripts;

[RegisterCard(typeof(WRedCardPool))]
public sealed class DoomShroom : ModCardTemplate
{
	protected override IEnumerable<IHoverTip> AdditionalHoverTips => 
	[		
		HoverTipFactory.FromPower<DoomPower>()
	];
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
		new DamageVar(36, ValueProp.Move),
	};
	private const CardType type = CardType.Attack;
	private const CardRarity rarity = CardRarity.Rare;
	private const TargetType targetType = TargetType.AllEnemies;
	private const bool shouldShowInCardLibrary = true;
	public DoomShroom() : base(3, type, rarity, targetType, shouldShowInCardLibrary)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		AttackCommand attackCommand = await DamageCmd
        .Attack(base.DynamicVars.Damage.BaseValue)
        .FromCard(this, cardPlay)
        .TargetingAllOpponents(base.CombatState)
		.Execute(choiceContext);
		int totalDamage = attackCommand.Results
        .Sum(list => list.Sum(damageResult => damageResult.TotalDamage));
        //这里其实是对所有人造成的总伤，但是卡牌描述的确可以如此理解
		if (totalDamage > 0 && base.Owner?.Creature != null)
		{
			foreach (var enemy in base.CombatState.HittableEnemies)
			{
				await PowerCmd.Apply<DoomPower>(choiceContext, enemy, totalDamage, base.Owner.Creature, this);
			}
		}
	}
	protected override void OnUpgrade()
	{
		DynamicVars.Damage.UpgradeValueBy(9);
	}
}