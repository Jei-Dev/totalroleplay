using Dalamud.Interface.Windowing;
using Dalamud.Logging;
using ImGuiNET;
using System;
using System.Numerics;
using totalRoleplay.Service;

namespace totalRoleplay.Windows;

public class MainWindow : Window, IDisposable
{
	public MainWindow() : base("Total Roleplay")
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
		var contextActive = IAmGod.pluginConfiguration.gameInteractionContextMenu;
		ImGui.Text("Context menu interaction is " + (contextActive ? "enabled" : "disabled"));

		if (ImGui.Button("Show Settings"))
		{
			IAmGod.plugin.DrawConfigUI();
		}

		ImGui.Spacing();

		ImGui.Text("Woah! Progress!");
		ImGui.Indent(55);
		ImGui.Unindent(55);
	}
}
