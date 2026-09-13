using MegaCrit.Sts2.Core.Entities.Cards;
using STS2RitsuLib.Scaffolding.Content;
using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Wisadel.Scripts;

[RegisterCard(typeof(WRedCardPool))]
public sealed class BombDevil: ModCardTemplate
{
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => 
	[
		HoverTipFactory.FromCard<TheBomb>(false),	
		HoverTipFactory.FromPower<TheBombPower>()
	];
    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
	{
		new HpLossVar(3m),

	};
    public override CardAssetProfile AssetProfile => 
    new 
    (
	PortraitPath: $"res://wisadel/images/cards/{GetType().Name}.png"
	);
    public BombDevil() : base(2, CardType.Power, CardRarity.Ancient, TargetType.Self)
    {
    }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
    
    }
}