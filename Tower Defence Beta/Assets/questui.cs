
using UnityEngine;

public class questUI : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            questmanager.instance.ToggleShop();
        }



    }
    private void OnTriggerExit2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            questmanager.instance.ToggleShop();
        }



    }
}
