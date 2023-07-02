using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace totalRoleplay.Windows;

public record Quest
{
    public required string Name { get; init; }
    public required string Description { get; init; }
}

public class QuestListWindow : Window, IDisposable
{
    private int selectedQuest = -1;

    public QuestListWindow(Plugin plugin) : base("TRP - Quests", 0)
    {
        this.Size = new Vector2(200, 200);
        this.SizeCondition = ImGuiCond.Once;
    }

    public void Dispose() { }

    public override void Draw()
    {
        Quest[] quests = {
            new Quest { Name = "quest 1", Description = "my first quest!" },
            new Quest { Name = "quest 2", Description = "my second quest!" },
            new Quest { Name = "quest 3", Description = "booooooo" }
        };
        string[] questNames = new string[quests.Length];
        for (int i = 0; i < quests.Length; ++i) {
            questNames[i] = quests[i].Name;
        }
        ImGui.ListBox("Quests", ref selectedQuest, questNames, quests.Length);

        if (selectedQuest >= 0) {
            ImGui.Text(quests[selectedQuest].Description);
        }
    }
}
