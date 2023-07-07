using Dalamud;
using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.IoC;
using Dalamud.Logging;
using Dalamud.Plugin;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using totalRoleplay.Handlers;
using totalRoleplay.Model;

namespace totalRoleplay.Service;

[PluginInterface]
public class QuestService : IServiceType
{
	public Dictionary<string, Quest> Quests { get; init; }
	public List<ActiveQuest> ActiveQuests { get; } = new List<ActiveQuest>();

	public Dictionary<string, DialogueSequence> Dialogues { get; init; }

	public delegate void OnQuestComplete(string questId);
	public event OnQuestComplete? QuestComplete;

	private readonly FakeDialogueHandler dialogueHandler;

	public QuestService(DalamudPluginInterface pluginInterface, FakeDialogueHandler dialogueHandler)
	{
		var questJsonPath = Path.Join(pluginInterface.AssemblyLocation.Directory!.FullName, "quests.json");
		PluginLog.LogInformation(questJsonPath);
		var questJson = File.ReadAllText(questJsonPath);
		var story = JsonSerializer.Deserialize<Story>(
			questJson,
			new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }
		)!;
		Quests = story.Quests;
		Dialogues = story.Dialogues;
		this.dialogueHandler = dialogueHandler;
	}

	public void BeginQuest(string questId)
	{
		ActiveQuests.Add(new ActiveQuest
		{
			QuestId = questId,
			CurrentState = Quests[questId].InitialState,
			IsTracked = false,
			GoalCurrent = 0,
			GoalFinal = 5
		});
	}

	public void TriggerCommand(string cmd)
	{
		foreach (var aq in ActiveQuests)
		{
			var quest = Quests[aq.QuestId];
			var state = quest.States[aq.CurrentState];
			foreach (var trigger in state.Triggers)
			{
				if (trigger.When.Command == cmd)
				{
					ExecuteTriggerActions(aq.QuestId, trigger.Then);
				}
			}
		}
	}

	private (string, QuestStateTriggerAction)? GetInteractionTriggerForTarget(GameObject target)
	{
		foreach (var activeQuest in ActiveQuests)
		{
			var quest = Quests[activeQuest.QuestId];
			var state = quest.States[activeQuest.CurrentState];
			foreach (var trigger in state.Triggers)
			{
				var targetCond = trigger.When.InteractWithObject;
				if (targetCond?.DataId != null && targetCond.DataId == target.DataId)
				{
					return (activeQuest.QuestId, trigger.Then);
				}
				if (targetCond?.Player != null && target is PlayerCharacter player)
				{

					if (
						player.Name.ToString() == targetCond.Player.CharacterName &&
						player.HomeWorld.GameData?.Name.ToString() == targetCond.Player.World
					)
					{
						return (activeQuest.QuestId, trigger.Then);
					}
				}
			}
		}
		return null;
	}

	public bool CanInteractWithTarget(GameObject target)
	{
		return GetInteractionTriggerForTarget(target) != null;
	}

	public void InteractWithTarget(GameObject target)
	{
		var trigger = GetInteractionTriggerForTarget(target);
		if (trigger.HasValue)
		{
			ExecuteTriggerActions(trigger.Value.Item1, trigger.Value.Item2);
		}
		else
		{
			PluginLog.LogWarning("Target has no dialogue.");
		}
	}

	private void ExecuteTriggerActions(string questId, QuestStateTriggerAction action)
	{
		if (action.GoToState != null)
		{
			ActiveQuests.FindAll(aq => aq.QuestId == questId).ForEach(aq => aq.CurrentState = action.GoToState);
		}
		if (action.BeginDialogueSequence != null)
		{
			dialogueHandler.startFakeDialogue(
				Dialogues[action.BeginDialogueSequence],
				questActionTriggerHandler: action => ExecuteTriggerActions(questId, action)
			);
		}
		if (action.FinishQuest)
		{
			if (QuestComplete != null)
			{
				QuestComplete(questId);
			}
			ActiveQuests.RemoveAll(aq => aq.QuestId == questId);
		}
	}
}
