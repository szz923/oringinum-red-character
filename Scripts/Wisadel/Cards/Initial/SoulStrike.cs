//魂灵打击
//造成6（9）点伤害。
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
[RegisterCharacterStarterCard(typeof(WisadelCharacter), 4)]
public class SoulStrike : ModCardTemplate
{
	protected override HashSet<CardTag> CanonicalTags => new HashSet<CardTag> { CardTag.Strike };
	private const int energyCost = 1;
	private const CardType type = CardType.Attack;
	private const CardRarity rarity = CardRarity.Basic;
	private const TargetType targetType = TargetType.AnyEnemy;
	private const bool shouldShowInCardLibrary = true;
	public SoulStrike() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
	{
	}
	// 卡图资源
	public override CardAssetProfile AssetProfile => new(
		PortraitPath: $"res://wisadel/images/cards/{GetType().Name}.png"
	);
	// 基础伤害6，升级后+3变成9
	protected override IEnumerable<DynamicVar> CanonicalVars => [
		new DamageVar(6, ValueProp.Move)
	];
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		var target = cardPlay.Target!;
		// 造成主伤害
		await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
		.FromCard(this, cardPlay)
		.Targeting(target)
		.Execute(choiceContext);
	}

	protected override void OnUpgrade()
	{
		DynamicVars.Damage.UpgradeValueBy(3);
	}
}
