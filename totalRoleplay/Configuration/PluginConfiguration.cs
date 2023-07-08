using Dalamud.Configuration;
using Dalamud.Logging;
using Dalamud.Plugin;
using System;

namespace totalRoleplay.Configuration;
[Serializable]
public class PluginConfiguration : IPluginConfiguration
{
	public bool showTextNotify { get; set; } = true;

	private readonly static int VersionLatest = 0;
	public int Version { get; set; } = VersionLatest;

	public bool gameInteractionContextMenu { get; set; } = true;

	public float dialogueDrawSpeed { get; set; } = 0.05f;

	// the below exist just to make saving less cumbersome
	[NonSerialized]
	private DalamudPluginInterface? pluginInterface;

	public void Initialize(DalamudPluginInterface pluginInterface)
	{
		this.pluginInterface = pluginInterface;

		var needsResave = Version != VersionLatest;
		if (needsResave) { Save(); }
	}

	public void Save()
	{
		pluginInterface!.SavePluginConfig(this);
		PluginLog.Debug("Saved Configuration");
	}
}
