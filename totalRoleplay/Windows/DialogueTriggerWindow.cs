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
		var target = targetManager.Target;
		return target != null && questService.CanInteractWithTarget(target);
	}

	public override void Draw()
	{
		if (ImGui.Button("Talk"))
		{
			var target = targetManager.Target;
			if (target != null)
			{
				questService.InteractWithTarget(target);
			}
		}
	}
}
