# MarketLocked

A server mod for **SPT 4.0.13** that locks the flea market behind a 5-quest chain obtained from **Fence**. Players must earn access to the flea market through gameplay rather than having it available from the start.

---

## Features

- Locks the flea market on server start by setting the minimum user level requirement to 99
- Adds a 5-quest chain from Fence that must be completed in order to unlock the flea
- On server start, automatically detects if any profile has already completed the chain and unlocks the flea accordingly — no manual config changes needed
- Fully configurable via `config.json`

---

## Requirements

- [SPT 4.0.13](https://www.sp-tarkov.com/)
- [WTT-ServerCommonLib](https://github.com/WhosYourDaddy-SPT/WTT-ServerCommonLib) *(must be installed and loaded before this mod)*

---

## Installation

1. Download the latest release
2. Extract the `MarketLocked` folder into `SPT/user/mods/`
3. Ensure **WTT-ServerCommonLib** is also installed in `user/mods/`
4. Start the server

Your folder structure should look like:
```
SPT/
└── user/
    └── mods/
        ├── WTT-ServerCommonLib/
        └── MarketLocked/
            ├── MarketLocked.dll
            ├── config/
            │   └── config.json
            └── db/
                ├── CustomQuests/
                ├── CustomLocales/
                └── CustomQuestZones/
```

---

## Quest Chain

All quests are given by **Fence** and must be completed in order.

| # | Quest | Objective Summary |
|---|-------|-------------------|
| 1 | **Establishing Connections** | Leave items at 12 Fence drop zones across multiple maps |
| 2 | **Prove Your Loyalty** | Survive 3 raids without killing scavs + reach Fence rep 3.0 |
| 3 | **Way Too Much Loot** | Be overencumbered for 5 minutes then extract |
| 4 | **Clearing Out The Competition** | Kill 3 PMCs on each of 10 different maps |
| 5 | **The Buy In** | Hand over 10 million roubles + stash 3 bitcoins at a Woods drop zone |

Once **The Buy In** is turned in, restart the server. The flea market will be unlocked permanently for all profiles on that server.

---

## Configuration

Located at `user/mods/MarketLocked/config/config.json`:

```json
{
    "Enabled": true,
    "LockedFleaLevel": 99,
    "UnlockedFleaLevel": 1,
    "FinalQuestId": "66c8b497a756f800b530a071"
}
```

| Field | Default | Description |
|-------|---------|-------------|
| `Enabled` | `true` | Set to `false` to disable the mod entirely |
| `LockedFleaLevel` | `99` | Minimum level required to access flea while locked (effectively unreachable) |
| `UnlockedFleaLevel` | `1` | Minimum level set after the quest chain is completed |
| `FinalQuestId` | `66c8b497...` | Internal ID of the final quest — do not change unless you know what you're doing |

---

## Server Log Messages

On every server start you will see one of the following:

```
[MarketLocked] Profile "YourName" has completed the quest chain — flea market is UNLOCKED.
```
```
[MarketLocked] No profiles have completed the quest chain — flea market is LOCKED (level 99).
```

---

## How It Works

On server start, MarketLocked reads all profile JSON files from `user/profiles/` and checks whether any profile has completed the final quest in the chain. If found, it sets `RagFair.MinUserLevel = 1`, unlocking the flea market globally. If not found, it sets `MinUserLevel = 99`, effectively locking it.

After completing the quest chain in-game, simply **restart the server** to unlock the flea market.

---

## Compatibility

- Built for **SPT 4.0.13**
- Compatible with other mods that do not modify `RagFair.MinUserLevel`
- Not compatible with mods that replace or override the Fence quest pool in a conflicting way

---

## License

MIT
