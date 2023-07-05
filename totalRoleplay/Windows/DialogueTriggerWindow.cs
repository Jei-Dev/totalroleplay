using System;
using System.Diagnostics;
using Dalamud.Game.ClientState.Objects;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using totalRoleplay.Service;

namespace totalRoleplay.Windows;

public class DialogueTriggerWindow : Window, IDisposable
{
	private TargetManager targetManager { get; init; }
	private QuestService questService { get; init; }

	public DialogueTriggerWindow(TargetManager targetManager, QuestService questService) : base("Asdf")
	{
		this.IsOpen = true;
		this.targetManager = targetManager;
		this.questService = questService;
	}

	public void Dispose()
	{
	}

	public override bool DrawConditions()
	{
		var targetId = targetManager.Target?.DataId;
		return targetId != null && questService.CanInteractWithTarget(targetId.Value);
	}

	public override void Draw()
	{
		if (ImGui.Button("Talk"))
		{
			var targetId = targetManager.Target?.DataId;
			if (targetId != null)
			{
				questService.InteractWithTarget(targetId.Value);
			}
		}
	}
}
