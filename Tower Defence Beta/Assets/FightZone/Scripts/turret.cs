using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class turret : MonoBehaviour
{
    public int Cost = 100;
    public bool isOriginal = false;

    public TrackLock trackLock;
    public Gold Gold;

    public GameObject Mana;
    public GameObject orngg;
    public int dmg = 25;

    private bool isPlaced = true;

    public GameObject CantPlace;
    private GameObject rangeIndicator;
    private bool isSelected = false;


    private float bulletTimer = 0f;

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
    private List<MonsterHp> enemiesInRange = new List<MonsterHp>();

    private void Start()
    {
        if (isOriginal)
        {
            trackLock.endWave = true;
        }
        CantPlace.SetActive(false);

        // 🔥 THIS FIXES IT 🔥 yay tack gpt
        Transform range = transform.Find("Range");
        {
            rangeIndicator = range.gameObject;
            rangeIndicator.SetActive(false);
        }

        if (bullet != null)
        {
            bulletStartPos = bullet.position;
            bullet.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (!isPlacing && Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(
                Camera.main.ScreenToWorldPoint(Input.mousePosition),
                Vector2.zero
        );

        if (hit.collider == null || hit.collider.transform.root != transform)
        {
            isSelected = false;
            if (rangeIndicator != null)
                rangeIndicator.SetActive(false);
            }
        } 
        if(isPlacing){
            orngg.SetActive(false);
            CantPlace.SetActive(true);
        }
        // Handle turret placement
        if (isPlacing && turretInstance != null && Engineer.instance.isQuestComplete == true)
        {
            
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10f;
            turretInstance.transform.position = Camera.main.ScreenToWorldPoint(mousePos);

            if (Input.GetMouseButtonDown(0))
            {
                Mana.SetActive(true);
                isPlacing = false;
                isPlaced = true;
                rangeIndicator.SetActive(false);
                turretInstance = null;
                CantPlace.SetActive(false);

            }
        }

        // Attack the first enemy in range if possible
        if (canAttack && enemiesInRange.Count > 0)
        {
            MonsterHp enemy = enemiesInRange[0];
            if (enemy != null && !bulletFlying)
            {
                bulletFlying = true;
                bulletTarget = enemy.transform;
                bulletStartPos = bullet.position = transform.position; // <- update start pos here
                bullet.gameObject.SetActive(true);
                bulletTimer = 0f; // reset timer
                StartCoroutine(AttackCooldown());
            }
            if (bulletFlying)
            {
                bulletTimer += Time.deltaTime;

                // If bullet has been flying more than 2 seconds, reset it
                if (bulletTimer >= 2f)
                {
                    bullet.position = bulletStartPos;
                    bullet.gameObject.SetActive(false);
                    bulletFlying = false;
                    bulletTarget = null;
                    bulletTimer = 0f;
                }
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
                bulletTarget.GetComponent<MonsterHp>()?.TakeDamage(dmg);

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
        if (!isPlacing && Gold.GoldCoinHave > (Cost - 1))
        {
            Gold.AddGold(- Cost);
            turretInstance = Instantiate(turretPrefab);
            turretInstance.GetComponent<turret>().isOriginal = false;
            isPlaced = false; // this one is a ghost

            GameObject ghostRange = turretInstance.transform.Find("Range")?.gameObject;
            if (ghostRange != null)
                ghostRange.SetActive(true);

            isPlacing = true;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(enemyTag))
        {
            MonsterHp enemy = collision.GetComponent<MonsterHp>();
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
            MonsterHp enemy = collision.GetComponent<MonsterHp>();
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

    public void OnHitboxClicked()
    {
        if (isPlacing) return;

        isSelected = !isSelected;
            rangeIndicator.SetActive(isSelected);
    }


}
