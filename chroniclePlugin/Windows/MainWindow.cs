using Dalamud.Interface.Windowing;
using Dalamud.Logging;
using ImGuiNET;
using System;
using System.Numerics;
using chroniclePlugin.Configuration;
using chroniclePlugin.Service;

namespace chroniclePlugin.Windows;

public class MainWindow : Window, IDisposable
{
	private readonly Plugin plugin;
	private readonly PluginConfiguration pluginConfiguration;

	public MainWindow(Plugin plugin, PluginConfiguration pluginConfiguration) : base("Total Roleplay")
	{
		Flags = ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse;
		this.SizeConstraints = new WindowSizeConstraints
		{
			MinimumSize = new Vector2(375, 330),
			MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
		};
		this.plugin = plugin;
		this.pluginConfiguration = pluginConfiguration;
	}

	public void Dispose() { }

	public override void Draw()
	{
		var contextActive = pluginConfiguration.gameInteractionContextMenu;
		ImGui.Text("Context menu interaction is " + (contextActive ? "enabled" : "disabled"));

		if (ImGui.Button("Show Settings"))
		{
			plugin.DrawConfigUI();
		}

		ImGui.Spacing();

		ImGui.Text("Woah! Progress!");
		ImGui.Indent(55);
		ImGui.Unindent(55);
	}
}
