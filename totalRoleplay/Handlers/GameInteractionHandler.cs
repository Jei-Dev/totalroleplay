using Dalamud.ContextMenu;
using Dalamud.Game.ClientState.Objects;
using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Logging;
using System;
using totalRoleplay.Configuration;
using totalRoleplay.Handlers;
using totalRoleplay.Service;

namespace totalRoleplay.Handlers;
public class GameInteractionHandler : IDisposable
{
	private readonly PluginConfiguration pluginConfig;
	private readonly DalamudContextMenu dalamudContextMenu;
	private readonly ObjectTable objectTable;
	private readonly FakeDialogueHandler fakeDialogueHandler;
	public GameInteractionHandler(PluginConfiguration _pluginConfig, DalamudContextMenu _dalamudContextMenu, ObjectTable _objectTable, FakeDialogueHandler _fakeHandler)
	{
		pluginConfig = _pluginConfig;
		dalamudContextMenu = _dalamudContextMenu;
		objectTable = _objectTable;
		fakeDialogueHandler = _fakeHandler;

		_dalamudContextMenu.OnOpenGameObjectContextMenu += OpenGameObjectContextMenu;
	}
	public void OpenGameObjectContextMenu(GameObjectContextMenuOpenArgs args)
	{
		if (args.ObjectId == 0xE000000) { PluginLog.LogVerbose("Object ID does not match - Ignoring"); return; }
		if (!pluginConfig.gameInteractionContextMenu) { PluginLog.LogVerbose("Ignoring Context Menu creation - User does not allow it"); return; }

		AddContextMenu(args);
	}

	public void Dispose()
	{
		dalamudContextMenu.OnOpenGameObjectContextMenu -= OpenGameObjectContextMenu;
	}

	public void AddContextMenu(GameObjectContextMenuOpenArgs args)
	{
		var gameObject = objectTable.SearchById(args.ObjectId);
		if (!gameObject) return;

		args.AddCustomItem(new GameObjectContextMenuItem("[TRP] Talk to " + gameObject.Name, (a) =>
		{
			fakeDialogueHandler.startFakeDialogue();
			// Allow the Item to do something when pressed (Specifically fake dialogue for "Talking")
		}, false));

		var _QuestItem = "Empty Item";
		args.AddCustomItem(new GameObjectContextMenuItem("[TRP] Give (" + _QuestItem + ")", (a) =>
		{
			PluginLog.LogInformation("You gave a " + _QuestItem + " to " + gameObject.Name);
		}, false));
	}
}
