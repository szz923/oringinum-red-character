//破坏计划
//抽牌堆捡1
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Combat;
using System.IO;
using MegaCrit.Sts2.Core.Localization;

namespace Wisadel.Scripts;

[RegisterPower]
public class SabotagePower : ModPowerTemplate
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override PowerAssetProfile AssetProfile => 
    new(
        IconPath: "res://wisadel/images/powers/test_power.png",
        BigIconPath: "res://wisadel/images/powers/test_power.png"
       );
	public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
	{
		if (player == base.Owner.Player)
		{
			await CardPileCmd.ShuffleIfNecessary(choiceContext, base.Owner.Player);

			var drawPileCards = PileType.Draw.GetPile(base.Owner.Player).Cards
								.OrderBy(c => c.Rarity)
								.ThenBy(c => c.Id)
								.ToList();

			LocString prompt = new LocString("", "选择一张牌");
			var selectedCards = await CardSelectCmd.FromSimpleGrid(
				choiceContext,
				drawPileCards,
				base.Owner.Player,
				new CardSelectorPrefs(prompt, base.Amount)
			);
			foreach (var card in selectedCards)
			{
				await CardPileCmd.Add(card, PileType.Hand);
			}
		}
	}
}