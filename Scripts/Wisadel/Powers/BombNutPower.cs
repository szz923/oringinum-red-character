//自爆坚果：受到被格挡的伤害获得1炸弹
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

namespace Wisadel.Scripts;

[RegisterPower]
public class BombNutPower : ModPowerTemplate
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override PowerAssetProfile AssetProfile => 
    new(
        IconPath: "res://wisadel/images/powers/BombNut.png",
        BigIconPath: "res://wisadel/images/powers/BombNutBig.png"
       );
    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
	{
		if (target == base.Owner && result.BlockedDamage > 0 && props.IsPoweredAttack() && dealer != null)
		{
			await PowerCmd.Apply<TheBombPower>(choiceContext,  base.Owner, 3, base.Owner, null, false);
		}
	}
    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
	{
		if (player == this.Owner.Player)
		{
			await PowerCmd.Decrement(this);
		}
	}
}