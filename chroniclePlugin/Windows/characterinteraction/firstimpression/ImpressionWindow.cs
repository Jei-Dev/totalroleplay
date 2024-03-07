using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using ImGuiNET;
using System.Numerics;

namespace chroniclePlugin.Windows.characterinteraction.firstimpression;
public class ImpressionWindow : Window
{
	private readonly IPluginLog log;
	private int i = 1;
	public ImpressionWindow(IPluginLog _log) : base("First Impressions")
	{
		log = _log;
		log.Verbose("Loaded ImpressionWindow");

		Flags = ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse | ImGuiWindowFlags.NoTitleBar;
		this.Size = new Vector2(210, 60);
		this.SizeCondition = ImGuiCond.Always;
	}

	public override void Draw()
	{
		while (i == 1)
		{
			log.Verbose("Rendering FirstImpression UI");
			i--;
		}

		ImGui.SetCursorPos(new Vector2(10, 10));
		ImGui.BeginChildFrame(31, new Vector2(40, 40));
		if (ImGui.IsItemHovered())
		{
			ImGui.BeginTooltip();
			ImGui.Text("Looking Golden! :D");
			ImGui.EndTooltip();
		}
		ImGui.Image(new nint(0), new Vector2(40, 40)); // For current purposes we are just using 'nint 0' as a place holder for now.
		ImGui.Text("Test");
		ImGui.EndChildFrame();

		ImGui.SetCursorPos(new Vector2(60, 10));
		ImGui.BeginChildFrame(32, new Vector2(40, 40));
		ImGui.Text("Test 1");
		ImGui.EndChildFrame();

		ImGui.SetCursorPos(new Vector2(110, 10));
		ImGui.BeginChildFrame(33, new Vector2(40, 40));
		ImGui.Text("Test 2");
		ImGui.EndChildFrame();

		ImGui.SetCursorPos(new Vector2(160, 10));
		ImGui.BeginChildFrame(34, new Vector2(40, 40));
		ImGui.Text("Test 3");
		ImGui.EndChildFrame();
	}
}
