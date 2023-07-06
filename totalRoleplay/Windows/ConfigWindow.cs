using Dalamud.Interface.Windowing;
using Dalamud.Logging;
using ImGuiNET;
using System;
using System.Numerics;
using totalRoleplay.Service;

namespace totalRoleplay.Windows;

public class ConfigWindow : Window, IDisposable
{
	public ConfigWindow() : base("Total Roleplay Configuration")
	{
		Flags = ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse;
		this.Size = new Vector2(500, 100);
		this.SizeCondition = ImGuiCond.Always;
	}

	public void Dispose() { }
	public override void Draw()
	{
		// can't ref a property, so use a local copy
		var configValue = IAmGod.pluginConfig.BooleanProperty;
		if (ImGui.Checkbox("Random Config Bool", ref configValue))
		{
			IAmGod.pluginConfig.BooleanProperty = configValue;
		}

		var textDrawSpeed = IAmGod.pluginConfig.dialogueDrawSpeed;
		if (ImGui.SliderFloat("Dialogue Speed", ref textDrawSpeed, 0.03f, 0.1f))
		{
			IAmGod.pluginConfig.dialogueDrawSpeed = textDrawSpeed;
		}



		ImGui.SetCursorPos(new Vector2(195, 60));
		if (ImGui.Button("Save", new Vector2(50, 50)))
		{
			IAmGod.pluginConfig.Save();
			PluginLog.Debug("Save button hit");
		}
	}
}
