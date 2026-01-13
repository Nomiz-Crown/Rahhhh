using UnityEngine;
using UnityEngine.UI;

public class inventory : MonoBehaviour
{
    [Header("UI References")]
    public GameObject TurretIcon;

    [Header("Inventory Flags")]
    public static bool turret = false; // survives scene reload, resets on play stop

    void Start()
    {
        // Set UI state based on turret flag
        if (TurretIcon != null)
            TurretIcon.SetActive(turret);
    }

    // Call this from other scripts when player acquires turret
    public void AcquireTurret()
    {
        turret = true;

        if (TurretIcon != null)
            TurretIcon.SetActive(true);
    }
}
