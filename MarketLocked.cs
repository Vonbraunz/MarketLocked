using System.IO;
using System.Reflection;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Helpers;
using SPTarkov.Server.Core.Models.Spt.Mod;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Services;

namespace MarketLocked;

[Injectable(TypePriority = OnLoadOrder.PostDBModLoader + 2)]
public class MarketLockedMod(
    ISptLogger<MarketLockedMod> logger,
    ModHelper modHelper,
    DatabaseService databaseService,
    WTTServerCommonLib.WTTServerCommonLib wttCommon
) : IOnLoad
{
    public async Task OnLoad()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var modPath = modHelper.GetAbsolutePathToModFolder(assembly);
        var config = modHelper.GetJsonDataFromFile<ModConfig>(Path.Combine(modPath, "config"), "config.json");

        if (!config.Enabled)
        {
            logger.Warning("[MarketLocked] Disabled via config.json — skipping.");
            return;
        }

        await wttCommon.CustomQuestService.CreateCustomQuests(assembly);
        await wttCommon.CustomLocaleService.CreateCustomLocales(assembly);
        await wttCommon.CustomQuestZoneService.CreateCustomQuestZones(assembly);

        logger.Info("[MarketLocked] Quests, locales, and zones loaded.");

        var ragFair = databaseService.GetGlobals().Configuration.RagFair;

        if (FleaUnlockHelper.FindProfileThatCompletedQuest(config.FinalQuestId, logger) is string profileName)
        {
            ragFair.MinUserLevel = config.UnlockedFleaLevel;
            logger.Success($"[MarketLocked] Profile \"{profileName}\" has completed the quest chain — flea market is UNLOCKED.");
        }
        else
        {
            ragFair.MinUserLevel = config.LockedFleaLevel;
            logger.Success($"[MarketLocked] No profiles have completed the quest chain — flea market is LOCKED (level {config.LockedFleaLevel}).");
        }
    }
}
