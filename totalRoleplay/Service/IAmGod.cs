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
