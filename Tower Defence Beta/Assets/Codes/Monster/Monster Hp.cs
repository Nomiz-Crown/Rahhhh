using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterHp : MonoBehaviour
{
    public int maxHp = 100;
    private int currentHP;
    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHp; 
    }

    // Update is called once per frame
    void Update()
    {
        if( currentHP <= 0)
        {
            Destroy(gameObject); 
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // currentHp - attack;   CODE FÖR SENARE 

    }
}
