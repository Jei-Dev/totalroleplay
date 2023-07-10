using Dalamud.Game.Command;
using Dalamud.Logging;
using ImGuiNET;
using System;
using totalRoleplay.Service;

namespace totalRoleplay.Handlers;

public class CommandHandler : IDisposable
{
	private record Commands
	{
		public required string CommandName { get; init; }
		public string? HelpMessage { get; init; }
	};

	private readonly Commands[] aCommands = {
		new Commands { CommandName = "trp", HelpMessage = "Opens the Total Roleplay menu."},
		new Commands { CommandName = "trpq", HelpMessage = null},
		new Commands { CommandName = "trpqa", HelpMessage = null},
		new Commands { CommandName = "trpqb", HelpMessage = null},
		new Commands { CommandName = "trpqt", HelpMessage = null},
		new Commands { CommandName = "trpcurrency", HelpMessage = "Opens the Currency Window"},
		new Commands { CommandName = "trpfakeDialogue", HelpMessage = "Opens the Fake Dialogue Window"},
		new Commands { CommandName = "trpCharSheet", HelpMessage = "Opens the TRP Character Sheet"}
	};

	private readonly Plugin plugin;
	private readonly CommandManager commandManager;
	private readonly QuestService questService;

	public CommandHandler(Plugin plugin, CommandManager commandManager, QuestService questService)
	{
		this.plugin = plugin;
		this.commandManager = commandManager;
		this.questService = questService;

		for (int i = 0; i < aCommands.Length; i++)
		{
			commandManager.AddHandler("/" + aCommands[i].CommandName, new CommandInfo(CommandsHandler) { HelpMessage = aCommands[i].HelpMessage ?? "N/A" });
			PluginLog.LogDebug("CommandManager: Loaded /" + aCommands[i].CommandName);
		}
	}

	public void Dispose()
	{
		for (int i = 0; i < aCommands.Length; i++)
		{
			commandManager.RemoveHandler("/" + aCommands[i].CommandName);
			PluginLog.LogDebug("CommandManager: Destroyed /" + aCommands[i].CommandName);
		}
	}

	public void CommandsHandler(string command, string arguments)
	{
		switch (command.ToLower())
		{
			case "/trp":
				plugin.TRPWindowMain.Toggle();
				break;
			case "/trpq":
				plugin.QuestListWindow.Toggle();
				break;
			case "/trpqa":
				plugin.QuestListWindow.IncrementCurrentQuestGoal();
				break;
			case "/trpqb":
				questService.BeginQuest(arguments);
				break;
			case "/trpqt":
				questService.TriggerCommand(arguments);
				break;
			case "/trpcurrency":
				plugin.currencyWindow.Toggle();
				break;
			case "/trpfakedialogue":
				var fakeDialogueWin = plugin.fakeDialogueWindow;
				var fakeDialogueTimer = fakeDialogueWin.sw.IsRunning;
				fakeDialogueWin.IsOpen = !fakeDialogueWin.IsOpen;
				if (!fakeDialogueWin.IsOpen)
				{
					fakeDialogueWin.sw.Reset();
					PluginLog.LogDebug("Stopped dialogue timer? " + fakeDialogueTimer);
					ImGui.GetIO().WantTextInput = false;
				}
				else { fakeDialogueWin.sw.Start(); PluginLog.LogDebug("Started dialogue timer? " + !fakeDialogueTimer); }
				break;
			case "/trpcharsheet":
				plugin.characterSheetWindow.Toggle();
				break;
		}
	}
}
