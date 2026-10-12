using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace rf;

[BepInPlugin("com.rf.aimbot", "rf", "1.0.0")]
public class Main : BaseUnityPlugin
{
    internal static ManualLogSource Log;
    private Harmony _harmony;

    private void Awake()
    {
        Log = Logger;

        _harmony = new Harmony("com.rf.aimbot");
        _harmony.PatchAll(typeof(SpawnProjectilePatch).Assembly);

        Log.LogInfo("rf loaded. Auto-lock active.");
    }

    private void Update()
    {
        TargetLock.Instance.Tick();
    }

    private void OnGUI()
    {
        Overlay.Instance.Draw();
    }
}
