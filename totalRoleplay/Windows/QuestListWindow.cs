using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using Dalamud.Logging;
using ImGuiNET;

namespace totalRoleplay;

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
        ImGui.Text("Quests");
        if (ImGui.BeginListBox("##quests", new Vector2(float.Epsilon, ImGui.GetTextLineHeightWithSpacing() * quests.Length + ImGui.GetStyle().FramePadding.Y)))
        {
            for (int i = 0; i < quests.Length; ++i)
            {
                if (ImGui.Selectable(quests[i].Name, selectedQuest == i))
                {
                    selectedQuest = i;
                }
            }
            ImGui.EndListBox();
        }

        if (selectedQuest >= 0)
        {
            ImGui.Text(quests[selectedQuest].Description);
        }
    }
}
