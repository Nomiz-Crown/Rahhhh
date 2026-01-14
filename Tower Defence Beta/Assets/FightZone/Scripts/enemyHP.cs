using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyHP : MonoBehaviour
{
    [Header("Enemy Health")]
    public int health = 3;

   
    [Header("HP Display")]
     public GameObject hp1;
     public GameObject hp2;
    public GameObject hp3;

    [Header("Death Clone")]
    public GameObject deathPrefab;   // Prefab with animation
    public float deathAnimLength = 1f; // Length of animation (seconds)

    private void Start()
    {
        if (hp1 == null) hp1 = transform.Find("hp1")?.gameObject;
        if (hp2 == null) hp2 = transform.Find("hp2")?.gameObject;
        if (hp3 == null) hp3 = transform.Find("hp3")?.gameObject;

        UpdateHPDisplay();
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        health = Mathf.Max(health, 0);

        UpdateHPDisplay();

        if (health <= 0)
        {
            Die();
        }
    }

    private void UpdateHPDisplay()
    {
        if (hp1 != null) hp1.SetActive(health >= 1);
        if (hp2 != null) hp2.SetActive(health >= 2);
        if (hp3 != null) hp3.SetActive(health >= 3);
    }

    private void Die()
    {
        if (deathPrefab != null)
        {
            GameObject clone = Instantiate(
                deathPrefab,
                transform.position,
                transform.rotation
            );

            Destroy(clone, deathAnimLength);
        }

        Destroy(gameObject);
    }
}
