using Dalamud.Interface.Windowing;
using Dalamud.Logging;
using ImGuiNET;
using System;
using System.Numerics;

namespace totalRoleplay.Windows;

public class TRPWindowMain : Window, IDisposable
{
	public TRPWindowMain() : base("Total Roleplay")
	{
		Flags = ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse;
		this.SizeConstraints = new WindowSizeConstraints
		{
			MinimumSize = new Vector2(375, 330),
			MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
		};
	}

	public void Dispose() { }

	public override void Draw()
	{
		var getBooleanProperty = Service.pluginConfig.BooleanProperty;
		ImGui.Text($"The random config bool is {getBooleanProperty}");

		if (ImGui.Button("Show Settings"))
		{
			Service.plugin.DrawConfigUI();
		}

		ImGui.Spacing();

		ImGui.Text("Woah! Progress!");
		ImGui.Indent(55);
		ImGui.Unindent(55);
	}
}
