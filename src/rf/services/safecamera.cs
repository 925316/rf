namespace rf;

// The game's GetMainManagedCamera dereferences SpectatorCamera.instance without
// a null check (NRE in early menu / spectate transitions) -- never call it raw.
public static class SafeCamera
{
    public static UnityEngine.Camera Get()
    {
        try
        {
            return GameManager.GetMainCamera();
        }
        catch (System.Exception)
        {
            return null;
        }
    }
}
