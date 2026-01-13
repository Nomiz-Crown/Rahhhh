using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    public int attack = 10;
    public float waitSecBeforeAttack = 1.5f;

    private bool canAttack = true;
    private DoorHP doorInRange;

   
    public GameObject attackEffectPrefab; 
    public Transform effectSpawnPoint;  

    private void DoAttackEffect()
    {
        if (attackEffectPrefab != null && effectSpawnPoint != null)
        {
            GameObject effect = Instantiate(attackEffectPrefab, effectSpawnPoint.position, effectSpawnPoint.rotation);
            Destroy(effect, 0.5f); 
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Door"))
        {
            doorInRange = collision.GetComponent<DoorHP>();
        }
    }

    // Called when leaving the door collider
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Door"))
        {
            doorInRange = null;
        }
    }

    void Update()
    {
        if (doorInRange != null && canAttack)
        {
            doorInRange.takeDamage(attack); // damage the door
            DoAttackEffect();
            StartCoroutine(AttackCooldown());
        }
    }

    private IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(waitSecBeforeAttack);
        canAttack = true;
    }

}
