using Dalamud.Game.Command;
using Dalamud.Game.Gui.Toast;
using Dalamud.Interface.Windowing;
using Dalamud.IoC;
using Dalamud.Plugin;

namespace totalRoleplay
{
    public sealed class Plugin : IDalamudPlugin
    {
        public string Name => "Roleplay Totality";
        private const string PluginCommand = "/trp";
        private const string QuestCommand = "/trpq";

        private DalamudPluginInterface PluginInterface { get; init; }
        private CommandManager CommandManager { get; init; }
        public Configuration Configuration { get; init; }
        public WindowSystem WindowSystem = new("totalRoleplay");

        private ConfigWindow ConfigWindow { get; init; }
        private TRPWindowMain TRPWindowMain { get; init; }

        public Plugin(DalamudPluginInterface pluginInterface)
        {

            pluginInterface.Create<Service>();

            Service.plugin = this;
            Service.pluginConfig = pluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            Service.pluginConfig.Initialize(pluginInterface);

            ConfigWindow = new ConfigWindow();
            TRPWindowMain = new TRPWindowMain();

            WindowSystem.AddWindow(ConfigWindow);
            WindowSystem.AddWindow(TRPWindowMain);

            Service.commandManager.AddHandler("/trp", new CommandInfo(OnCommand) { HelpMessage = "Opens the Total Roleplay window." });
            Service.commandManager.AddHandler("/trpa", new CommandInfo(OnCommand) { HelpMessage = "Displays custom text in a Toast" });

            pluginInterface.UiBuilder.Draw += DrawUI;
            pluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
        }

        public void Dispose()
        {
            this.WindowSystem.RemoveAllWindows();

            ConfigWindow.Dispose();
            TRPWindowMain.Dispose();

            Service.commandManager.RemoveHandler("/trp");
            Service.commandManager.RemoveHandler("/trpa");
        }


        public void showQuestLine(string args)
        {
            var canShow = Service.pluginConfig.showTextNotify;
            if (canShow)
            {
                Service.toastGui?.ShowQuest($"{args}", new QuestToastOptions
                {
                    Position = QuestToastPosition.Centre,
                    DisplayCheckmark = false,
                    IconId = 0,
                    PlaySound = true
                });
            }
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
                    showQuestLine(args);
                    break;
            }
        }

        private void DrawUI()
        {
            this.WindowSystem.Draw();
        }

        public void DrawConfigUI()
        {
            ConfigWindow.IsOpen = true;
        }
    }
}
