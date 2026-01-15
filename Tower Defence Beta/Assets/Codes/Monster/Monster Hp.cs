using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHp : MonoBehaviour
{
    public int maxHP = 100;
    public int currentHP;
    public Image  hpFill;
    // Start is called before the first frame update

    public GameObject BloodEffect;
    public Transform effectSpawnPoint;



    void Start()
    {
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
        hpFill.fillAmount = currentHP / maxHP;
    }
}
