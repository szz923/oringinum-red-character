using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Saves.Runs;  // ⬅️ 新增：用于 [SavedProperty]
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Scaffolding.Content;
using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;

namespace Wisadel.Scripts;

[RegisterCard(typeof(WRedCardPool))]
public sealed class BladeMercenary : ModCardTemplate
{
	public override IEnumerable<CardKeyword> CanonicalKeywords => 
	[
		CardKeyword.Exhaust
	];
    private const string _increaseKey = "Increase";
	private const int _baseHitCount = 3;
	private int CurrentHitCount = 3;
	private int IncreasedHitCount;
	[SavedProperty]
	public int CurrentRepeat
	{
		get
		{
			return CurrentHitCount;
		}
		set
		{
			AssertMutable();
			CurrentHitCount = value;
			base.DynamicVars.Repeat.BaseValue = CurrentHitCount;
		}
	}
	[SavedProperty]
	public int IncreasedRepeat
	{
		get
		{
			return IncreasedHitCount;
		}
		set
		{
			AssertMutable();
			IncreasedHitCount = value;
		}
	}
    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new DamageVar(3m, ValueProp.Move),
        new RepeatVar(CurrentHitCount),
        new IntVar("Increase",1)
    };
    private const CardType type = CardType.Attack;
    private const CardRarity rarity = CardRarity.Rare;
    private const TargetType targetType = TargetType.AnyEnemy;
    private const bool shouldShowInCardLibrary = true;

    public BladeMercenary() : base(2, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }

    public override CardAssetProfile AssetProfile => new(
        PortraitPath: $"res://wisadel/images/cards/{GetType().Name}.png"
    );

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        
        bool shouldTriggerFatal = cardPlay.Target.Powers.All((PowerModel p) => p.ShouldOwnerDeathTriggerFatal());

        AttackCommand attackCommand = await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .WithHitCount(DynamicVars.Repeat.IntValue)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);

        int intValue = base.DynamicVars["Increase"].IntValue;
        BuffFromPlay(intValue);
        (base.DeckVersion as BladeMercenary)?.BuffFromPlay(intValue);
        
	}

    protected override void OnUpgrade()
    {
        // 升级：基础伤害 +2
        DynamicVars.Damage.UpgradeValueBy(2m);
    }
    	protected override void AfterDowngraded()
	{
		UpdateRepeat();
	}
	private void BuffFromPlay(int extraDamage)
	{
		IncreasedHitCount += extraDamage;
		UpdateRepeat();
	}
	private void UpdateRepeat()
	{
		CurrentRepeat = _baseHitCount + IncreasedHitCount;
	}
}