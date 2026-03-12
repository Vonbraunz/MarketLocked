namespace MarketLocked;

public record ModConfig
{
    public bool Enabled { get; set; } = true;
    public int LockedFleaLevel { get; set; } = 99;
    public int UnlockedFleaLevel { get; set; } = 1;

    // Must match the _id of the final quest in Q5_TheBuyIn.json
    public string FinalQuestId { get; set; } = "66c8b497a756f800b530a071";
}
