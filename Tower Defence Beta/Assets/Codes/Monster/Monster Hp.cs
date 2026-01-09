using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterHp : MonoBehaviour
{
    public int maxHp;
    private int currentHP;
    // Start is called before the first frame update
    void Start()
    {
        maxHp = currentHP; 
    }

    // Update is called once per frame
    void Update()
    {
        if( currentHP <= 0)
        {
            Destroy(gameObject); 
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // currentHp - attack;   CODE FÖR SENARE 

    }
}
