using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Wisadel.Scripts;

[RegisterCard(typeof(WRedCardPool))]
public sealed class BombNut : ModCardTemplate
{
	public override IEnumerable<CardKeyword> CanonicalKeywords => 
	[
		CardKeyword.Exhaust
	];
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => 
	[
		HoverTipFactory.FromCard<TheBomb>(false),	
		HoverTipFactory.FromPower<BombNutPower>(),	
		HoverTipFactory.FromPower<TheBombPower>()
	];
	public override bool GainsBlock => true;
	protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
	{
		new BlockVar(16, ValueProp.Move)
	};
	public BombNut() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
	{
	}
	public override CardAssetProfile AssetProfile => 
	new
	(
		PortraitPath: $"res://wisadel/images/cards/{GetType().Name}.png"
	);
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay);
        await PowerCmd.Apply<BombNutPower>(choiceContext,  base.Owner.Creature, 1, base.Owner.Creature, this, false);
	}
	protected override void OnUpgrade()
	{
		base.DynamicVars.Block.UpgradeValueBy(4);
	}
}