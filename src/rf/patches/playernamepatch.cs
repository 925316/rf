using HarmonyLib;

namespace rf;

// GetPlayerName is the single source of the local player's display name
// (the killfeed "{player}" tag resolves through it live; the scoreboard reads
// the actor.name it assigns on spawn), so replacing it covers every surface.
// Nothing compares against the returned string, and the campaign hero-squad
// pose overwrites vanilla names the same way -- zero side effects.
[HarmonyPatch(typeof(GameManager), "GetPlayerName")]
public static class PlayerNamePatch
{
    private static bool Prefix(ref string __result)
    {
        __result = Settings.PlayerName;
        return false;
    }
}
