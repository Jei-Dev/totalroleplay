using Dalamud.Game.Command;
using Dalamud.Plugin.Services;
using ImGuiNET;
using System;
using totalRoleplay.Service;

namespace totalRoleplay.Handlers;

public class ICommandHandler : IDisposable
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
		new Commands { CommandName = "trpc", HelpMessage = "Opens the Currency Window"},
		new Commands { CommandName = "trpfd", HelpMessage = "Opens the Fake Dialogue Window"},
		new Commands { CommandName = "trpcs", HelpMessage = "Opens the TRP Character Sheet"}
	};

	private readonly Plugin plugin;
	private readonly ICommandHandler commandHandler;
	private readonly QuestService questService;
	private readonly ICommandManager commandManager;
	private readonly IPluginLog log;

	public ICommandHandler(Plugin plugin, ICommandHandler commandHandler, QuestService questService, ICommandManager commandManager, IPluginLog log)
	{
		this.plugin = plugin;
		this.commandHandler = commandHandler;
		this.questService = questService;
		this.commandManager = commandManager;
		this.log = log;

		for (var i = 0; i < aCommands.Length; i++)
		{

			log.Debug("CommandHandler: Loaded / {0}", aCommands[i].CommandName);

			commandManager.AddHandler("/" + aCommands[i].CommandName, new CommandInfo(CommandsHandler) { HelpMessage = aCommands[i].HelpMessage ?? "N/A" });
		}
	}

	public void Dispose()
	{
		for (var i = 0; i < aCommands.Length; i++)
		{
			commandManager.RemoveHandler("/" + aCommands[i].CommandName);
			log.Debug("CommandHandler: Destroyed / {0}", aCommands[i].CommandName);
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
					log.Debug("Stopped dialogue timer? " + fakeDialogueTimer);
					ImGui.GetIO().WantTextInput = false;
				}
				else { fakeDialogueWin.sw.Start(); log.Debug("Started dialogue timer? " + !fakeDialogueTimer); }
				break;
			case "/trpcharsheet":
				plugin.characterSheetWindow.Toggle();
				break;
		}
	}
}
