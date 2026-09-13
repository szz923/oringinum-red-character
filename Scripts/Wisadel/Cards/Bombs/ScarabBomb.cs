using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Scaffolding.Content;
using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Models.Powers;
using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.HoverTips;

namespace Wisadel.Scripts;

[RegisterCard(typeof(WRedCardPool))]
public sealed class ScarabBomb : ModCardTemplate
{
	protected override IEnumerable<IHoverTip> AdditionalHoverTips => 
	[		
		HoverTipFactory.Static(StaticHoverTip.Block),
		HoverTipFactory.FromPower<VulnerablePower>()
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
		new DamageVar(12, ValueProp.Move),
        new DynamicVar("Power", 2m)
	};
	private const CardType type = CardType.Attack;
	private const CardRarity rarity = CardRarity.Uncommon;
	private const TargetType targetType = TargetType.AnyEnemy;
	private const bool shouldShowInCardLibrary = true;
	public ScarabBomb() : base(1, type, rarity, targetType, shouldShowInCardLibrary)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
		VfxCmd.PlayOnCreatureCenter(base.Owner.Creature, "vfx/vfx_flying_slash");
		int amount = base.DynamicVars["Power"].IntValue;
		await CreatureCmd.LoseBlock(choiceContext, cardPlay.Target, 999, null);
		await PowerCmd.Apply<VulnerablePower>(choiceContext, cardPlay.Target, amount, Owner.Creature, this, false);
		var target = cardPlay.Target!;
		await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
		.FromCard(this, cardPlay)
		.Targeting(target)
		.Execute(choiceContext);
	}
	protected override void OnUpgrade()
	{
		base.DynamicVars["Power"].UpgradeValueBy(1m);
	}
}
