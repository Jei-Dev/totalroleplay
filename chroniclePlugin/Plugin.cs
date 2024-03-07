using chroniclePlugin.Configuration;
using chroniclePlugin.Handlers;
using chroniclePlugin.Service;
using chroniclePlugin.Windows;
using chroniclePlugin.Windows.characterinteraction.firstimpression;
using chroniclePlugin.Windows.charactersheet;
using Dalamud.ContextMenu;
using Dalamud.Game.ClientState.Objects;
using Dalamud.Interface.Windowing;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;

namespace chroniclePlugin
{
	public sealed class Plugin : IDalamudPlugin
	{
		public static string Name => "Chronicle";

		public DalamudContextMenu dalamudContextMenu { get; init; }
		public WindowSystem WindowSystem { get; init; }
		public ConfigWindow ConfigWindow { get; init; }
		public DialogueService dialogueService { get; init; }
		public GameInteractionHandler gameInteractionHandler { get; init; }
		public CommandHandler commandHandler { get; init; }
		public CharacterConfiguration characterConfiguration { get; init; }
		public CommandHandler _commandHandler { get; private set; } = null!;
		public MainWindow TRPWindowMain { get; init; }
		public QuestListWindow QuestListWindow { get; init; }
		public CurrencyWindow currencyWindow { get; init; }
		public FakeDialogueWindow fakeDialogueWindow { get; init; }
		public DialogueTriggerWindow dialogueTriggerWindow { get; init; }
		public CharacterSheetWindow characterSheetWindow { get; init; }
		public ImpressionWindow impressionWindow { get; init; }
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
			currencyWindow = new CurrencyWindow();
			fakeDialogueWindow = new FakeDialogueWindow(dialogueService, dalamud.keyState, dalamud.clientState, dalamud.targetManager, pluginConfiguration, pluginInterface);
			dialogueTriggerWindow = new DialogueTriggerWindow(dalamud.targetManager, questService);
			characterSheetWindow = new CharacterSheetWindow(characterConfiguration, charConfig: characterConfiguration, dalamud.loggingService);
			impressionWindow = new ImpressionWindow(dalamud.loggingService);

			WindowSystem = new("chroniclePlugin");
			WindowSystem.AddWindow(ConfigWindow);
			WindowSystem.AddWindow(TRPWindowMain);
			WindowSystem.AddWindow(QuestListWindow);
			WindowSystem.AddWindow(currencyWindow);
			WindowSystem.AddWindow(fakeDialogueWindow);
			WindowSystem.AddWindow(dialogueTriggerWindow);
			WindowSystem.AddWindow(characterSheetWindow);
			WindowSystem.AddWindow(impressionWindow);

			commandHandler = new CommandHandler(this, _commandHandler, questService, dalamud.commandManager, dalamud.loggingService);
			pluginInterface.UiBuilder.Draw += DrawUI;
			pluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
		}

		public void Dispose()
		{

			WindowSystem.RemoveAllWindows();
			dalamudContextMenu.Dispose();
			ConfigWindow.Dispose();
			TRPWindowMain.Dispose();
			QuestListWindow.Dispose();
			currencyWindow.Dispose();
			fakeDialogueWindow.Dispose();
			dialogueTriggerWindow.Dispose();
			commandHandler.Dispose();

		}

		private void DrawUI()
		{
			WindowSystem.Draw();
		}

		public void DrawConfigUI()
		{
			ConfigWindow.IsOpen = !ConfigWindow.IsOpen;
		}

		private sealed class DalamudServices
		{
			[PluginService] public DalamudPluginInterface pluginInterface { get; private set; } = null!;
			//	[PluginService] public FlyTextGui flyTextGui { get; private set; } = null!;
			[PluginService] public IToastGui toastGui { get; private set; } = null!;
			[PluginService] public IClientState clientState { get; private set; } = null!;
			[PluginService] public IChatGui chatGui { get; private set; } = null!;
			//	[PluginService] public SigScanner sigScanner { get; private set; } = null!;
			[PluginService] public IObjectTable objectTable { get; private set; } = null!;
			[PluginService] public IFramework framework { get; private set; } = null!;
			[PluginService] public IGameGui gameGui { get; private set; } = null!;
			[PluginService] public IKeyState keyState { get; private set; } = null!;
			[PluginService] public ITargetManager targetManager { get; private set; } = null!;
			[PluginService] public IPluginLog loggingService { get; private set; } = null!;
			[PluginService] public ICommandManager commandManager { get; private set; } = null!;
			//	Dalamud.ContextMenu is deprecated for 6.58 -> Will need to update to the Service below.
			//	[PluginService] public IContextMenu contextMenu { get; private set; } = null!;
			[PluginService] public ITextureProvider textureProvider { get; private set; } = null!;
		}
	}
}
