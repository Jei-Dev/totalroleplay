using Dalamud.Game.Command;
using Dalamud.Interface.Windowing;
using Dalamud.Logging;
using Dalamud.Plugin;
using ImGuiNET;
using System;
using totalRoleplay.Service;
using totalRoleplay.Windows;

namespace totalRoleplay.Handlers;

public class CommandHandler : IDisposable
{
	private record Commands
	{
		public required string CommandName { get; init; }
		public string? HelpMessage { get; init; }
	};

	private readonly Commands[] ACommands = {
		new Commands { CommandName = "trp", HelpMessage = "Opens the Total Roleplay menu."},
		new Commands { CommandName = "trpq", HelpMessage = null},
		new Commands { CommandName = "trpqa", HelpMessage = null},
		new Commands { CommandName = "trpqb", HelpMessage = null},
		new Commands { CommandName = "trpqt", HelpMessage = null},
		new Commands { CommandName = "trpcurrency", HelpMessage = "Opens the Currency Window"},
		new Commands { CommandName = "trpfakeDialogue", HelpMessage = "Opens the Fake Dialogue Window"}
	};

	private readonly Plugin plugin;
	private readonly CommandManager commandManager;
	private readonly QuestService questService;

	public CommandHandler(Plugin plugin, CommandManager commandManager, QuestService questService)
	{
		this.plugin = plugin;
		this.commandManager = commandManager;
		this.questService = questService;

		for (int i = 0; i < ACommands.Length; i++)
		{
			commandManager.AddHandler("/" + ACommands[i].CommandName, new CommandInfo(CommandsHandler) { HelpMessage = ACommands[i].HelpMessage ?? "N/A" });
			PluginLog.LogDebug("CommandManager: Loaded /" + ACommands[i].CommandName);
		}
	}

	public void Dispose()
	{
		for (int i = 0; i < ACommands.Length; i++)
		{
			commandManager.RemoveHandler("/" + ACommands[i].CommandName);
			PluginLog.LogDebug("CommandManager: Destroyed /" + ACommands[i].CommandName);
		}
	}

	public void CommandsHandler(string command, string arguments)
	{
		switch (command)
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
			case "/trpfakeDialogue":
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
		}
	}
}
