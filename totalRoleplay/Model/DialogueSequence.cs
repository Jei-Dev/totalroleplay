namespace totalRoleplay.Model;

using System.Collections.Generic;

public record DialogueSequence
{
	public required DialogueLine[] Lines { get; init; }
}

public record DialogueLine
{
	public required string ActorName { get; init; }
	public required string Content { get; init; }
	public DialogueLineTrigger[] Triggers { get; init; } = {};
}

public record DialogueLineTrigger
{
	public required DialogueLineTriggerCondition When { get; init; }
	public required DialogueLineTriggerAction Then { get; init; }
}

public record DialogueLineTriggerCondition
{
	public required bool Closed { get; init; }
}

public record DialogueLineTriggerAction
{
	public QuestStateTriggerAction? Quest { get; init; }
}
