using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrenchPickUp : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            WrenchQuestManager.instance.hasWrench = true;
            Destroy(gameObject);
        }
    }
}
