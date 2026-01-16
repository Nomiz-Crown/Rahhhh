using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public key keyManager; // assign the manager in Inspector script sak idfk

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            keyManager.keyCollected = true;
            gameObject.SetActive(false);
        }
    }
}
