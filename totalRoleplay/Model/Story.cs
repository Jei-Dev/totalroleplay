namespace totalRoleplay.Model;

using System.Collections.Generic;

public record Story
{
	public required Dictionary<string, Quest> Quests { get; init; }
	public required Dictionary<string, DialogueSequence> Dialogues { get; init; }
}
