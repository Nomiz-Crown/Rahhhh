using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    public int attack = 10;
    public float waitSecBeforeAttack = 1.5f;

    private bool canAttack = true; 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Door") && canAttack)
        { 
            DoorHP  door = collision.gameObject.GetComponent<DoorHP>();

            if (door != null)
            {
                door.takeDamage(attack);
                StartCoroutine(AttackCooldown());
            }
        }
    }
    private IEnumerator AttackCooldown()
    {
        canAttack = false;      //  så inte attackerar igen
        yield return new WaitForSeconds(waitSecBeforeAttack); // Wait
        canAttack = true;             // kan attackera          
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
