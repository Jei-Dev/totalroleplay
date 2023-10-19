using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using ImGuiNET;
using System;
using System.Numerics;
using totalRoleplay.Configuration;

namespace totalRoleplay.Windows;

public class ConfigWindow : Window, IDisposable
{
	private readonly PluginConfiguration pluginConfiguration;
	private readonly IPluginLog log;

	public ConfigWindow(PluginConfiguration pluginConfiguration, IPluginLog log) : base("Total Roleplay Configuration")
	{
		Flags = ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse;
		this.Size = new Vector2(500, 100);
		this.SizeCondition = ImGuiCond.Always;
		this.pluginConfiguration = pluginConfiguration;
		this.log = log;
	}

	public void Dispose() { }
	public override void Draw()
	{
		// can't ref a property, so use a local copy
		var configValue = pluginConfiguration.gameInteractionContextMenu;
		if (ImGui.Checkbox("Enable Context Menu buttons", ref configValue))
		{
			pluginConfiguration.gameInteractionContextMenu = configValue;
		}

		var textDrawSpeed = pluginConfiguration.dialogueDrawSpeed;
		if (ImGui.SliderFloat("Dialogue Speed", ref textDrawSpeed, 0.03f, 0.1f))
		{
			pluginConfiguration.dialogueDrawSpeed = textDrawSpeed;
		}

		ImGui.SetCursorPos(new Vector2(195, 60));
		if (ImGui.Button("Save", new Vector2(50, 50)))
		{
			pluginConfiguration.Save();
			log.Debug("Save button hit");
		}
	}
}
