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
	private int selectedQuest = -1;
	private Quest[] quests = {
			new Quest { Name = "quest 1", Description = "my first quest!", goalFinal = 3},
			new Quest { Name = "quest 2", Description = "my second quest!", goalFinal = 10},
			new Quest { Name = "quest 3", Description = "booooooo", goalFinal = 2 }
		};

	public QuestListWindow(Plugin plugin) : base("TRP - Quests", 0)
	{
		this.Size = new Vector2(200, 200);
		this.SizeCondition = ImGuiCond.Once;
	}

	public void Dispose() { }

	public override void Draw()
	{
		ImGui.Text("Quests");
		if (ImGui.BeginListBox("##quests", new Vector2(float.Epsilon, ImGui.GetTextLineHeightWithSpacing() * quests.Length + ImGui.GetStyle().FramePadding.Y)))
		{
			for (int i = 0; i < quests.Length; ++i)
			{
				if (ImGui.Selectable(quests[i].Name, selectedQuest == i))
				{
					selectedQuest = i;
				}
				var quSelect = quests[i].questSelected;
				var gCurr = quests[i].goalCurrent;
				var gNext = quests[i].goalFinal;
				ImGui.Checkbox($"{gCurr}/{gNext}", ref quSelect);
				if (quSelect && !quests[i].questSelected && (quests[i].goalCurrent == quests[i].goalFinal))
				{
					showQuestToast(true);
				}
				quests[i].questSelected = quSelect;
			}
			ImGui.EndListBox();
		}

		if (selectedQuest >= 0)
		{
			ImGui.Text(quests[selectedQuest].Description);
		}
	}
	public void IncrementCurrentQuestGoal()
	{
		quests[selectedQuest].goalCurrent++;
	}
	/// <summary>
	/// Shows Quest Toast
	/// </summary>
	public void showQuestToast(Boolean complete)
	{
		var canShow = Service.pluginConfig.showTextNotify;
		if (canShow)
		{
			var questName = quests[selectedQuest].Name;
			Service.toastGui?.ShowQuest($"{questName}", new QuestToastOptions
			{
				Position = QuestToastPosition.Centre,
				DisplayCheckmark = complete,
				IconId = 0, // Maybe usesful for the user if we wish for them to have custom Icons for their quests.
				PlaySound = complete,
			});
		}
	}
}
