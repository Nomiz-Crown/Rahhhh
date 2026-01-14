using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrenchPickUp : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E)) 
        {
            Engineer.instance.playerHasWrench = true;
            Destroy(gameObject);
        }
    }
}
