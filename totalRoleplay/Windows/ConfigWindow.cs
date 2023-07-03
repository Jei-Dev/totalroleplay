using Dalamud.Interface.Windowing;
using ImGuiNET;
using System;
using System.Numerics;

namespace totalRoleplay.Windows;

public class ConfigWindow : Window, IDisposable
{
	public ConfigWindow() : base("Total Roleplay Configuration")
	{
		Flags = ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse;
		this.Size = new Vector2(232, 75);
		this.SizeCondition = ImGuiCond.Always;
	}

	public void Dispose() { }

	public override void Draw()
	{
		// can't ref a property, so use a local copy
		var configValue = Service.pluginConfig.BooleanProperty;
		if (ImGui.Checkbox("Random Config Bool", ref configValue))
		{
			Service.pluginConfig.BooleanProperty = configValue;
			// can save immediately on change, if you don't want to provide a "Save and Close" button
			Service.pluginConfig.Save();
		}
	}
}
