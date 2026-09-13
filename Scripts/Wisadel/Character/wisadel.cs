using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Characters;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Animation;

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.PotionPools;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace Wisadel.Scripts;

[RegisterCharacter]

public class WisadelCharacter : ModCharacterTemplate<WRedCardPool, WRedRelicPool, WRedPotionPool>
{
	// 基础属性
	public override Color NameColor => new Color("9E0D22");
	public override Color EnergyLabelOutlineColor => new Color("802020");
	public override Color MapDrawingColor => new Color("9E0D22");
	//角色性别
	public override CharacterGender Gender => CharacterGender.Masculine;
	//初始血量金币
	public override int StartingHp => 55;
	public override int StartingGold => 150;
	//动画间隔
	public override float AttackAnimDelay => 0.15f;
	public override float CastAnimDelay => 1f;
	//是否需要时间线
	public override bool RequiresEpochAndTimeline => false;
	public override CharacterAssetProfile AssetProfile => CharacterAssetProfiles.Merge
	(
		CharacterAssetProfiles.Ironclad(),
		new
		(
			Scenes: new
			(
				VisualsPath: "res://wisadel/scenes/wisadel_character.tscn",
				EnergyCounterPath: "res://wisadel/scenes/wisadel_energy_counter.tscn"
			),
			Ui: new
			(
				CharacterSelectBgPath: "res://wisadel/scenes/wisadel_bg.tscn"
			),
			Audio: new
			(
				// AttackSfx: null,
				// CastSfx: null,
				// DeathSfx: null,
				CharacterSelectSfx: "event:/beselected"
				// CharacterTransitionSfx: "event:/sfx/ui/wipe_ironclad"
			)
		)
	);
	protected override CreatureAnimator? SetupCustomCreatureAnimator(MegaSprite controller)
	{
		// 定义动画状态
		AnimState idle = new("Idle", true);
		AnimState attack = new("Attack_A") { NextState = idle };
		AnimState cast = new("Attack_C") { NextState = idle };
		AnimState hurt = new("Die") { NextState = idle };
		AnimState die = new("Stun");
		AnimState deadLoop = new("Stun", true);

		// 设置分支
		idle.AddBranch("Hit", hurt);
		attack.AddBranch("Hit", hurt);
		hurt.AddBranch("Hit", hurt);

		// 死亡后进入死亡循环
		die.NextState = deadLoop;

		// 创建动画器
		CreatureAnimator animator = new(idle, controller);

		// 添加任意状态转换
		animator.AddAnyState("Attack", attack);
		animator.AddAnyState("Cast", attack);
		animator.AddAnyState("Dead", die);

		return animator;
	}
	//攻击建筑师的特效
	public override List<string> GetArchitectAttackVfx() => new()
	{
		"vfx/vfx_attack_blunt",
		"vfx/vfx_heavy_blunt",
		"vfx/vfx_attack_slash",
		"vfx/vfx_bloody_impact",
        "vfx/vfx_rock_shatter"
	};
}
//截止目前
//攻击牌：32张
/*
死魂灵打击
定点清算
饱和复仇
爆裂黎明
曲射
试射
摧毁掩体
清空弹夹
拾刀者
背叛者（联机）
纵火者

佣兵的死斗
佣兵的重锋
佣兵的贪婪
佣兵的劫掠
佣兵的追杀

死魂灵的法骸
死魂灵的残迹
死魂灵的迸发

C4
D12
红桃k
Joker
此面向敌
惊喜盒子
黑寡妇
烟雾弹
弹力手榴弹
樱桃炸弹
爆裂葡萄
土豆地雷
原始土豆地雷
毁灭菇
即兴制作
*/
//技能牌  30张
/*
死魂灵防御
欺诈
维什戴尔
临时防线
爆炸坚果
逃脱树根
生于黑夜
暗夜无明
独影归途
箱下亡魂
慈悲愿景
神出鬼没
伺机而动
索然无味
S.B.C
快速装填

佣兵的补给
佣兵的机械制品
佣兵的本能
佣兵的针线
佣兵的嘲弄
佣兵的恍惚

死魂灵的薪柴
死魂灵的涡流
死魂灵的驰援（联机）
死魂灵的哀嚎
死魂灵的圣杯
死魂灵的远去
死魂灵的激情
死魂灵的自焚
死魂灵的守成
死魂灵的棱镜
死魂灵的纯粹
死魂灵的喋喋不休
*/
//能力牌  20张
/*
佣兵的疤痕
佣兵的军火库
佣兵的残忍
佣兵的斩草除根

卡兹戴尔
罗德戴尔
破坏计划
落井下石
魂灵形态

聚焦情感
死魂灵的无界
死魂灵的好礼
死魂灵的余息
死魂灵的仪式
死魂灵的激荡

炸弹恶魔
乱炸无歇
雷区
提前设伏
*/
