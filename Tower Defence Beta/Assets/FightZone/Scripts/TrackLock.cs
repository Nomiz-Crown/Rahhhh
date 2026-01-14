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

    private IEnumerator MoveAlongPath(GameObject moverObj, List<GameObject> path)
    {
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

       // Destroy(moverObj); // Optional: remove clone after finishing path
    }

    private void SpawnClone()
    {
        if (mover == null) return;

        // Define the 3 paths
        List<List<GameObject>> possiblePaths = new List<List<GameObject>>();

        // Path 1: TopToBottom
        List<GameObject> path1 = new List<GameObject>();
        path1.AddRange(square_0_1.topToBottom);
        path1.AddRange(square_1_1.topToBottom);
        path1.AddRange(square_2_1.topToBottom);
        possiblePaths.Add(path1);

        // Path 2: 0_1 TopToRight → 0_2 LeftToBottom → 1_2 TopToBottom → 2_2 TopToLeft → 2_1 RightToBottom
        // Path 2
        List<GameObject> path2 = new List<GameObject>();
        path2.AddRange(square_0_1.topToRight);
        path2.AddRange(square_0_2.leftToBottom);
        path2.AddRange(square_1_2.topToBottom);
        path2.AddRange(square_2_2.topToLeft);
        path2.AddRange(square_2_1.rightToBottom);
        possiblePaths.Add(path2);

        // Path 3
        List<GameObject> path3 = new List<GameObject>();
        path3.AddRange(square_0_1.topToLeft);
        path3.AddRange(square_0_0.rightToBottom);
        path3.AddRange(square_1_0.topToBottom);
        path3.AddRange(square_2_0.topToRight);
        path3.AddRange(square_2_1.leftToBottom);
        possiblePaths.Add(path3);


        // Pick a random path
        List<GameObject> chosenPath = possiblePaths[Random.Range(0, possiblePaths.Count)];

        // Spawn clone at first waypoint
        GameObject moverClone = Instantiate(mover, chosenPath[0].transform.position, mover.transform.rotation);

        // Start moving the clone along the chosen path
        StartCoroutine(MoveAlongPath(moverClone, chosenPath));
    }

    private IEnumerator SpawnClonesContinuously()
    {
        while (true)
        {
            SpawnClone();
            yield return new WaitForSeconds(2f); // 2f = 2 sekunder. uhh space mellan spawn. (spawn rate)
        }
    }




}
