using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    [Header("References")]
    public GameObject objectToActivate;

    private bool playerInTrigger = false;

    // Persistent pickup state
    public static bool pickedUp = false; // survives scene reload

    private const string PICKEDUP_KEY = "WrenchPickedUp";

    void Start()
    {
        if (pickedUp)
        {
            if (objectToActivate != null)
                objectToActivate.SetActive(false);

            gameObject.SetActive(false);
        }
    }


    void Update()
    {
        if (playerInTrigger && !pickedUp && Input.GetKeyDown(KeyCode.E))
        {
            PickUp();
        }
    }

    void PickUp()
    {
        pickedUp = true;

        if (objectToActivate != null)
            objectToActivate.SetActive(false);

        gameObject.SetActive(false);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !pickedUp)
        {
            playerInTrigger = true;

            if (objectToActivate != null)
                objectToActivate.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;

            if (objectToActivate != null)
                objectToActivate.SetActive(false);
        }
    }
}
