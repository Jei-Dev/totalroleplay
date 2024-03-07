using chroniclePlugin.Service;
using Dalamud.Game.Command;
using Dalamud.Plugin.Services;
using ImGuiNET;
using System;

namespace chroniclePlugin.Handlers;

public class CommandHandler : IDisposable
{
	private sealed record Commands
	{
		public required string CommandName { get; init; }
		public string? HelpMessage { get; init; }
	};

	private readonly Commands[] aCommands = {
		new() { CommandName = "trp", HelpMessage = "Opens the Total Roleplay menu."},
		new() { CommandName = "trpq", HelpMessage = null},
		new() { CommandName = "trpqa", HelpMessage = null},
		new() { CommandName = "trpqb", HelpMessage = null},
		new() { CommandName = "trpqt", HelpMessage = null},
		new() { CommandName = "trpc", HelpMessage = "Opens the Currency Window"},
		new() { CommandName = "trpfd", HelpMessage = "Opens the Fake Dialogue Window"},
		new() { CommandName = "trpcs", HelpMessage = "Opens the Character Sheet"},
		new() { CommandName = "trpfi", HelpMessage = "Opens the First Impression Menu"},
		new() { CommandName = "cronfakelog", HelpMessage = "Fire a fake log message. 1, Verbose. 2, Debug."},
	};

	private readonly Plugin plugin;
	//private readonly ICommandHandler commandHandler;
	private readonly QuestService questService;
	private readonly ICommandManager commandManager;
	private readonly IPluginLog log;

	public CommandHandler(Plugin plugin, CommandHandler commandHandler, QuestService questService, ICommandManager commandManager, IPluginLog log)
	{
		this.plugin = plugin;
		//this.commandHandler = commandHandler;
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
			case "/trpc":
				plugin.currencyWindow.Toggle();
				break;
			case "/trpfd":
				var fakeDialogueWin = plugin.fakeDialogueWindow;
				var fakeDialogueTimer = fakeDialogueWin.sw.IsRunning;
				fakeDialogueWin.IsOpen = !fakeDialogueWin.IsOpen;
				if (!fakeDialogueWin.IsOpen)
				{
					fakeDialogueWin.sw.Reset();
					log.Debug("Stopped dialogue timer? {0}", fakeDialogueTimer);
					ImGui.GetIO().WantTextInput = false;
				}
				else { fakeDialogueWin.sw.Start(); log.Debug("Started dialogue timer? " + !fakeDialogueTimer); }
				break;
			case "/trpcs":
				plugin.characterSheetWindow.Toggle();
				break;
			case "/trpfi":
				plugin.impressionWindow.Toggle();
				break;
			case "/cronfakelog":
				switch (arguments)
				{
					case ("1"):
						log.Verbose("Fake Verbose Log Message");
						break;
					case ("2"):
						log.Debug("Fake Debug Log Message");
						break;
				}
				break;
		}
	}
}
