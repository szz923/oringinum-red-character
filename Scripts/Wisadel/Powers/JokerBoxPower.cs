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
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Wisadel.Scripts;

[RegisterPower]
public class JokerBoxPower : ModPowerTemplate
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
	public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;
	protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(31, ValueProp.Unpowered)];
    public override PowerAssetProfile AssetProfile => 
    new(
        IconPath: "res://wisadel/images/powers/JokerBox.png",
        BigIconPath: "res://wisadel/images/powers/JokerBoxBig.png"
       );
	public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (base.Amount > 1)
		{
			await PowerCmd.Decrement(this);
			return;
		}
		else
        {
            Flash();
			await Cmd.CustomScaledWait(0.2f, 0.4f);
			
			foreach (Creature hittableEnemy in base.CombatState.HittableEnemies)
			{
				NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(NFireSmokePuffVfx.Create(hittableEnemy));
			}
			await Cmd.CustomScaledWait(0.2f, 0.4f);
			await CreatureCmd.Damage(choiceContext, base.CombatState.HittableEnemies, base.DynamicVars.Damage, base.Owner);
			await PowerCmd.Remove(this);
        }
    }
}