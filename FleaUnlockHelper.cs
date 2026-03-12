using System.IO;
using System.Text.Json;
using SPTarkov.Server.Core.Models.Utils;

namespace MarketLocked;

public static class FleaUnlockHelper
{
    public static string? FindProfileThatCompletedQuest(string questId, ISptLogger<MarketLockedMod> logger)
    {
        try
        {
            var profilesDir = Path.Combine(Directory.GetCurrentDirectory(), "user", "profiles");

            if (!Directory.Exists(profilesDir))
                return null;

            foreach (var file in Directory.GetFiles(profilesDir, "*.json"))
            {
                try
                {
                    var json = File.ReadAllText(file);
                    using var doc = JsonDocument.Parse(json);
                    var root = doc.RootElement;

                    var nickname = root
                        .GetProperty("characters")
                        .GetProperty("pmc")
                        .GetProperty("Info")
                        .GetProperty("Nickname")
                        .GetString() ?? Path.GetFileNameWithoutExtension(file);

                    var quests = root
                        .GetProperty("characters")
                        .GetProperty("pmc")
                        .GetProperty("Quests");

                    foreach (var quest in quests.EnumerateArray())
                    {
                        var qid = quest.TryGetProperty("qid", out var qidProp) ? qidProp.GetString()
                                : quest.TryGetProperty("QuestId", out var qidProp2) ? qidProp2.GetString()
                                : null;

                        var status = quest.TryGetProperty("status", out var statusProp) ? statusProp.GetRawText()
                                   : quest.TryGetProperty("Status", out var statusProp2) ? statusProp2.GetRawText()
                                   : null;

                        if (qid == questId && status is "4" or "\"Success\"" or "\"success\"")
                            return nickname;
                    }
                }
                catch (Exception ex)
                {
                    logger.Warning($"[MarketLocked] Failed to parse profile {Path.GetFileName(file)}: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            logger.Warning($"[MarketLocked] Exception reading profiles: {ex.Message}");
        }

        return null;
    }
}
