using chroniclePlugin.Configuration;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using ImGuiNET;
using System;
using System.Numerics;

namespace chroniclePlugin.Windows;

public class ConfigWindow : Window, IDisposable
{
	private readonly PluginConfiguration pluginConfiguration;
	private readonly IPluginLog log;

	public ConfigWindow(PluginConfiguration _pluginConfiguration, IPluginLog _log) : base("Chronicle - Configuration")
	{
		Flags = ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse;
		Size = new Vector2(500, 130);
		SizeCondition = ImGuiCond.Always;
		pluginConfiguration = _pluginConfiguration;
		log = _log;
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

		ImGui.SetCursorPos(new Vector2(450, 90));
		if (ImGui.Button("Save", new Vector2(40, 30)))
		{
			pluginConfiguration.Save();
			log.Debug("Save button hit");
		}
	}
}
