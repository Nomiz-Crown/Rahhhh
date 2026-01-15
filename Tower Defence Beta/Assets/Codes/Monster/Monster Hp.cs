using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHp : MonoBehaviour
{
    private GameObject healthBarInstance;

    public float maxHP = 100;
    private float currentHP;
    public Image  hpFill;
    public GameObject healthBarPrefab;

    
    public GameObject BloodEffect;
    public Transform effectSpawnPoint;
    public Vector3 offset = new Vector3(0, 2f, 0);


    void Start()
    {
        healthBarInstance = Instantiate(
        healthBarPrefab,
        transform.position + offset,
        Quaternion.identity,
        transform
    );

    healthBarInstance.SetActive(false);


        //GameObject bar = Instantiate(healthBarPrefab, transform.position + offset, Quaternion.identity, transform);

        Image[] images = healthBarInstance.GetComponentsInChildren<Image>();

        foreach (Image img in images)
        {
            if (img.type == Image.Type.Filled)
            {
                hpFill = img;
                break;
            }
        }

        if (hpFill == null)
        {
            Debug.LogError("HP FILL NOT FOUND!");
            return;
        }
        currentHP = maxHP;
        UpdateHP();
    }

    // Update is called once per frame
    void Update()
    {
        
        if( currentHP <= 0)
        {
            Die();
            Destroy(gameObject); 
        }

    }
    private void Die()
    {
        if (BloodEffect != null && effectSpawnPoint != null)
        {
            GameObject effect = Instantiate(BloodEffect, effectSpawnPoint.position, effectSpawnPoint.rotation);
            Destroy(effect, 0.5f);
        }
    }
    public void TakeDamage(int damageAmount)   
    {
        currentHP -= damageAmount;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        UpdateHP();

    }
    void UpdateHP()
    {
     
      hpFill.fillAmount = (float)currentHP / maxHP;
    }
    void OnMouseEnter()
    {
        if (healthBarInstance != null)
            healthBarInstance.SetActive(true);
    }

    void OnMouseExit()
    {
        if (healthBarInstance != null)
            healthBarInstance.SetActive(false);
    }

}
