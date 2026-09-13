using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Scaffolding.Content;
using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Entities.Creatures;
using System.Linq;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace Wisadel.Scripts;

[RegisterCard(typeof(WRedCardPool))]
public sealed class Arsonist : ModCardTemplate
{
    public override CardAssetProfile AssetProfile => new
    (
        PortraitPath: $"res://wisadel/images/cards/{GetType().Name}.png"
    );

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new DamageVar(13, ValueProp.Move),
    };

    private const CardType type = CardType.Attack;
    private const CardRarity rarity = CardRarity.Rare;
    private const TargetType targetType = TargetType.AllEnemies;
    private const bool shouldShowInCardLibrary = true;

    public Arsonist() : base(1, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 2. 先快照每个敌人当前的负面效果，避免传播过程中互相污染
        var snapshots = new List<(Creature Source, Dictionary<PowerModel, int> Debuffs)>();

        foreach (Creature source in base.CombatState.HittableEnemies)
        {
            Dictionary<PowerModel, int> debuffAmounts =
                (from p in source.Powers
                 where p.TypeForCurrentAmount == PowerType.Debuff
                 select ((PowerModel)p.ClonePreservingMutability(), Amount: p.Amount))
                .ToDictionary();

            // 处理 ITemporaryPower：把临时能力的层数合并到内部实际生效的 Power 上
            foreach (KeyValuePair<PowerModel, int> item in debuffAmounts.ToList())
            {
                if (item.Key is ITemporaryPower temporaryPower)
                {
                    KeyValuePair<PowerModel, int> keyValuePair =
                        debuffAmounts.FirstOrDefault(p =>
                            p.Key.Id == temporaryPower.InternallyAppliedPower.Id);

                    if (keyValuePair.Key != null)
                    {
                        debuffAmounts[keyValuePair.Key] += item.Value;
                    }
                }
            }

            snapshots.Add((source, debuffAmounts));
        }

        // 3. 统一传播：每个敌人都把快照中的负面效果给其他所有敌人
        foreach (var (source, debuffs) in snapshots)
        {
            foreach (Creature target in base.CombatState.HittableEnemies)
            {
                if (target == source)
                {
                    continue;
                }

                foreach (KeyValuePair<PowerModel, int> item in debuffs)
                {
                    if (item.Value == 0)
                    {
                        continue;
                    }

                    PowerModel existing = PowerCmd.FindExistingInstanceForStacking(item.Key, target, item.Key.Applier);

                    if (existing != null)
                    {
                        await PowerCmd.ModifyAmount(choiceContext, existing, item.Value, item.Key.Applier, this);
                        continue;
                    }
                    PowerModel power = (PowerModel)item.Key.ClonePreservingMutability();
                    await PowerCmd.Apply(choiceContext, power, target, item.Value, item.Key.Applier, this);
                }
            }
        }
        // 1. 对全体造成伤害
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
        .FromCard(this, cardPlay)
        .TargetingAllOpponents(base.CombatState)
        .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);
    }
}