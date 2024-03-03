using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using ImGuiNET;
using System.Numerics;

namespace chroniclePlugin.Windows.characterinteraction.firstimpression;
public class ImpressionWindow : Window
{
	//	private readonly IPluginLog log;
	public ImpressionWindow(IPluginLog log) : base("First Impressions")
	{
		//		this.log = log;
		log.Verbose("Loaded ImpressionWindow");

		Flags = ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse | ImGuiWindowFlags.NoTitleBar;
		this.Size = new Vector2(20, 50);
		this.SizeCondition = ImGuiCond.Always;
	}

	public override void Draw()
	{

	}
}
