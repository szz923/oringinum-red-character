using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Scaffolding.Content;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Wisadel.Scripts;

[RegisterCard(typeof(WRedCardPool))]
[RegisterCharacterStarterCard(typeof(WisadelCharacter), 1)]
public sealed class MercenaryPlunder : ModCardTemplate
{
    //卡牌属性：固有/保留等等
    public override CardAssetProfile AssetProfile => new(
        PortraitPath: $"res://wisadel/images/cards/{GetType().Name}.png"
    );
	protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new GoldVar(15),
        new DamageVar(4, ValueProp.Move)
    };
	private const CardType type = CardType.Attack;
    private const CardRarity rarity = CardRarity.Basic;
    private const TargetType targetType = TargetType.AnyEnemy;
    private const bool shouldShowInCardLibrary = true;
    public MercenaryPlunder() : base(0, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
		await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
        .FromCard(this, cardPlay)
        .Targeting(cardPlay.Target)
        .WithHitFx("vfx/vfx_dramatic_stab", null, "blunt_attack.mp3")
		.Execute(choiceContext);
        await PlayerCmd.GainGold(base.DynamicVars["Gold"].IntValue, base.Owner);
	}
	protected override void OnUpgrade()
	{
		base.DynamicVars.Gold.UpgradeValueBy(15);
	}
}