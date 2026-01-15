using UnityEngine;
using System.Collections.Generic;

public class EnemyMover : MonoBehaviour
{
    private Rigidbody2D rb;

    public bool isClone = false;

    [Header("Speed")]
    public float baseSpeed = 2f;
    public float currentSpeed;

    [Header("Waypoint Settings")]
    [Tooltip("How close the enemy must be to a waypoint to consider it reached.")]
    public float defaultReachDistance = 0.15f;
    public float zoneReachDistance = 1f; // distance while in Zone
    private float reachDistance;

    [Header("Paths")]
    public List<Transform> path1 = new List<Transform>();
    public List<Transform> path2 = new List<Transform>();
    public List<Transform> path3 = new List<Transform>();

    private List<Transform> currentPath = new List<Transform>();
    private int currentWaypointIndex = 0;
    private Transform currentTarget;

    private bool inZone = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        currentSpeed = baseSpeed;
        reachDistance = defaultReachDistance;
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

        // Set the first waypoint as target
        currentWaypointIndex = 0;
        if (currentPath.Count > 0)
        {
            if (Vector2.Distance(transform.position, currentPath[0].position) < 0.1f)
                currentWaypointIndex = 1;

            if (currentWaypointIndex < currentPath.Count)
                currentTarget = currentPath[currentWaypointIndex];
        }
    }

    void FixedUpdate()
    {
        if (!isClone || currentTarget == null) return;

        Vector2 toTarget = (Vector2)currentTarget.position - rb.position;
        float distance = toTarget.magnitude;

        // Reached waypoint
        if (distance <= reachDistance)
        {
            rb.position = currentTarget.position;
            rb.velocity = Vector2.zero;
            NextWaypoint();
            return;
        }

        Vector2 direction = toTarget.normalized;
        rb.velocity = direction * currentSpeed;
    }

    private void NextWaypoint()
    {
        currentWaypointIndex++;
        if (currentWaypointIndex < currentPath.Count)
            currentTarget = currentPath[currentWaypointIndex];
        else
            currentTarget = null; // reached end
    }

    // Trigger detection
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isClone) return;

        if (other.CompareTag("Zone"))
        {
            inZone = true;
            currentSpeed = baseSpeed * 100f;
            reachDistance = zoneReachDistance;
            Debug.Log("Entered Zone! Speed boosted, waypoint margin increased.");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!isClone) return;

        if (other.CompareTag("Zone"))
        {
            inZone = false;
            currentSpeed = baseSpeed;
            reachDistance = defaultReachDistance;
            Debug.Log("Exited Zone! Speed reset, waypoint margin normal.");
        }
    }
}
