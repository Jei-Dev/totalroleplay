using Dalamud.ContextMenu;
using Dalamud.Game.ClientState.Objects;
using Dalamud.Interface.Windowing;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using totalRoleplay.Configuration;
using totalRoleplay.Handlers;
using totalRoleplay.Service;
using totalRoleplay.Windows;
using totalRoleplay.Windows.charactersheet;

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
		public ICommandHandler commandHandler { get; init; }
		public CharacterConfiguration characterConfiguration { get; init; }

		public MainWindow TRPWindowMain { get; init; }
		public QuestListWindow QuestListWindow { get; init; }
		public currencyWindow currencyWindow { get; init; }
		public FakeDialogueWindow fakeDialogueWindow { get; init; }
		public DialogueTriggerWindow dialogueTriggerWindow { get; init; }
		public CharacterSheetWindow characterSheetWindow { get; init; }
		public Plugin(DalamudPluginInterface pluginInterface)
		{
			var dalamud = new DalamudServices();
			pluginInterface.Inject(dalamud);

			var pluginConfiguration = pluginInterface.GetPluginConfig() as PluginConfiguration ?? new PluginConfiguration();
			pluginConfiguration.Initialize(pluginInterface);
			characterConfiguration = new CharacterConfiguration();
			dialogueService = new DialogueService(dalamud.loggingService);
			var questService = new QuestService(pluginInterface, dialogueService, dalamud.loggingService);
			dalamudContextMenu = new DalamudContextMenu(pluginInterface);
			gameInteractionHandler = new GameInteractionHandler(pluginConfiguration, dalamudContextMenu, dalamud.objectTable, questService, dalamud.loggingService);

			ConfigWindow = new ConfigWindow(pluginConfiguration, dalamud.loggingService);
			TRPWindowMain = new MainWindow(this, pluginConfiguration);
			QuestListWindow = new QuestListWindow(this, pluginConfiguration, questService, dalamud.toastGui);
			currencyWindow = new currencyWindow();
			fakeDialogueWindow = new FakeDialogueWindow(dialogueService, dalamud.keyState, dalamud.clientState, dalamud.targetManager, pluginConfiguration, pluginInterface);
			dialogueTriggerWindow = new DialogueTriggerWindow(dalamud.targetManager, questService);
			characterSheetWindow = new CharacterSheetWindow(characterConfiguration);

			WindowSystem.AddWindow(ConfigWindow);
			WindowSystem.AddWindow(TRPWindowMain);
			WindowSystem.AddWindow(QuestListWindow);
			WindowSystem.AddWindow(currencyWindow);
			WindowSystem.AddWindow(fakeDialogueWindow);
			WindowSystem.AddWindow(dialogueTriggerWindow);
			WindowSystem.AddWindow(characterSheetWindow);

			commandHandler = new ICommandHandler(this, dalamud.commandHandler, questService, dalamud.commandManager, dalamud.loggingService);
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

			//[PluginService]
			public ICommandHandler commandHandler { get; private set; } = null!;

			//        [PluginService]
			//        public FlyTextGui flyTextGui { get; private set; } = null!;

			[PluginService]
			public IToastGui toastGui { get; private set; } = null!;

			[PluginService]
			public IClientState clientState { get; private set; } = null!;

			[PluginService]
			public IChatGui chatGui { get; private set; } = null!;

			//[PluginService]
			//public SigScanner sigScanner { get; private set; } = null!;

			[PluginService]
			public IObjectTable objectTable { get; private set; } = null!;

			[PluginService]
			public IFramework framework { get; private set; } = null!;

			[PluginService]
			public IGameGui gameGui { get; private set; } = null!;

			[PluginService]
			public IKeyState keyState { get; private set; } = null!;

			[PluginService]
			public ITargetManager targetManager { get; private set; } = null!;

			[PluginService]
			public IPluginLog loggingService { get; private set; } = null!;

			[PluginService]
			public ICommandManager commandManager { get; private set; } = null!;
		}
	}
}
