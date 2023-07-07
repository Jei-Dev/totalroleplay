namespace totalRoleplay.Model;

using System;
using System.Collections.Generic;

public record Quest
{
	public required string Title { get; init; }
	public required string Description { get; init; }
	public required string InitialState { get; init; }
	public required Dictionary<string, QuestState> States { get; init; }
	public QuestStateTrigger[] StartTriggers { get; init; } = Array.Empty<QuestStateTrigger>();
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
	public InteractionTarget? InteractWithObject { get; init; }
	public string? Command { get; init; }
}

public record InteractionTarget
{
	public uint? DataId { get; init; }
	public PlayerReference? Player { get; init; }
}

public record PlayerReference
{
	public required string CharacterName { get; init; }
	public required string World { get; init; }
}

public record QuestStateTriggerAction
{
	public string? GoToState { get; init; }
	public string? BeginDialogueSequence { get; init; }
	public bool BeginQuest { get; init; }
	public bool FinishQuest { get; init; }
}
