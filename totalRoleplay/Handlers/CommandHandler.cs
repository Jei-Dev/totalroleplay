using Dalamud.Game.Command;
using Dalamud.Interface.Windowing;
using Dalamud.Logging;
using Dalamud.Plugin;
using System;
using totalRoleplay.Service;

namespace totalRoleplay.Handlers;

public static class CommandHandler
{
	private record Commands
	{
		public required string CommandName { get; init; }
		public string? HelpMessage { get; init; }
	};

	private static readonly Commands[] ACommands = {
		new Commands { CommandName = "trp", HelpMessage = "Opens the Total Roleplay menu."},
		new Commands { CommandName = "trpq", HelpMessage = null},
		new Commands { CommandName = "trpqa", HelpMessage = null},
		new Commands { CommandName = "trpqb", HelpMessage = null},
		new Commands { CommandName = "trpqt", HelpMessage = null},
		new Commands { CommandName = "trpcurrency", HelpMessage = "Opens the Currency Window"},
		new Commands { CommandName = "trpfakeDialogue", HelpMessage = "Opens the Fake Dialogue Window"}
	};

	/// <summary>
	/// Call to Load all Commands currently set in ReadOnly table "Commands".
	/// </summary>
	public static void Load()
	{
		for (int i = 0; i < ACommands.Length; i++)
		{
			IAmGod.commandManager.AddHandler("/" + ACommands[i].CommandName, new CommandInfo(CommandsHandler) { HelpMessage = ACommands[i].HelpMessage ?? "N/A" });
			PluginLog.LogDebug("CommandManager: Loaded /" + ACommands[i].CommandName);
		}
	}

	/// <summary>
	/// Call to UnLoad all Commands currently Loaded. ONLY call in Dispose()
	/// </summary>
	public static void UnLoad()
	{
		for (int i = 0; i < ACommands.Length; i++)
		{
			IAmGod.commandManager.RemoveHandler("/" + ACommands[i].CommandName);
			PluginLog.LogDebug("CommandManager: Destroyed /" + ACommands[i].CommandName);
		}
	}

	public static void CommandsHandler(string command, string arguments)
	{
		switch (command)
		{
			case "/trp":
				IAmGod.plugin.TRPWindowMain.IsOpen = !IAmGod.plugin.TRPWindowMain.IsOpen;
				break;
			case "/trpq":
				IAmGod.plugin.QuestListWindow.IsOpen = true;
				break;
			case "/trpqa":
				IAmGod.plugin.QuestListWindow.IncrementCurrentQuestGoal();
				break;
			case "/trpqb":
				IAmGod.questService.BeginQuest(arguments);
				break;
			case "/trpqt":
				IAmGod.questService.TriggerCommand(arguments);
				break;
			case "/trpcurrency":
				IAmGod.plugin.currencyWindow.IsOpen = !IAmGod.plugin.currencyWindow.IsOpen;
				break;
		}
	}
}
