using Godot;
using STS2RitsuLib.Scaffolding.Content;
using STS2RitsuLib.Utils;

namespace Wisadel.Scripts;

public class WRedCardPool : TypeListCardPoolModel
{
	// 卡池的ID。必须唯一防撞车。
	public override string Title => "OriginumRed";
	public override string EnergyColorName => "RedOriginum";
	public override string? TextEnergyIconPath => "res://wisadel/images/energy_originum_red.png";
	public override string? BigEnergyIconPath => "res://wisadel/images/energy_originum_red_big.png";
	public override Color DeckEntryCardColor => new Color("9E0D22");
	public override Color EnergyOutlineColor => new Color("9E0D22");
	// 根据你使用的卡框决定使用哪个Material
   private static readonly Material? _poolFrameMaterial = (Material?)(object)MaterialUtils.CreateRgbShaderMaterial(158f/255f,13f/255f,34f/255f);
	public override Material? PoolFrameMaterial => _poolFrameMaterial;
	// 卡池是否是无色。例如事件、状态等卡池就是无色的。
	public override bool IsColorless => false;
}
