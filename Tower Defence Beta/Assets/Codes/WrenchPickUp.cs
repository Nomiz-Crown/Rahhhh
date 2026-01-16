using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrenchPickUp : MonoBehaviour
{
    private bool playerInRange = false;

    void Update()
    {
        if(playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Engineer.instance.playerHasWrench = true;
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player"))
          playerInRange = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
           playerInRange = false;
    }
}

