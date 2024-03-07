using chroniclePlugin.Configuration;
using chroniclePlugin.Service;
using Dalamud.ContextMenu;
using Dalamud.Plugin.Services;
using System;

namespace chroniclePlugin.Handlers;
#pragma warning disable CS9113 // Parameter is unread.
public class GameInteractionHandler : IDisposable
#pragma warning restore CS9113 // Parameter is unread.
{
	private readonly DalamudContextMenu dalamudContextMenu;
	private readonly IObjectTable objectTable;
	private readonly QuestService questService;
	private readonly IPluginLog log;
	private readonly PluginConfiguration pluginConfig;

	public GameInteractionHandler(PluginConfiguration pluginConfig, DalamudContextMenu dalamudContextMenu, IObjectTable objectTable, QuestService questService, IPluginLog log)
	{
		this.pluginConfig = pluginConfig;
		this.dalamudContextMenu = dalamudContextMenu;
		this.objectTable = objectTable;
		this.questService = questService;
		this.log = log;

		dalamudContextMenu.OnOpenGameObjectContextMenu += OpenGameObjectContextMenu;
	}

	public void Dispose()
	{
		dalamudContextMenu.OnOpenGameObjectContextMenu -= OpenGameObjectContextMenu;
	}

	public void OpenGameObjectContextMenu(GameObjectContextMenuOpenArgs args)
	{
		//if (args.ObjectId == 0xE000000) { log.Verbose("Object ID does not match - Ignoring"); return; }
		if (!pluginConfig.gameInteractionContextMenu) { log.Verbose("Ignoring Context Menu creation - User does not allow it"); return; }

		var gameObject = objectTable.SearchById(args.ObjectId);
		if (gameObject == null) { log.Verbose("GameObject: " + gameObject + " does not exist."); return; }

		//if (questService.CanInteractWithTarget(gameObject))
		if (gameObject)
		{
			args.AddCustomItem(new GameObjectContextMenuItem(" Talk to " + gameObject.Name, (a) =>
			{
				questService.InteractWithTarget(gameObject);
				// Allow the Item to do something when pressed (Specifically fake dialogue for "Talking")
			}));

			var _QuestItem = "Empty Item";
			args.AddCustomItem(new GameObjectContextMenuItem(" Give (" + _QuestItem + ")", (a) =>
			{
				log.Information("You gave a " + _QuestItem + " to " + gameObject.Name);
			}, false));
		}
		log.Verbose("Opened Context Menu");
	}
}
