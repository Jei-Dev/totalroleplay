using Dalamud.Logging;
using Dalamud.Plugin;
using System;

namespace totalRoleplay.Configuration;
[Serializable]
public class CharacterConfiguration
{
	public string characterName { get; set; } = "Name!"; // Replace this eventually with the characters actual name. For development this is current a custom string.
	public string[] characterJobs { get; set; } = { "My First Job" };
	public string characterPhysicalBiography { get; set; } = "This character has not set-up a physical biography."; // Is it smart to save a Biography like this? We'll find out...
	public string characterHistoryBiography { get; set; } = "This character does not have a History.";
	public int currentJob { get; set; }
	public string? characterTitle { get; set; } // Should look into getting the title from Honorific.
	public bool isPrefix { get; set; } = false; // In this context, isPrefix just means whether or not the Title is a prefix. This is false by default.

	[NonSerialized]
	private DalamudPluginInterface? pluginInterface;

	public void Save()
	{
		// Save Logic
		PluginLog.LogError("Save not implimented.");
	}

}
