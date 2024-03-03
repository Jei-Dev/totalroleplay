namespace chroniclePlugin.Model;

public record ActiveQuest
{
    public required string QuestId { get; init; }
    public required string CurrentState { get; set; }
    public required bool IsTracked { get; set; }
    public required int GoalCurrent { get; set; }
    public required int GoalFinal { get; init; }
}
