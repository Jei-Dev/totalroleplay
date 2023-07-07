using Dalamud.ContextMenu;
using Dalamud.Logging;
using FFXIVClientStructs.FFXIV.Client.UI;
using System;
using totalRoleplay.Configuration;

namespace totalRoleplay.Handlers
{
	public class GameInteractionHandler : IDisposable
	{
		private readonly PluginConfiguration pluginConfig;
		private readonly DalamudContextMenu _dalamudContextMenu;
		public GameInteractionHandler(PluginConfiguration pluginConfig, DalamudContextMenu dalamudContextMenu)
		{
			_dalamudContextMenu = dalamudContextMenu;

			_dalamudContextMenu.OnOpenGameObjectContextMenu += OpenGameObjectContextMenu;
		}
		public void OpenGameObjectContextMenu(GameObjectContextMenuOpenArgs args)
		{
			if (args.ObjectId == 0xE000000) { PluginLog.LogVerbose("Object ID does not match - Ignoring"); return; }
			if (!pluginConfig.gameInteractionContextMenu) { PluginLog.LogVerbose("Ignoring Context Menu creation - User does not allow it"); return; }

			// Need logic to add Context Selection to all NPCs currently loaded
			AddContextMenu(args);
		}

		public void Dispose()
		{
			_dalamudContextMenu.OnOpenGameObjectContextMenu -= OpenGameObjectContextMenu;
		}

		public void AddContextMenu(GameObjectContextMenuOpenArgs args)
		{
			// Needs player/npc null check

			args.AddCustomItem(new GameObjectContextMenuItem("[TRP] Talk", (a) =>
			{
				// Allow the Item to do something when pressed (Specifically fake dialogue for "Talking")
			}, false));

			var _QuestItem = "Empty Item";
			args.AddCustomItem(new GameObjectContextMenuItem("[TRP] Give (" + _QuestItem + ")", (a) =>
			{

			}, false));
		}
	}
}
