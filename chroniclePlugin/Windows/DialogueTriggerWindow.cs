using Dalamud.Game.ClientState.Objects;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using System;
using chroniclePlugin.Service;

namespace chroniclePlugin.Windows;

public class DialogueTriggerWindow : Window, IDisposable
{
	private ITargetManager targetManager { get; init; }
	private QuestService questService { get; init; }

	public DialogueTriggerWindow(ITargetManager targetManager, QuestService questService) : base("Asdf")
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
