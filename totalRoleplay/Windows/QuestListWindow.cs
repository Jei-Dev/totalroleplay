using Dalamud.Game.Gui.Toast;
using Dalamud.Interface.Windowing;
using Dalamud.Logging;
using ImGuiNET;
using System;
using System.Numerics;

namespace totalRoleplay;

public record Quest
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public int goalCurrent { get; set; }
    public int goalFinal { get; init; }
    public Boolean questSelected { get; set; } = false;
};

public class QuestListWindow : Window, IDisposable
{
    private string? selectedQuest;

    public QuestListWindow(Plugin plugin) : base("TRP - Quests", 0)
    {
        this.Size = new Vector2(200, 200);
        this.SizeCondition = ImGuiCond.Once;
        Service.questService.QuestComplete += QuestCompleted;
    }

    public void Dispose()
    {
        Service.questService.QuestComplete -= QuestCompleted;
    }

    public override void Draw()
    {
        ImGui.Text("Quests");
        if (ImGui.BeginListBox("##quests", new Vector2(float.Epsilon, ImGui.GetTextLineHeightWithSpacing() * Service.questService.ActiveQuests.Count * 2 + ImGui.GetStyle().FramePadding.Y)))
        {
            foreach (var activeQuest in Service.questService.ActiveQuests)
            {
                var quest = Service.questService.Quests[activeQuest.QuestId];
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
            ImGui.Text(Service.questService.Quests[selectedQuest].Description);
        }
    }
    public void IncrementCurrentQuestGoal()
    {
        Service.questService.ActiveQuests
            .FindAll(aq => aq.QuestId == selectedQuest)
            .ForEach(aq => aq.GoalCurrent++);
    }
    /// <summary>
    /// Shows Quest Toast
    /// </summary>
    public void showQuestToast(Boolean complete)
    {
        var canShow = Service.pluginConfig.showTextNotify;
        if (canShow)
        {
            var questName = Service.questService.Quests[selectedQuest!].Title;
            Service.toastGui?.ShowQuest($"{questName}", new QuestToastOptions
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
        var questName = Service.questService.Quests[questId].Title;
        Service.toastGui?.ShowQuest($"{questName}", new QuestToastOptions
        {
            Position = QuestToastPosition.Centre,
            DisplayCheckmark = true,
            IconId = 0, // Maybe usesful for the user if we wish for them to have custom Icons for their quests.
            PlaySound = true,
        });
    }
}
