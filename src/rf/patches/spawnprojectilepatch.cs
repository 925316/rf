using HarmonyLib;
using UnityEngine;

namespace rf;

[HarmonyPatch(typeof(Weapon), "SpawnProjectile")]
public static class SpawnProjectilePatch
{
    private static bool Prefix(Weapon __instance, ref Vector3 direction, Vector3 muzzlePosition)
    {
        if (!ShouldSteer(__instance))
        {
            return true;
        }

        LockTarget target = TargetLock.Instance.CurrentTarget;
        bool valid = target.IsValid;
        bool fov = valid && TargetLock.IsWithinFov(target);
        if (!valid || !fov)
        {
            return true;
        }

        Vector3 toTarget = target.AimPoint - muzzlePosition;
        if (toTarget.sqrMagnitude < 0.01f)
        {
            return true;
        }

        Vector3 desiredDir = ComputeDesiredDirection(toTarget, target, __instance, muzzlePosition);
        if (desiredDir.sqrMagnitude < 0.0001f)
        {
            return true;
        }

        direction = desiredDir.normalized;
        return true;
    }

    private static void Postfix(Weapon __instance, Projectile __result)
    {
        if (__result == null || !ShouldSteer(__instance) || !TargetLock.Instance.CurrentTarget.IsValid)
        {
            return;
        }

        HomingProjectile.Attach(__result, TargetLock.Instance.CurrentTarget, __instance);
    }

    // Every player-controlled weapon steers: handheld, mounted guns, vehicle turrets.
    // AI weapons are excluded so the mod only ever touches the player's own shots.
    private static bool ShouldSteer(Weapon weapon)
    {
        Actor actor = weapon.user;
        return actor != null && !actor.aiControlled;
    }

    private static Vector3 ComputeDesiredDirection(Vector3 toTarget, LockTarget target, Weapon weapon, Vector3 muzzlePosition)
    {
        float speed = weapon.projectileSpeed;
        if (speed <= 0.01f)
        {
            return toTarget.normalized;
        }

        float flightTime = toTarget.magnitude / speed * Settings.PredictTimeScale;
        Vector3 predicted = target.AimPoint + target.Velocity * flightTime;
        return predicted - muzzlePosition;
    }
}
