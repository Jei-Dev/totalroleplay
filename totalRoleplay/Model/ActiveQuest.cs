namespace totalRoleplay.Model;

public record ActiveQuest
{
    public required string QuestId { get; init; }
    public required string CurrentState { get; set; }
}
