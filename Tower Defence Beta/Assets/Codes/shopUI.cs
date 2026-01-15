
using UnityEngine;

public class shopUI : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
     if (other.CompareTag("Player"))
        {
            shopmanager.instance.ToggleShop();
        }
        

        
    }
    private void OnTriggerExit2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            shopmanager.instance.ToggleShop();
        }
        
            
        
    }
}
