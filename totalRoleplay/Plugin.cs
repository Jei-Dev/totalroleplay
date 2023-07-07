using Dalamud.ContextMenu;
using Dalamud.Game.ClientState.Keys;
using Dalamud.Game.ClientState.Objects;
using Dalamud.Interface.Windowing;
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

        private DalamudPluginInterface PluginInterface { get; init; }
        public PluginConfiguration pluginConfiguration { get; init; }
        public DalamudContextMenu dalamudContextMenu { get; init; }
        public WindowSystem WindowSystem = new("totalRoleplay");
        public ConfigWindow ConfigWindow { get; init; }
        public MainWindow TRPWindowMain { get; init; }
        public QuestListWindow QuestListWindow { get; init; }
        public currencyWindow currencyWindow { get; init; }
        public FakeDialogueWindow fakeDialogueWindow { get; init; }
        public FakeDialogueHandler fakeDialogueHandler { get; init; }
        public DialogueTriggerWindow dialogueTriggerWindow { get; init; }
        public GameInteractionHandler gameInteractionHandler { get; init; }

        public Plugin(DalamudPluginInterface pluginInterface)
        {

            pluginInterface.Create<IAmGod>();

            IAmGod.plugin = this;
            IAmGod.pluginConfiguration = pluginInterface.GetPluginConfig() as PluginConfiguration ?? new PluginConfiguration();
            IAmGod.pluginConfiguration.Initialize(pluginInterface);
            IAmGod.questService = new QuestService(pluginInterface);
            dalamudContextMenu = new DalamudContextMenu();
            fakeDialogueHandler = new FakeDialogueHandler();
            gameInteractionHandler = new GameInteractionHandler(IAmGod.pluginConfiguration, dalamudContextMenu, IAmGod.objectTable, fakeDialogueHandler);
            IAmGod.questService = new QuestService(pluginInterface, fakeDialogueHandler);

            ConfigWindow = new ConfigWindow();
            TRPWindowMain = new MainWindow();
            QuestListWindow = new QuestListWindow(this);
            currencyWindow = new currencyWindow();
            fakeDialogueWindow = new FakeDialogueWindow(dialogueHandler, IAmGod.keyState, IAmGod.clientState);
            dialogueTriggerWindow = new DialogueTriggerWindow(IAmGod.targetManager, IAmGod.questService);

            WindowSystem.AddWindow(ConfigWindow);
            WindowSystem.AddWindow(TRPWindowMain);
            WindowSystem.AddWindow(QuestListWindow);
            WindowSystem.AddWindow(currencyWindow);
            WindowSystem.AddWindow(fakeDialogueWindow);
            WindowSystem.AddWindow(dialogueTriggerWindow);

            CommandHandler.Load();
            /*
			 * We don't need to keep this, however its here for Histories sake. (Remove when cleaning code for production)
			IAmGod.commandManager.AddHandler("/trp", new CommandInfo(OnCommand) { HelpMessage = "Opens the Total Roleplay window." });
			IAmGod.commandManager.AddHandler("/trpq", new CommandInfo(OnCommand));
			IAmGod.commandManager.AddHandler("/trpqa", new CommandInfo(OnCommand));
			IAmGod.commandManager.AddHandler("/trpqb", new CommandInfo(OnCommand));
			IAmGod.commandManager.AddHandler("/trpqt", new CommandInfo(OnCommand));
			IAmGod.commandManager.AddHandler("/trpcurrency", new CommandInfo(OnCommand) { HelpMessage = "Shows Total Roleplay's Currency Window." });
			*/
            pluginInterface.UiBuilder.Draw += DrawUI;
            pluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
        }

        public void Dispose()
        {
            CommandHandler.UnLoad();
            this.WindowSystem.RemoveAllWindows();
            dalamudContextMenu.Dispose();

            ConfigWindow.Dispose();
            TRPWindowMain.Dispose();
            QuestListWindow.Dispose();
            currencyWindow.Dispose();
            fakeDialogueWindow.Dispose();
            dialogueTriggerWindow.Dispose();

            /*
			IAmGod.commandManager.RemoveHandler("/trp");
			IAmGod.commandManager.RemoveHandler("/trpq");
			IAmGod.commandManager.RemoveHandler("/trpqa");
			IAmGod.commandManager.RemoveHandler("/trpqb");
			IAmGod.commandManager.RemoveHandler("/trpqt");
			IAmGod.commandManager.RemoveHandler("/trpcurrency");
			*/
        }
        /*
		private void OnCommand(string command, string args)
		{
			// in response to the slash command, just display our main ui
			switch (command)
			{
				case "/trp":
					TRPWindowMain.IsOpen = !TRPWindowMain.IsOpen;
					break;
				case "/trpq":
					QuestListWindow.IsOpen = true;
					break;
				case "/trpqa":
					QuestListWindow.IncrementCurrentQuestGoal();
					break;
				case "/trpqb":
					IAmGod.questService.BeginQuest(args);
					break;
				case "/trpqt":
					IAmGod.questService.TriggerCommand(args);
					break;
				case "/trpcurrency":
					currencyWindow.IsOpen = !currencyWindow.IsOpen;
					break;
			}
		}
		*/

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
