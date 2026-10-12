using System.Collections.Generic;
using UnityEngine;

namespace rf;

public struct LockTarget
{
    public Actor Actor;
    public Vehicle Vehicle;

    public static LockTarget FromActor(Actor actor)
    {
        return new LockTarget { Actor = actor };
    }

    public static LockTarget FromVehicle(Vehicle vehicle)
    {
        return new LockTarget { Vehicle = vehicle };
    }

    public bool IsValid
    {
        get
        {
            if (Actor != null)
            {
                return !Actor.dead && Actor.health > 0f;
            }
            return Vehicle != null && !Vehicle.dead;
        }
    }

    public Vector3 AimPoint
    {
        get
        {
            if (Actor != null)
            {
                return Actor.CenterPosition();
            }
            return Vehicle != null ? Vehicle.GetWorldCenterOfMass() : Vector3.zero;
        }
    }

    public Vector3 Velocity
    {
        get
        {
            if (Actor != null)
            {
                return Actor.Velocity();
            }
            return Vehicle != null ? Vehicle.Velocity() : Vector3.zero;
        }
    }

    public Vector3 ScreenPoint
    {
        get
        {
            Camera camera = SafeCamera.Get();
            return camera != null ? camera.WorldToScreenPoint(AimPoint) : Vector3.zero;
        }
    }

    public string Label
    {
        get
        {
            if (Actor != null)
            {
                return Actor.aiControlled ? "Ped Enemy" : "Ped Player";
            }
            if (Vehicle != null)
            {
                return "Vehicle " + Vehicle.name.Replace("(Clone)", "");
            }
            return "None";
        }
    }
}

public sealed class TargetLock
{
    public static readonly TargetLock Instance = new TargetLock();

    public LockTarget CurrentTarget { get; private set; }

    private float _nextEvaluateTime;

    public float NextEvaluateIn()
    {
        return _nextEvaluateTime - Time.time;
    }

    public void Tick()
    {
        if (ActorManager.instance == null)
        {
            CurrentTarget = default(LockTarget);
            return;
        }

        Actor player = ActorManager.instance.player;
        if (player == null || player.dead)
        {
            CurrentTarget = default(LockTarget);
            return;
        }

        if (CurrentTarget.IsValid && IsStillEligible(CurrentTarget))
        {
            return;
        }

        CurrentTarget = default(LockTarget);

        bool skipByTime = Time.time < _nextEvaluateTime;
        if (skipByTime)
        {
            return;
        }
        _nextEvaluateTime = Time.time + 0.05f;

        CurrentTarget = FindBestTarget();
    }

    public static bool IsWithinFov(LockTarget target)
    {
        return PassesAnyCamera(target);
    }

    public float DistanceTo(LockTarget target)
    {
        Actor player = ActorManager.instance.player;
        if (player == null)
        {
            return 0f;
        }
        return Vector3.Distance(player.Position(), target.AimPoint);
    }

    private static bool IsStillEligible(LockTarget target)
    {
        return PassesAnyCamera(target);
    }

    // The managed resolver can return a stray camera (up-tilted "Camera" object
    // seen bouncing in logs), so a target passes if ANY candidate camera's cone
    // contains it. A wrong camera may only add locks, never veto them.
    private static bool PassesAnyCamera(LockTarget candidate)
    {
        Camera camera = SafeCamera.Get();
        if (camera != null && PassesFilters(candidate, camera.transform.position, camera.transform.forward))
        {
            return true;
        }
        camera = SafeCamera.GetMain();
        return camera != null && PassesFilters(candidate, camera.transform.position, camera.transform.forward);
    }

    private static LockTarget FindBestTarget()
    {
        float bestScore = float.PositiveInfinity;
        LockTarget best = default(LockTarget);

        ScanWithCamera(SafeCamera.Get(), ref bestScore, ref best);
        ScanWithCamera(SafeCamera.GetMain(), ref bestScore, ref best);

        return best;
    }

    private static void ScanWithCamera(Camera camera, ref float bestScore, ref LockTarget best)
    {
        if (camera == null)
        {
            return;
        }

        Vector3 origin = camera.transform.position;
        Vector3 forward = camera.transform.forward;
        int playerTeam = ActorManager.instance.player.team;

        List<Actor> actors = ActorManager.instance.actors;
        for (int i = 0; i < actors.Count; i++)
        {
            Actor actor = actors[i];
            if (actor == null || actor.dead || actor.health <= 0f || actor.team == playerTeam)
            {
                continue;
            }
            ScoreCandidate(LockTarget.FromActor(actor), origin, forward, ref bestScore, ref best);
        }

        List<Vehicle> vehicles = ActorManager.instance.vehicles;
        for (int i = 0; i < vehicles.Count; i++)
        {
            Vehicle vehicle = vehicles[i];
            if (vehicle == null || vehicle.dead || vehicle.ownerTeam == playerTeam || !vehicle.AnySeatsTaken())
            {
                continue;
            }
            ScoreCandidate(LockTarget.FromVehicle(vehicle), origin, forward, ref bestScore, ref best);
        }
    }

    private static void ScoreCandidate(LockTarget candidate, Vector3 origin, Vector3 forward, ref float bestScore, ref LockTarget best)
    {
        if (!PassesFilters(candidate, origin, forward))
        {
            return;
        }

        Vector3 toTarget = candidate.AimPoint - origin;
        float dot = Vector3.Dot(forward, toTarget.normalized);
        float score = (1f - dot) * 1000f + toTarget.magnitude;
        if (score < bestScore)
        {
            bestScore = score;
            best = candidate;
        }
    }

    private static bool PassesFilters(LockTarget candidate, Vector3 origin, Vector3 forward)
    {
        Vector3 toTarget = candidate.AimPoint - origin;

        float fovLimit = Settings.FieldOfViewLimit;
        if (fovLimit >= 360f)
        {
            return true;
        }

        float dot = Vector3.Dot(forward, toTarget.normalized);
        return dot >= Mathf.Cos(fovLimit * Mathf.Deg2Rad * 0.5f);
    }
}
