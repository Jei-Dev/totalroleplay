using Dalamud;
using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.IoC;
using Dalamud.Logging;
using Dalamud.Plugin;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using totalRoleplay.Handlers;
using totalRoleplay.Model;

namespace totalRoleplay.Service;

[PluginInterface]
public class QuestService : IServiceType
{
	public Dictionary<string, Quest> Quests { get; init; }
	public List<ActiveQuest> ActiveQuests { get; } = new List<ActiveQuest>();
	private Dictionary<string, bool> CompletedQuests { get; init; } = new Dictionary<string, bool>();

	private Dictionary<string, DialogueSequence> Dialogues { get; init; }

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

	private IEnumerable<(string, QuestStateTrigger)> ActiveQuestTriggers =>
		Quests
		.Where(quest => !CompletedQuests.ContainsKey(quest.Key))
		.SelectMany(quest =>
		{
			var activeQuest = ActiveQuests.Find(aq => aq.QuestId == quest.Key);
			QuestStateTrigger[] triggers = activeQuest == null ? quest.Value.StartTriggers : quest.Value.States[activeQuest.CurrentState].Triggers;
			return triggers.Select(trigger => (quest.Key, trigger));
		});

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
		foreach (var (questId, trigger) in ActiveQuestTriggers)
		{
			if (trigger.When.Command == cmd)
			{
				ExecuteTriggerActions(questId, trigger.Then);
			}
		}
	}

	private (string, QuestStateTriggerAction)? GetInteractionTriggerForTarget(GameObject target)
	{
		foreach (var (questId, trigger) in ActiveQuestTriggers)
		{
			var targetCond = trigger.When.InteractWithObject;
			if (targetCond?.DataId != null && targetCond.DataId == target.DataId)
			{
				return (questId, trigger.Then);
			}
			if (targetCond?.Player != null && target is PlayerCharacter player)
			{

				if (
					player.Name.ToString() == targetCond.Player.CharacterName &&
					player.HomeWorld.GameData?.Name.ToString() == targetCond.Player.World
				)
				{
					return (questId, trigger.Then);
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
		if (action.BeginQuest)
		{
			BeginQuest(questId);
		}
		if (action.FinishQuest)
		{
			if (QuestComplete != null)
			{
				QuestComplete(questId);
			}
			ActiveQuests.RemoveAll(aq => aq.QuestId == questId);
			CompletedQuests[questId] = true;
		}
	}
}
