using Dalamud.Game;
using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.Keys;
using Dalamud.Game.ClientState.Objects;
using Dalamud.Game.Command;
using Dalamud.Game.Gui;
//using Dalamud.Game.Gui.FlyText;
using Dalamud.Game.Gui.Toast;
using Dalamud.IoC;
using Dalamud.Plugin;
using totalRoleplay.Configuration;

namespace totalRoleplay.Service
{
	internal class IAmGod
	{
		public static Plugin plugin;

		public static PluginConfiguration pluginConfiguration;

		public static QuestService questService;

		[PluginService]
		public static DalamudPluginInterface pluginInterface { get; private set; } = null!;

		[PluginService]
		public static CommandManager commandManager { get; private set; } = null!;

		//        [PluginService]
		//        public static FlyTextGui flyTextGui { get; private set; } = null!;

		[PluginService]
		public static ToastGui toastGui { get; private set; } = null!;

		[PluginService]
		public static ClientState clientState { get; private set; } = null!;

		[PluginService]
		public static ChatGui chatGui { get; private set; } = null!;

		[PluginService]
		public static SigScanner sigScanner { get; private set; } = null!;

		[PluginService]
		public static ObjectTable objectTable { get; private set; } = null!;

		[PluginService]
		public static Framework framework { get; private set; } = null!;

		[PluginService]
		public static GameGui gameGui { get; private set; } = null!;

		[PluginService]
		public static KeyState keyState { get; private set; } = null!;

		[PluginService]
		public static TargetManager targetManager { get; private set; } = null!;

	}
}
