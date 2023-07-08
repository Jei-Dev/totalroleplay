using Dalamud.ContextMenu;
using Dalamud.Game.ClientState.Keys;
using Dalamud.Game.ClientState.Objects;
using Dalamud.Interface.Windowing;
using Dalamud.Logging;
using Dalamud.Plugin;
using totalRoleplay.Configuration;
using totalRoleplay.Handlers;
using totalRoleplay.Service;
using totalRoleplay.Windows;

namespace totalRoleplay
{
	public sealed class Plugin : IDalamudPlugin
	{
		public string Name => "Roleplay Totality";

		public DalamudContextMenu dalamudContextMenu { get; init; }
		public WindowSystem WindowSystem = new("totalRoleplay");
		public ConfigWindow ConfigWindow { get; init; }
		public FakeDialogueHandler fakeDialogueHandler { get; init; }
		public GameInteractionHandler gameInteractionHandler { get; init; }
		public CommandHandler commandHandler { get; init; }

		public MainWindow TRPWindowMain { get; init; }
		public QuestListWindow QuestListWindow { get; init; }
		public currencyWindow currencyWindow { get; init; }
		public FakeDialogueWindow fakeDialogueWindow { get; init; }
		public DialogueTriggerWindow dialogueTriggerWindow { get; init; }

		public Plugin(DalamudPluginInterface pluginInterface)
		{
			var god = new IAmGod();
			pluginInterface.Inject(god);

			var pluginConfiguration = pluginInterface.GetPluginConfig() as PluginConfiguration ?? new PluginConfiguration();
			pluginConfiguration.Initialize(pluginInterface);
			fakeDialogueHandler = new FakeDialogueHandler();
			var questService = new QuestService(pluginInterface, fakeDialogueHandler);
			dalamudContextMenu = new DalamudContextMenu();
			gameInteractionHandler = new GameInteractionHandler(pluginConfiguration, dalamudContextMenu, god.objectTable, questService);

			ConfigWindow = new ConfigWindow(pluginConfiguration);
			TRPWindowMain = new MainWindow(this, pluginConfiguration);
			QuestListWindow = new QuestListWindow(this, pluginConfiguration, questService, god.toastGui);
			currencyWindow = new currencyWindow();
			fakeDialogueWindow = new FakeDialogueWindow(fakeDialogueHandler, god.keyState, god.clientState, god.targetManager, pluginConfiguration, pluginInterface);
			dialogueTriggerWindow = new DialogueTriggerWindow(god.targetManager, questService);

			WindowSystem.AddWindow(ConfigWindow);
			WindowSystem.AddWindow(TRPWindowMain);
			WindowSystem.AddWindow(QuestListWindow);
			WindowSystem.AddWindow(currencyWindow);
			WindowSystem.AddWindow(fakeDialogueWindow);
			WindowSystem.AddWindow(dialogueTriggerWindow);

			commandHandler = new CommandHandler(this, god.commandManager, questService);
			pluginInterface.UiBuilder.Draw += DrawUI;
			pluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
		}

		public void Dispose()
		{
			commandHandler.Dispose();
			this.WindowSystem.RemoveAllWindows();
			dalamudContextMenu.Dispose();

			ConfigWindow.Dispose();
			TRPWindowMain.Dispose();
			QuestListWindow.Dispose();
			currencyWindow.Dispose();
			fakeDialogueWindow.Dispose();
			dialogueTriggerWindow.Dispose();

		}

		private void DrawUI()
		{
			this.WindowSystem.Draw();
		}

		public void DrawConfigUI()
		{
			ConfigWindow.IsOpen = !ConfigWindow.IsOpen;
		}
	}
}
