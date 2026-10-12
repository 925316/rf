namespace rf;

// GameManager.GetMainManagedCamera throws NRE during loading and controller teardown.
public static class SafeCamera
{
    public static UnityEngine.Camera Get()
    {
        try
        {
            UnityEngine.Camera camera = LocalPlayer.GetCurrentCameraIncludeSpectator();
            if (camera != null)
            {
                return camera;
            }
        }
        catch (System.Exception)
        {
        }

        try
        {
            return GameManager.GetMainCamera();
        }
        catch (System.Exception)
        {
            return null;
        }
    }

    public static UnityEngine.Camera GetMain()
    {
        try
        {
            return UnityEngine.Camera.main;
        }
        catch (System.Exception)
        {
            return null;
        }
    }
}
