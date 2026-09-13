using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Wisadel.Scripts;

[RegisterCard(typeof(WRedCardPool))]
public class MercenaryInstinct : ModCardTemplate
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => 
    [
		CardKeyword.Exhaust
	];
	private const int energyCost = 3;
	private const CardType type = CardType.Skill;
	private const CardRarity rarity = CardRarity.Rare;
	private const TargetType targetType = TargetType.AnyEnemy;
	private const bool shouldShowInCardLibrary = true;
	public override CardAssetProfile AssetProfile => new(
		PortraitPath: $"res://wisadel/images/cards/{GetType().Name}.png"
	);
    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
	{
	};
	public MercenaryInstinct () : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target.Monster.IntendsToAttack)
        {
            int num = 10 - base.Owner.PlayerCombatState.Hand.Cards.Count;
		    await CardPileCmd.Draw(choiceContext, num, base.Owner);
            foreach (CardModel card in PileType.Hand.GetPile(base.Owner).Cards)
            {
                if (!card.EnergyCost.CostsX)
                {
                    card.SetToFreeThisTurn();
                }
            };
        }
    }
	protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }
}