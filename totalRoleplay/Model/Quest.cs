namespace totalRoleplay.Model;

using System.Collections.Generic;

public record Quest
{
	public required string Title { get; init; }
	public required string Description { get; init; }
	public required string InitialState { get; init; }
	public required Dictionary<string, QuestState> States { get; init; }
}

public record QuestState
{
	public required QuestStateTrigger[] Triggers { get; init; }
}

public record QuestStateTrigger
{
	public required QuestStateTriggerCondition When { get; init; }
	public required QuestStateTriggerAction Then { get; init; }
}

public record QuestStateTriggerCondition
{
	public uint? InteractWithObject { get; init; }
	public string? Command { get; init; }
}

public record QuestStateTriggerAction
{
	public string? GoToState { get; init; }
	public string? BeginDialogueSequence { get; init; }
	public bool FinishQuest { get; init; }
}
