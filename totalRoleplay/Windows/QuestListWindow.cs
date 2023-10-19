using Dalamud.Game.Gui.Toast;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using ImGuiNET;
using System;
using System.Numerics;
using totalRoleplay.Configuration;
using totalRoleplay.Service;

namespace totalRoleplay.Windows;

public class QuestListWindow : Window, IDisposable
{
	private string? selectedQuest;
	private readonly PluginConfiguration configuration;
	private readonly QuestService questService;
	private readonly IToastGui toastGui;

	public QuestListWindow(Plugin plugin, PluginConfiguration configuration, QuestService questService, IToastGui toastGui) : base("TRP - Quests", 0)
	{
		this.Size = new Vector2(200, 200);
		this.SizeCondition = ImGuiCond.Once;
		this.configuration = configuration;
		this.questService = questService;
		this.toastGui = toastGui;
		questService.QuestComplete += QuestCompleted;
	}

	public void Dispose()
	{
		questService.QuestComplete -= QuestCompleted;
	}

	public override void Draw()
	{
		ImGui.Text("Quests");
		if (ImGui.BeginListBox("##quests", new Vector2(float.Epsilon, (ImGui.GetTextLineHeightWithSpacing() * questService.ActiveQuests.Count * 2) + ImGui.GetStyle().FramePadding.Y)))
		{
			foreach (var activeQuest in questService.ActiveQuests)
			{
				var quest = questService.Quests[activeQuest.QuestId];
				if (ImGui.Selectable(quest.Title, activeQuest.QuestId == selectedQuest))
				{
					selectedQuest = activeQuest.QuestId;
				}
				var quSelect = activeQuest.IsTracked;
				var gCurr = activeQuest.GoalCurrent;
				var gNext = activeQuest.GoalFinal;
				ImGui.Checkbox($"{gCurr}/{gNext}", ref quSelect);
				activeQuest.IsTracked = quSelect;
			}
			ImGui.EndListBox();
		}

		if (selectedQuest != null)
		{
			ImGui.Text(questService.Quests[selectedQuest].Description);
		}
	}
	public void IncrementCurrentQuestGoal()
	{
		questService.ActiveQuests
			.FindAll(aq => aq.QuestId == selectedQuest)
			.ForEach(aq => aq.GoalCurrent++);
	}
	/// <summary>
	/// Shows Quest Toast
	/// </summary>
	public void showQuestToast(Boolean complete)
	{
		if (configuration.showTextNotify)
		{
			var questName = questService.Quests[selectedQuest!].Title;
			toastGui.ShowQuest($"{questName}", new QuestToastOptions
			{
				Position = QuestToastPosition.Centre,
				DisplayCheckmark = complete,
				IconId = 0, // Maybe usesful for the user if we wish for them to have custom Icons for their quests.
				PlaySound = complete,
			});
		}
	}

	private void QuestCompleted(string questId)
	{
		var questName = questService.Quests[questId].Title;
		toastGui.ShowQuest($"{questName}", new QuestToastOptions
		{
			Position = QuestToastPosition.Centre,
			DisplayCheckmark = true,
			IconId = 0, // Maybe usesful for the user if we wish for them to have custom Icons for their quests.
			PlaySound = true,
		});
	}
}
