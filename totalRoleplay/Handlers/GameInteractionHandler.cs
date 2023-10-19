using Dalamud.ContextMenu;
using Dalamud.Plugin.Services;
using System;
using totalRoleplay.Configuration;
using totalRoleplay.Service;

namespace totalRoleplay.Handlers;
public class GameInteractionHandler : IDisposable
{
	private readonly PluginConfiguration pluginConfig;
	private readonly DalamudContextMenu dalamudContextMenu;
	private readonly IObjectTable objectTable;
	private readonly QuestService questService;
	private readonly IPluginLog log;
	public GameInteractionHandler(PluginConfiguration _pluginConfig, DalamudContextMenu _dalamudContextMenu, IObjectTable _objectTable, QuestService _questService, IPluginLog log)
	{
		pluginConfig = _pluginConfig;
		dalamudContextMenu = _dalamudContextMenu;
		objectTable = _objectTable;
		questService = _questService;
		this.log = log;

		_dalamudContextMenu.OnOpenGameObjectContextMenu += OpenGameObjectContextMenu;
	}
	public void OpenGameObjectContextMenu(GameObjectContextMenuOpenArgs args)
	{
		if (args.ObjectId == 0xE000000) { log.Verbose("Object ID does not match - Ignoring"); return; }
		if (!pluginConfig.gameInteractionContextMenu) { log.Verbose("Ignoring Context Menu creation - User does not allow it"); return; }

		AddContextMenu(args);
	}

	public void Dispose()
	{
		dalamudContextMenu.OnOpenGameObjectContextMenu -= OpenGameObjectContextMenu;
	}

	public void AddContextMenu(GameObjectContextMenuOpenArgs args)
	{
		var gameObject = objectTable.SearchById(args.ObjectId);
		if (gameObject == null) return;

		if (questService.CanInteractWithTarget(gameObject))
		{
			args.AddCustomItem(new GameObjectContextMenuItem("[TRP] Talk to " + gameObject.Name, (a) =>
			{
				questService.InteractWithTarget(gameObject);
				// Allow the Item to do something when pressed (Specifically fake dialogue for "Talking")
			}, false));

			var _QuestItem = "Empty Item";
			args.AddCustomItem(new GameObjectContextMenuItem("[TRP] Give (" + _QuestItem + ")", (a) =>
			{
				log.Information("You gave a " + _QuestItem + " to " + gameObject.Name);
			}, false));
		}
	}
}
