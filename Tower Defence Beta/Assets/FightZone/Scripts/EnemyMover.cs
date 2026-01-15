using UnityEngine;
using System.Collections.Generic;

public class EnemyMover : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("Speed")]
    public float baseSpeed = 2f;
    public float currentSpeed;

    [Header("Paths")]
    public List<Transform> path1 = new List<Transform>();
    public List<Transform> path2 = new List<Transform>();
    public List<Transform> path3 = new List<Transform>();

    private List<Transform> currentPath = new List<Transform>();
    private int currentWaypointIndex = 0;
    private Transform currentTarget;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = baseSpeed;
    }

    void Start()
    {
        // Pick a random path at spawn
        int randomIndex = Random.Range(0, 3);
        switch (randomIndex)
        {
            case 0: currentPath = new List<Transform>(path1); break;
            case 1: currentPath = new List<Transform>(path2); break;
            case 2: currentPath = new List<Transform>(path3); break;
        }

        // Set the first waypoint as target, skip if at spawn position
        currentWaypointIndex = 0;
        if (currentPath.Count > 0)
        {
            if (Vector2.Distance(transform.position, currentPath[0].position) < 0.01f)
                currentWaypointIndex = 1;

            if (currentWaypointIndex < currentPath.Count)
                currentTarget = currentPath[currentWaypointIndex];
        }
    }

    void FixedUpdate()
    {
        if (currentTarget != null)
        {
            Vector2 direction = ((Vector2)currentTarget.position - rb.position).normalized;
            rb.MovePosition(rb.position + direction * currentSpeed * Time.fixedDeltaTime);

            if (Vector2.Distance(rb.position, currentTarget.position) < 0.01f)
            {
                NextWaypoint();
            }
        }
    }

    private void NextWaypoint()
    {
        currentWaypointIndex++;
        if (currentWaypointIndex < currentPath.Count)
        {
            currentTarget = currentPath[currentWaypointIndex];
        }
        else
        {
            currentTarget = null; // reached end of path
        }
    }

    // Trigger detection
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Zone"))
        {
            currentSpeed = baseSpeed * 100f;
            Debug.Log("Entered Zone! Speed boosted.");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Zone"))
        {
            currentSpeed = baseSpeed;
            Debug.Log("Exited Zone! Speed reset.");
        }
    }
}
