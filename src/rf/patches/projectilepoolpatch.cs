using HarmonyLib;
using UnityEngine;

namespace rf;

[HarmonyPatch(typeof(Projectile), "OnReturnedToPool")]
public static class ProjectilePoolPatch
{
    private static void Postfix(Projectile __instance)
    {
        HomingProjectile missile = __instance.GetComponent<HomingProjectile>();
        if (missile != null)
        {
            missile.Abort();
        }
    }
}
