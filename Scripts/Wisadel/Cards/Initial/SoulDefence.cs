//魂灵防御
//获得5（8）点格挡。
using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Wisadel.Scripts;

[RegisterCard(typeof(WRedCardPool))]
[RegisterCharacterStarterCard(typeof(WisadelCharacter), 4)]
public sealed class SoulDefence : ModCardTemplate
{
	public override bool GainsBlock => true;
	protected override HashSet<CardTag> CanonicalTags => new HashSet<CardTag> { CardTag.Defend };
	protected override IEnumerable<DynamicVar> CanonicalVars => new[]
	{
		new BlockVar(5, ValueProp.Move)
	};
	public SoulDefence() : base(1, CardType.Skill, CardRarity.Basic, TargetType.Self)
	{
	}
	public override CardAssetProfile AssetProfile => new(
		PortraitPath: $"res://wisadel/images/cards/{GetType().Name}.png"
	);
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay);
	}
	protected override void OnUpgrade()
	{
		base.DynamicVars.Block.UpgradeValueBy(3);
	}
}
