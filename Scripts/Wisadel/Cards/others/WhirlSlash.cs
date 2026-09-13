// 定点清算
// 造成6点伤害。本回合每打出过一张攻击牌，其费用-1。
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;
using STS2RitsuLib.Scaffolding.Content;

namespace Wisadel.Scripts;

[RegisterCard(typeof(WRedCardPool))]
public class WhirlSlash : ModCardTemplate
{
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => 
	[
		HoverTipFactory.FromCard<Dazed>(false)
	];
	private const int energyCost = 1;
	private const CardType type = CardType.Attack;
	private const CardRarity rarity = CardRarity.Uncommon;
	private const TargetType targetType = TargetType.AnyEnemy;
	private const bool shouldShowInCardLibrary = true;
	public override CardAssetProfile AssetProfile => new(
		PortraitPath: $"res://wisadel/images/cards/{GetType().Name}.png"
	);
	protected override IEnumerable<DynamicVar> CanonicalVars => [
		new DamageVar(1, ValueProp.Move),
		new RepeatVar(3)
	];

	public WhirlSlash() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		var target = cardPlay.Target!;
		//造成主伤害
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this, cardPlay)
            .Targeting(target)
			.WithHitFx("vfx/vfx_dramatic_stab")
            .WithHitCount(DynamicVars.Repeat.IntValue)
            .Execute(choiceContext);
        List<CardModel> list = new List<CardModel>();
        list.Add(base.CombatState.CreateCard<Dazed>(base.Owner));
        await CardPileCmd.AddGeneratedCardsToCombat(list, PileType.Hand, this.Owner);
        CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(CombatState.CreateCard<Dazed>(Owner), PileType.Draw,Owner,CardPilePosition.Random));
        CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(CombatState.CreateCard<Dazed>(Owner), PileType.Discard,Owner,CardPilePosition.Random));
	}
    protected override CardLocation GetResultLocationForCardPlay()
    {
        var loc = base.GetResultLocationForCardPlay();
        if (loc.pileType == PileType.Discard)
        {
            loc.pileType = PileType.Hand;
            loc.position = CardPilePosition.Bottom;
        }
        return loc;
    }
	protected override void OnUpgrade()
	{
        base.EnergyCost.UpgradeBy(-1);
	}
}