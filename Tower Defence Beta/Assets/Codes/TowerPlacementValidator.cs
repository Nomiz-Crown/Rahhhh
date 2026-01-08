using UnityEngine;

public class TowerPlacementValidator : MonoBehaviour
{
    public LayerMask blockedLayers;

    private int blockedContacts = 0;
    public bool CanPlace => blockedContacts == 0;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & blockedLayers) != 0)
        {
            blockedContacts++;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & blockedLayers) != 0)
        {
            blockedContacts--;
        }
    }
}
