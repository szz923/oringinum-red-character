using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Models;
using Godot;
using STS2RitsuLib.Scaffolding.Content;

namespace Wisadel.Scripts;

[RegisterRelic(typeof(WRedRelicPool))]
[RegisterCharacterStarterRelic(typeof(WisadelCharacter))] 
public sealed class ThrowerMedal:ModRelicTemplate
{
	public override RelicRarity Rarity => RelicRarity.Starter;
	public override bool ShowCounter => false;
	protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
	{
		new DamageVar(1, ValueProp.Move)
	};
	public override RelicAssetProfile AssetProfile => new(
		IconPath: $"res://wisadel/images/relics/thrower_medal.png",
		IconOutlinePath: $"res://wisadel/images/relics/thrower_medal.png",
		BigIconPath: $"res://wisadel/images/relics/thrower_medal_big.png"
	);
	public override async Task AfterAttack(PlayerChoiceContext choiceContext, AttackCommand command)
	{
		if (command.Attacker != base.Owner.Creature)
			return;
			
		if (command.ModelSource is not CardModel card || card.Type != CardType.Attack)
			return;

		int hitCount = command.Results.Count();
		if (hitCount <= 0)
			return;

		for (int i = 0; i < hitCount; i++)
		{
			await CreatureCmd.Damage(choiceContext, base.Owner.Creature.CombatState.HittableEnemies, base.DynamicVars.Damage, base.Owner.Creature);
		}
	}
}
