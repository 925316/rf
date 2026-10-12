using System.Reflection;
using UnityEngine;

namespace rf;

public sealed class HomingProjectile : MonoBehaviour
{
    // Actor hitbox layers 8,10,16 (Hitbox.IsHitboxLayer = 0x10500) + vehicle layer 12.
    // World geometry (layer 0 etc.) excluded: magic shells fly through walls
    // but still collide with enemies and vehicles.
    private const int MagicHitMask = 0x11500;

    private const float MagicLifetime = 8f;

    private static readonly FieldInfo HitMaskField =
        typeof(Projectile).GetField("hitMask", BindingFlags.Instance | BindingFlags.NonPublic);

    // expireTime is computed inside StartTravelling, which runs BEFORE the SpawnProjectile
    // postfix attaches this component, so the lifetime bump must write the field directly.
    private static readonly FieldInfo ExpireTimeField =
        typeof(Projectile).GetField("expireTime", BindingFlags.Instance | BindingFlags.NonPublic);

    private Projectile _projectile;
    private LockTarget _target;
    private Weapon _ownerWeapon;
    private float _projectileSpeed;
    private int _originalHitMask;
    private bool _magicApplied;
    private bool _aborted;

    public static HomingProjectile Attach(Projectile projectile, LockTarget target, Weapon weapon)
    {
        HomingProjectile missile = projectile.gameObject.GetComponent<HomingProjectile>();
        if (missile == null)
        {
            missile = projectile.gameObject.AddComponent<HomingProjectile>();
        }
        missile.Init(projectile, target, weapon);
        return missile;
    }

    public void Init(Projectile projectile, LockTarget target, Weapon weapon)
    {
        _projectile = projectile;
        _target = target;
        _ownerWeapon = weapon;
        _projectileSpeed = weapon != null ? weapon.projectileSpeed : 0f;
        _aborted = false;

        ApplyMagicCollision();
    }

    public void Abort()
    {
        _aborted = true;
    }

    private void ApplyMagicCollision()
    {
        if (_magicApplied || _projectile == null || HitMaskField == null)
        {
            return;
        }

        object stored = HitMaskField.GetValue(_projectile);
        if (stored is int mask)
        {
            _originalHitMask = mask;
            HitMaskField.SetValue(_projectile, MagicHitMask);
            _magicApplied = true;
        }

        if (ExpireTimeField != null)
        {
            ExpireTimeField.SetValue(_projectile, Time.time + MagicLifetime);
        }
    }

    private void ReleaseMagicCollision()
    {
        if (!_magicApplied)
        {
            return;
        }
        _magicApplied = false;

        if (_projectile == null)
        {
            return;
        }

        if (HitMaskField != null)
        {
            HitMaskField.SetValue(_projectile, _originalHitMask);
        }
    }

    private void Update()
    {
        if (_aborted || _projectile == null)
        {
            Disarm();
            return;
        }

        // Pooled projectiles get reused by other weapons (including AI fire).
        // A stale component from a previous shot must never steer those.
        if (_ownerWeapon == null || _projectile.sourceWeapon != _ownerWeapon)
        {
            Disarm();
            return;
        }

        if (!_target.IsValid || _projectile.velocity == Vector3.zero)
        {
            Disarm();
            return;
        }

        Vector3 desired = ComputeAimPoint(transform.position);
        Vector3 toDesired = desired - transform.position;
        if (toDesired.sqrMagnitude < 0.01f)
        {
            Disarm();
            return;
        }

        Vector3 desiredDir = toDesired.normalized;
        float speed = _projectile.velocity.magnitude;
        Vector3 currentDir = _projectile.velocity.normalized;

        float maxRadians = Settings.TurnRate * Mathf.Deg2Rad * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(currentDir, desiredDir, maxRadians, 0f);

        _projectile.velocity = newDir * speed;
        _projectile.transform.rotation = Quaternion.LookRotation(newDir);
    }

    private void Disarm()
    {
        ReleaseMagicCollision();
        Destroy(this);
    }

    private void OnDisable()
    {
        // Safety net: projectile returned to its pool without going through Disarm.
        ReleaseMagicCollision();
    }

    private Vector3 ComputeAimPoint(Vector3 from)
    {
        Vector3 targetPos = _target.AimPoint;
        float distance = Vector3.Distance(from, targetPos);
        float flightTime = distance / Mathf.Max(_projectileSpeed, 1f) * Settings.PredictTimeScale;
        return targetPos + _target.Velocity * flightTime;
    }
}
