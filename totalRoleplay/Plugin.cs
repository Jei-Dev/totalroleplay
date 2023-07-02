using Dalamud.Game.Command;
using Dalamud.Game.Gui.Toast;
using Dalamud.Interface.Windowing;
using Dalamud.IoC;
using Dalamud.Plugin;
using System;

namespace totalRoleplay
{
    public sealed class Plugin : IDalamudPlugin
    {
        public string Name => "Roleplay Totality";

        private DalamudPluginInterface PluginInterface { get; init; }
        private CommandManager CommandManager { get; init; }
        public Configuration Configuration { get; init; }
        public WindowSystem WindowSystem = new("totalRoleplay");

        private ConfigWindow ConfigWindow { get; init; }
        private TRPWindowMain TRPWindowMain { get; init; }
        private QuestListWindow QuestListWindow { get; init; }

        public Plugin(DalamudPluginInterface pluginInterface)
        {

            pluginInterface.Create<Service>();

            Service.plugin = this;
            Service.pluginConfig = pluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            Service.pluginConfig.Initialize(pluginInterface);

            ConfigWindow = new ConfigWindow();
            TRPWindowMain = new TRPWindowMain();
            QuestListWindow = new QuestListWindow(this);

            WindowSystem.AddWindow(ConfigWindow);
            WindowSystem.AddWindow(TRPWindowMain);
            WindowSystem.AddWindow(QuestListWindow);

            Service.commandManager.AddHandler("/trp", new CommandInfo(OnCommand) { HelpMessage = "Opens the Total Roleplay window." });
            Service.commandManager.AddHandler("/trpq", new CommandInfo(OnCommand) { HelpMessage = "Displays custom text in a Toast" });
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
            Service.commandManager.RemoveHandler("/trpq");
        }


        public void showQuestLine(string[] args)
        {
            var arg1 = args[0];
            var arg2 = args[1];
            var completeQuest = arg1 == "true";
            var canShow = Service.pluginConfig.showTextNotify;
            if (canShow)
            {
                Service.toastGui?.ShowQuest($"{arg2}", new QuestToastOptions
                {
                    Position = QuestToastPosition.Centre,
                    DisplayCheckmark = completeQuest,
                    IconId = 0,
                    PlaySound = completeQuest,
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
                    QuestListWindow.IsOpen = true;
                    break;
                case "/trpa":
                    var sArgs = args.Split(' ', 2);
                    showQuestLine(sArgs);
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
