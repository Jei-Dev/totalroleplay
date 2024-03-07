using Dalamud.Plugin;
using System;

namespace chroniclePlugin.Configuration;
[Serializable]
public class CharacterConfiguration
{
	public string characterName { get; set; } = "Name!"; // Replace this eventually with the characters actual name. For development this is currently a custom string.
	public string characterPhysicalBiography { get; set; } = "This character has not set-up a physical biography."; // Is it smart to save a Biography like this? We'll find out...
	public string characterHistoryBiography { get; set; } = "This character does not have a History.";
	public string characterPronouns { get; set; } = "She/Her";
	public int currentJob { get; set; }
	public string? characterTitle { get; set; } // Should look into getting the title from Honorific.
	public bool isPrefix { get; set; } = false; // In this context, isPrefix just means whether or not the Title is a prefix. This is false by default.
	public CharacterJobs[] characterJobList { get; set; } =
	[
		new CharacterJobs { name = "(None)", description = "", joblink = null },
		new CharacterJobs { name = "Baker", description = "I bake things", joblink = null },
		new CharacterJobs { name = "Show Starter", description = "I like doing plays.", joblink = null }
		];

	[NonSerialized]
#pragma warning disable IDE0051 // Remove unused private members
	private readonly DalamudPluginInterface? pluginInterface;
#pragma warning restore IDE0051 // Remove unused private members
	public record CharacterJobs
	{
		public required string? name { get; set; }
		public string? description { get; set; }
		public int? joblink { get; set; }
	}
	public static void Save()
	{
		// Save Logic
		//IPluginLog.Warning("Save not implimented.");
	}

}
