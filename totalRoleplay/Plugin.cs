using Dalamud.Game.Command;
using Dalamud.Game.Gui.Toast;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin;

namespace totalRoleplay
{
    public sealed class Plugin : IDalamudPlugin
    {
        public string Name => "Roleplay Totality";

        private DalamudPluginInterface PluginInterface { get; init; }
        public Configuration Configuration { get; init; }
        public WindowSystem WindowSystem = new("totalRoleplay");

        private ConfigWindow ConfigWindow { get; init; }
        private TRPWindowMain TRPWindowMain { get; init; }
        private QuestListWindow QuestListWindow { get; init; }
        private currencyWindow currencyWindow { get; init; }

        public Plugin(DalamudPluginInterface pluginInterface)
        {

            pluginInterface.Create<Service>();

            Service.plugin = this;
            Service.pluginConfig = pluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            Service.pluginConfig.Initialize(pluginInterface);
            Service.questService = new QuestService(pluginInterface);

            ConfigWindow = new ConfigWindow();
            TRPWindowMain = new TRPWindowMain();
            QuestListWindow = new QuestListWindow(this);
            currencyWindow = new currencyWindow();

            WindowSystem.AddWindow(ConfigWindow);
            WindowSystem.AddWindow(TRPWindowMain);
            WindowSystem.AddWindow(QuestListWindow);
            WindowSystem.AddWindow(currencyWindow);

            Service.commandManager.AddHandler("/trp", new CommandInfo(OnCommand) { HelpMessage = "Opens the Total Roleplay window." });
            Service.commandManager.AddHandler("/trpq", new CommandInfo(OnCommand));
            Service.commandManager.AddHandler("/trpqa", new CommandInfo(OnCommand));
            Service.commandManager.AddHandler("/trpcurrency", new CommandInfo(OnCommand) { HelpMessage = "Shows Total Roleplay's Currency Window." });

            pluginInterface.UiBuilder.Draw += DrawUI;
            pluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
        }

        public void Dispose()
        {
            this.WindowSystem.RemoveAllWindows();

            ConfigWindow.Dispose();
            TRPWindowMain.Dispose();

            Service.commandManager.RemoveHandler("/trp");
            Service.commandManager.RemoveHandler("/trpq");
            Service.commandManager.RemoveHandler("/trpqa");
            Service.commandManager.RemoveHandler("/trpcurrency");
        }
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
                case "/trpcurrency":
                    currencyWindow.IsOpen = !currencyWindow.IsOpen;
                    break;
            }
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
