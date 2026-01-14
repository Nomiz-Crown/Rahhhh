using UnityEngine;
using System.Collections.Generic;
using static TrackLock.TrackSquare;
using System.Collections;

public class TrackLock : MonoBehaviour
{
    public GameObject mover; // the object that will follow the waypoints

    public float moveSpeed = 2f;  // units per second

    private TrackSquare currentSquare;
    private List<GameObject> currentPath;
    private int waypointIndex;
    private bool isMoving;



    [System.Serializable]
    public class TrackSquare
    {
        [Header("Bottom Paths")]
        public List<GameObject> topToBottom = new List<GameObject>();
        public bool topToBottomBlocked;

        public List<GameObject> leftToBottom = new List<GameObject>();
        public bool leftToBottomBlocked;

        public List<GameObject> rightToBottom = new List<GameObject>();
        public bool rightToBottomBlocked;

        [Header("Left Paths")]
        public List<GameObject> topToLeft = new List<GameObject>();
        public bool topToLeftBlocked;

        [Header("Right Paths")]
        public List<GameObject> topToRight = new List<GameObject>();
        public bool topToRightBlocked;

        public List<GameObject> leftToRight = new List<GameObject>();
        public bool leftToRightBlocked;


        public string label;          // idk, kan vara useful senare
        public GameObject lockObject; // lock for this square
        public bool isOpen;
    }

    [Header("Row 0")]
    public TrackSquare square_0_0;
    public TrackSquare square_0_1;
    public TrackSquare square_0_2;

    [Header("Row 1")]
    public TrackSquare square_1_0;
    public TrackSquare square_1_1;
    public TrackSquare square_1_2;

    [Header("Row 2")]
    public TrackSquare square_2_0;
    public TrackSquare square_2_1; // start square (always open)
    public TrackSquare square_2_2;

    void Start()
    {
        if (mover != null && square_0_1.topToBottom.Count > 0)
        {
            // Spawn clone at the first waypoint of square_0_1.topToBottom
            //GameObject moverClone = Instantiate(mover, square_0_1.topToBottom[0].transform.position, mover.transform.rotation);
            StartCoroutine(SpawnClonesContinuously());

        }


        // Lock everything
        SetSquare(square_0_0, true);  //set tilll false efter playtest brosky
        SetSquare(square_0_1, true);
        SetSquare(square_0_2, true);

        SetSquare(square_1_0, true);
        SetSquare(square_1_1, true);
        SetSquare(square_1_2, true);

        SetSquare(square_2_0, true);
        SetSquare(square_2_2, true);

        // Start square is always open
        SetSquare(square_2_1, true);

        

    }

    void SetSquare(TrackSquare square, bool open)
    {
        if (square == null) return;

        square.isOpen = open;

        // CORE RULE:
        // Open  = lock OFF
        // Closed = lock ON
        if (square.lockObject != null)
            square.lockObject.SetActive(!square.isOpen);
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        Apply(square_0_0);
        Apply(square_0_1);
        Apply(square_0_2);
        Apply(square_1_0);
        Apply(square_1_1);
        Apply(square_1_2);
        Apply(square_2_0);
        Apply(square_2_1);
        Apply(square_2_2);
    }

    void Apply(TrackSquare square)
    {
        if (square?.lockObject != null)
            square.lockObject.SetActive(!square.isOpen);
    }
#endif

    private IEnumerator MoveThroughPaths(GameObject moverObj)
    {
        // list of topToBottom paths in order
        List<List<GameObject>> paths = new List<List<GameObject>>()
    {
        square_0_1.topToBottom,
        square_1_1.topToBottom,
        square_2_1.topToBottom
    };

        foreach (var path in paths)
        {
            if (path == null || path.Count == 0) continue;

            foreach (var waypoint in path)
            {
                Vector3 startPos = moverObj.transform.position;
                Vector3 endPos = waypoint.transform.position;

                float distance = Vector3.Distance(startPos, endPos);
                float travelTime = distance / moveSpeed;
                float elapsed = 0f;

                while (elapsed < travelTime)
                {
                    moverObj.transform.position = Vector3.Lerp(startPos, endPos, elapsed / travelTime);
                    elapsed += Time.deltaTime;
                    yield return null;
                }

                moverObj.transform.position = endPos;
            }
        }

        Debug.Log("Clone finished all paths!");
    }
    private void SpawnClone()
    {
        if (mover == null) return;

        // Path 1: TopToBottom
        List<GameObject> path = square_0_1.topToBottom;

        if (path == null || path.Count == 0) return;

        // Spawn clone at first waypoint
        GameObject moverClone = Instantiate(mover, path[0].transform.position, mover.transform.rotation);

        // Start moving the clone along the path
        StartCoroutine(MoveThroughPaths(moverClone));
    }
    private IEnumerator SpawnClonesContinuously()
    {
        while (true)
        {
            SpawnClone();
            yield return new WaitForSeconds(2f); // spawn every 2 seconds
        }
    }




}
