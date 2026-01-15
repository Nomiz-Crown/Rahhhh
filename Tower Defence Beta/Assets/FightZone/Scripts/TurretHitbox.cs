using UnityEngine;

public class TurretHitbox : MonoBehaviour
{
    private turret turret;

    void Awake()
    {
        turret = GetComponentInParent<turret>();
    }

    void OnMouseDown()
    {
        turret?.OnHitboxClicked();
    }
}
