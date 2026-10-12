// Compile-time API stub for Ravenfield's Assembly-CSharp (identity: Version=0.0.0.0,
// unsigned, global namespace).
//
// Why this exists: rf compiles against this handwritten surface snapshot instead of the
// game's real assembly, so the repository ships zero game IP. At runtime the CLR binds
// rf's Assembly-CSharp reference to the real game assembly loaded by BepInEx, because
// assembly name, version, culture and public key token match exactly.
//
// Rules when editing this file (violation => MissingFieldException/MissingMethodException
// at runtime while compilation still succeeds):
//  - declaring type must match the decompiled source (e.g. Actor.team lives on Hurtable);
//  - member kind must match (field vs property vs method): Weapon.user is a property;
//  - keep UnityEngine.Object-based inheritance chains so `x == null` on these types
//    compiles to UnityEngine.Object.op_Equality instead of a raw reference comparison;
//  - only members rf actually touches belong here; line refs are the decompiled ILSpy
//    output of ravenfield_Data/Managed/Assembly-CSharp.dll.

using System.Collections.Generic;
using UnityEngine;

// decompiled 26399: public class Hurtable : MonoBehaviour
public class Hurtable : MonoBehaviour
{
    // decompiled 26401: field, inherited by Actor
    public int team;
}

// decompiled 4894: public class Actor : Hurtable, ITriggerForEachSubject
// (interface omitted: rf never uses its members)
public class Actor : Hurtable
{
    // decompiled 5127/5139/5341: fields declared on Actor itself
    public float health;
    public bool dead;
    public bool aiControlled;

    // decompiled 6929/6971/6980: virtual methods on Actor itself
    public virtual Vector3 Position() { return default(Vector3); }
    public virtual Vector3 CenterPosition() { return default(Vector3); }
    public virtual Vector3 Velocity() { return default(Vector3); }
}

// decompiled 87144: public class Vehicle : MonoBehaviour, ITriggerForEachSubject
public class Vehicle : MonoBehaviour
{
    // decompiled 87534/87552: fields on Vehicle
    public int ownerTeam;
    public bool dead;

    // decompiled 88552/87685/88720
    public virtual Vector3 Velocity() { return default(Vector3); }
    public Vector3 GetWorldCenterOfMass() { return default(Vector3); }
    public bool AnySeatsTaken() { return false; }
}

// decompiled 31203: public class Weapon : MonoBehaviour, IPrefabContainer
public class Weapon : MonoBehaviour
{
    // decompiled 31279: nested [Serializable] class Configuration
    public class Configuration
    {
        // decompiled 31283
        public int ammo;
    }

    // decompiled 31520/31558/31580: plain fields
    public Configuration configuration;
    public int ammo;
    public float projectileSpeed;

    // decompiled 31724: PROPERTY (get/set over _user), not a field
    public Actor user { get; set; }

    // decompiled 32555
    public void OnAmmoChanged() { }
}

// decompiled 28944: public class Projectile : MonoBehaviour
public class Projectile : MonoBehaviour
{
    // decompiled 29034/29042: [NonSerialized] fields (hitMask/expireTime are reached
    // via reflection by name, so they need no declaration here)
    public Vector3 velocity;
    public Weapon sourceWeapon;
}

// decompiled 8749: public class ActorManager : MonoBehaviour
public class ActorManager : MonoBehaviour
{
    // decompiled 8845/8916/8910/8919: static + plain fields
    public static ActorManager instance;
    public Actor player;
    public List<Actor> actors;
    public List<Vehicle> vehicles;
}

// decompiled 45490: public class GameManager : MonoBehaviour
public class GameManager : MonoBehaviour
{
    // decompiled 45844
    public static Camera GetMainCamera() { return null; }
}
