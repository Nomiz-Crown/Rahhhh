using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turret : MonoBehaviour
{
    [Header("Bullet Settings")]
    public Transform bullet;
    public float bulletSpeed = 10f;
    private Vector3 bulletStartPos;
    private bool bulletFlying = false;
    private Transform bulletTarget;


    public GameObject turretPrefab;
    private GameObject turretInstance;
    private bool isPlacing = false;

    [Header("Enemy Detection")]
    public string enemyTag = "Enemy";
    public float attackCooldown = 1.5f;

    private bool canAttack = true;
    private List<enemyHP> enemiesInRange = new List<enemyHP>();

    private void Start()
    {
        if (bullet != null)
        {
            bulletStartPos = bullet.position;
            bullet.gameObject.SetActive(false); // invisible until shooting
        }

    }
    void Update()
    {
        // Handle turret placement
        if (isPlacing && turretInstance != null)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10f;
            turretInstance.transform.position = Camera.main.ScreenToWorldPoint(mousePos);

            if (Input.GetMouseButtonDown(0))
            {
                isPlacing = false;
                turretInstance = null;
                

            }
        }

        // Attack the first enemy in range if possible
        if (canAttack && enemiesInRange.Count > 0)
        {
            enemyHP enemy = enemiesInRange[0];
            if (enemy != null && !bulletFlying)
            {
                bulletFlying = true;
                bulletTarget = enemy.transform;
                bulletStartPos = bullet.position = transform.position; // <- update start pos here
                bullet.gameObject.SetActive(true);
                StartCoroutine(AttackCooldown());
            }
        }


        // Move bullet if flying
        if (bulletFlying && bulletTarget != null)
        {
            Vector3 dir = (bulletTarget.position - bullet.position).normalized;
            bullet.position += dir * bulletSpeed * Time.deltaTime;

            // Check if bullet reached enemy
            if (Vector3.Distance(bullet.position, bulletTarget.position) < 0.1f)
            {
                // Damage enemy
                bulletTarget.GetComponent<enemyHP>()?.TakeDamage(1);

                // Reset bullet
                bullet.position = bulletStartPos;
                bullet.gameObject.SetActive(false);
                bulletFlying = false;
                bulletTarget = null;
            }
        }

    }

    public void StartPlacingTurret()
    {
        if (!isPlacing)
        {
            turretInstance = Instantiate(turretPrefab);
            isPlacing = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(enemyTag))
        {
            enemyHP enemy = collision.GetComponent<enemyHP>();
            if (enemy != null && !enemiesInRange.Contains(enemy))
            {
                enemiesInRange.Add(enemy);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(enemyTag))
        {
            enemyHP enemy = collision.GetComponent<enemyHP>();
            if (enemy != null && enemiesInRange.Contains(enemy))
            {
                enemiesInRange.Remove(enemy);
            }
        }
    }

    private IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}
