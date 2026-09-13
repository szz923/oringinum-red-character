using Godot.Bridge;
using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models.Cards;
using STS2RitsuLib;
using STS2RitsuLib.Interop;
using System.Reflection;
using STS2RitsuLib.Audio;

namespace Wisadel.Scripts;

[ModInitializer(nameof(Init))]
public class Entry
{
    public const string ModId = "wisadel";

    public static void Init()
    {
        //必须保留：让 tscn可以加载自定义脚本
        ScriptManagerBridge.LookupScriptsInAssembly(typeof(Entry).Assembly);

        //Harmony Patch（暂时不需要也可以保留）
        var harmony = new Harmony("sts2.originum.wisadel");
        harmony.PatchAll();

        //新增：注册你的内容（卡牌、角色等）
        var assembly = Assembly.GetExecutingAssembly();
        RitsuLibFramework.EnsureGodotScriptsRegistered(assembly, null);
        //欧洛巴斯
        RitsuLibFramework.RegisterArchaicToothTranscendenceMapping<MercenaryPlunder, MercenaryPlunder>();
        RitsuLibFramework.RegisterTouchOfOrobasRefinementMapping<ThrowerMedal, AncestorLauncher>();
        RitsuLibFramework
            .CreateContentPack(ModId)
            .Character<WisadelCharacter>()
            .Relic<WRedRelicPool, ThrowerMedal>()
            .Card<WRedCardPool, MercenaryPlunder>()
            .Card<WRedCardPool, ScoreSettling>()
            .Card<WRedCardPool, SoulDefence>()
            .Card<WRedCardPool, SoulStrike>()
            .Apply();
        FmodStudioDeferredBankRegistration.RegisterBank("res://wisadel/audios/SFX.bank");
        FmodStudioDeferredBankRegistration.RegisterStudioGuidMappings("res://wisadel/audios/GUIDs.txt");
        //注册模组程序集
        ModTypeDiscoveryHub.RegisterModAssembly(ModId, assembly);

        Log.Info("Wisadel mod initialized!");
    }
}
//.Card<WRedCardPool, NewCardName>()