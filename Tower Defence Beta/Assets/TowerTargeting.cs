using System.Collections.Generic;
using UnityEngine;

public class TowerTargeting : MonoBehaviour
{
    public string enemyTag = "Enemy";
    [HideInInspector] public bool isPlaced = false;

    public float attackCooldown = 2f;

    private Transform parentTower;
    private List<Transform> enemiesInRange = new List<Transform>();
    private float attackTimer = 0f;
    public GameObject bulletVisual;   // Square sprite prefab
    public float bulletLifetime = 0.1f;


    void Awake()
    {
        parentTower = transform.parent;
        if (parentTower == null)
        {
            Debug.LogError("TowerTargeting: radius object must have a parent!");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isPlaced) return;

        if (other.CompareTag(enemyTag))
        {
            enemiesInRange.Add(other.transform);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!isPlaced) return;

        if (other.CompareTag(enemyTag))
        {
            enemiesInRange.Remove(other.transform);
        }
    }

    void Update()
    {
        if (!isPlaced) return;

        // Reduce cooldown timer
        attackTimer -= Time.deltaTime;

        // Clean up destroyed enemies
        enemiesInRange.RemoveAll(e => e == null);

        if (enemiesInRange.Count == 0)
            return;

        // Sort by Y to get "first in line"
        enemiesInRange.Sort((a, b) => a.position.y.CompareTo(b.position.y));

        Transform target = enemiesInRange[0];

        // Rotate tower toward target
        Vector3 direction = target.position - parentTower.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        parentTower.rotation = Quaternion.Euler(0f, 0f, angle);

        // Attack if cooldown ready
        if (attackTimer <= 0f)
        {
            Attack(target);
            attackTimer = attackCooldown;
        }
    }

    void Attack(Transform target)
    {
        if (target == null)
            return;

        // Spawn bullet visual
        GameObject bullet = Instantiate(bulletVisual);

        Vector3 start = parentTower.position;
        Vector3 end = target.position;
        Vector3 direction = end - start;

        // Position bullet halfway
        bullet.transform.position = start + direction / 2f;

        // Rotate bullet to face target
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // Scale bullet to reach target
        Vector3 scale = bullet.transform.localScale;
        scale.x = direction.magnitude;
        bullet.transform.localScale = scale;

        // Destroy bullet after short time
        Destroy(bullet, bulletLifetime);

        // Kill enemy
        Destroy(target.gameObject);
    }

}
