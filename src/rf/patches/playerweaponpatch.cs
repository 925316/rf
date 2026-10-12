using System.Reflection;
using HarmonyLib;

namespace rf;

// Player weapon behavior patches, applied from a single Postfix on base Weapon.Shoot.
// 1. Infinite ammo: the only magazine consumption in the whole game is the "ammo--"
//    inside base Shoot. Refilling the magazine right after the shot means it never
//    empties, the auto reload never triggers, so spareAmmo is never consumed either.
//    Covers every weapon type -- handheld, single-shot launchers (RPG), mounted guns,
//    vehicle turrets and grenades all funnel through base Shoot.
// 2. Hold-to-auto: the semi-auto gate in CanFire is
//    "configuration.auto || !hasFiredSingleRoundThisTrigger". Clearing that latch
//    after every shot lets any weapon fire continuously while the trigger is held,
//    limited by its own cooldown -- the native rate of fire, nothing is sped up.
// Weapons with configuration.ammo == -1 (infinite by design) and melee /
// AutoRepairVehicleWeapon / CarHorn (no decrement, some skip base Shoot) are safe.
// Fire rate, overheat, charge weapons and reload animations stay vanilla.
// Player only; AI weapons keep consuming ammo and firing normally.
[HarmonyPatch(typeof(Weapon), "Shoot")]
public static class PlayerWeaponPatch
{
    private static readonly FieldInfo SemiAutoLatchField = typeof(Weapon).GetField(
        "hasFiredSingleRoundThisTrigger",
        BindingFlags.Instance | BindingFlags.NonPublic);

    private static void Postfix(Weapon __instance)
    {
        Actor actor = __instance.user;
        if (actor == null || actor.aiControlled)
        {
            return;
        }

        int magazineSize = __instance.configuration.ammo;
        if (magazineSize > 0 && __instance.ammo != magazineSize)
        {
            __instance.ammo = magazineSize;
            __instance.OnAmmoChanged();
        }

        if (SemiAutoLatchField != null)
        {
            SemiAutoLatchField.SetValue(__instance, false);
        }
    }
}
