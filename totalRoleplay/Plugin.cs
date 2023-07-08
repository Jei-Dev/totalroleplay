using Dalamud.ContextMenu;
using Dalamud.Game;
using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.Keys;
using Dalamud.Game.ClientState.Objects;
using Dalamud.Game.Command;
using Dalamud.Game.Gui;
using Dalamud.Game.Gui.Toast;
using Dalamud.Interface.Windowing;
using Dalamud.IoC;
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
		public DialogueService dialogueService { get; init; }
		public GameInteractionHandler gameInteractionHandler { get; init; }
		public CommandHandler commandHandler { get; init; }

		public MainWindow TRPWindowMain { get; init; }
		public QuestListWindow QuestListWindow { get; init; }
		public currencyWindow currencyWindow { get; init; }
		public FakeDialogueWindow fakeDialogueWindow { get; init; }
		public DialogueTriggerWindow dialogueTriggerWindow { get; init; }

		public Plugin(DalamudPluginInterface pluginInterface)
		{
			var dalamud = new DalamudServices();
			pluginInterface.Inject(dalamud);

			var pluginConfiguration = pluginInterface.GetPluginConfig() as PluginConfiguration ?? new PluginConfiguration();
			pluginConfiguration.Initialize(pluginInterface);
			dialogueService = new DialogueService();
			var questService = new QuestService(pluginInterface, dialogueService);
			dalamudContextMenu = new DalamudContextMenu();
			gameInteractionHandler = new GameInteractionHandler(pluginConfiguration, dalamudContextMenu, dalamud.objectTable, questService);

			ConfigWindow = new ConfigWindow(pluginConfiguration);
			TRPWindowMain = new MainWindow(this, pluginConfiguration);
			QuestListWindow = new QuestListWindow(this, pluginConfiguration, questService, dalamud.toastGui);
			currencyWindow = new currencyWindow();
			fakeDialogueWindow = new FakeDialogueWindow(dialogueService, dalamud.keyState, dalamud.clientState, dalamud.targetManager, pluginConfiguration, pluginInterface);
			dialogueTriggerWindow = new DialogueTriggerWindow(dalamud.targetManager, questService);

			WindowSystem.AddWindow(ConfigWindow);
			WindowSystem.AddWindow(TRPWindowMain);
			WindowSystem.AddWindow(QuestListWindow);
			WindowSystem.AddWindow(currencyWindow);
			WindowSystem.AddWindow(fakeDialogueWindow);
			WindowSystem.AddWindow(dialogueTriggerWindow);

			commandHandler = new CommandHandler(this, dalamud.commandManager, questService);
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

		private class DalamudServices
		{
			[PluginService]
			public DalamudPluginInterface pluginInterface { get; private set; } = null!;

			[PluginService]
			public CommandManager commandManager { get; private set; } = null!;

			//        [PluginService]
			//        public FlyTextGui flyTextGui { get; private set; } = null!;

			[PluginService]
			public ToastGui toastGui { get; private set; } = null!;

			[PluginService]
			public ClientState clientState { get; private set; } = null!;

			[PluginService]
			public ChatGui chatGui { get; private set; } = null!;

			[PluginService]
			public SigScanner sigScanner { get; private set; } = null!;

			[PluginService]
			public ObjectTable objectTable { get; private set; } = null!;

			[PluginService]
			public Framework framework { get; private set; } = null!;

			[PluginService]
			public GameGui gameGui { get; private set; } = null!;

			[PluginService]
			public KeyState keyState { get; private set; } = null!;

			[PluginService]
			public TargetManager targetManager { get; private set; } = null!;
		}
	}
}
