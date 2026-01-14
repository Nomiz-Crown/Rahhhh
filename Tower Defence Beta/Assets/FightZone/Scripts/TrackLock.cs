using UnityEngine;
using System.Collections.Generic;
using static TrackLock.TrackSquare;

public class TrackLock : MonoBehaviour
{
    private TrackSquare nextSquare;
    private EntryDirection nextEntryDir;

    private enum EntryDirection { Top, Left, Right }
    private EntryDirection entryDir;

    [Header("Follower Settings")]
    public GameObject follower;   //enemy sak
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

        [Header("Next Square Offset")]
        public int rowDelta;    // +1 = move down, -1 = move up
        public int columnDelta; // +1 = move right, -1 = move left
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
        entryDir = EntryDirection.Top;

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

        currentSquare = square_0_1;   // top middle start
        entryDir = EntryDirection.Top;

        PickNextPath();

    }

    List<(TrackSquare square, EntryDirection newEntry)> GetAvailableMoves(TrackSquare square)
    {
        var moves = new List<(TrackSquare, EntryDirection)>();
        if (square == null) return moves;

        switch (entryDir)
        {
            case EntryDirection.Top:
                if (!square.topToBottomBlocked)
                    foreach (var obj in square.topToBottom)
                        moves.Add((obj.GetComponent<TrackSquare>(), EntryDirection.Top));
                if (!square.topToLeftBlocked)
                    foreach (var obj in square.topToLeft)
                        moves.Add((obj.GetComponent<TrackSquare>(), EntryDirection.Right));
                if (!square.topToRightBlocked)
                    foreach (var obj in square.topToRight)
                        moves.Add((obj.GetComponent<TrackSquare>(), EntryDirection.Left));
                break;

            case EntryDirection.Left:
                if (!square.leftToBottomBlocked)
                    foreach (var obj in square.leftToBottom)
                        moves.Add((obj.GetComponent<TrackSquare>(), EntryDirection.Top));
                if (!square.leftToRightBlocked)
                    foreach (var obj in square.leftToRight)
                        moves.Add((obj.GetComponent<TrackSquare>(), EntryDirection.Left));
                break;

            case EntryDirection.Right:
                if (!square.rightToBottomBlocked)
                    foreach (var obj in square.rightToBottom)
                        moves.Add((obj.GetComponent<TrackSquare>(), EntryDirection.Top));
                break;
        }

        

        return moves;
    }

    void PickNextPath()
    {
        if (currentSquare == null) return;

        // Pick which waypoint list to follow based on entry direction
        switch (entryDir)
        {
            case EntryDirection.Top: currentPath = currentSquare.topToBottom; break;
            case EntryDirection.Left: currentPath = currentSquare.leftToRight; break;
            case EntryDirection.Right: currentPath = currentSquare.rightToBottom; break;
        }

        // Set the next square using the row/column offset
        nextSquare = GetSquareByOffset(currentSquare, currentSquare.rowDelta, currentSquare.columnDelta);

        // Keep the entry direction based on where the follower came from
        nextEntryDir = entryDir;

        waypointIndex = 0;
        follower.transform.position = currentPath[0].transform.position;
        isMoving = true;
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

    // ───── Optional helpers ─────
    public void Unlock_1_1() => SetSquare(square_1_1, true);
    public void Unlock_2_2() => SetSquare(square_2_2, true);
    public void Lock_0_0() => SetSquare(square_0_0, false);

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

    void Update()
    {
        if (!isMoving || currentPath == null || follower == null) return;

        Transform target = currentPath[waypointIndex].transform;

        follower.transform.position = Vector3.MoveTowards(
            follower.transform.position,
            target.position,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(follower.transform.position, target.position) < 0.01f)
        {
            waypointIndex++;

            if (waypointIndex >= currentPath.Count)
            {
                // Finished this path → choose next one
                isMoving = false;

                // Entry direction updates AFTER path completion
                if (currentPath == currentSquare.topToLeft)
                    entryDir = EntryDirection.Right;
                else if (currentPath == currentSquare.topToRight)
                    entryDir = EntryDirection.Left;
                else
                    entryDir = EntryDirection.Top;

                PickNextPath();
            }
        }
    }
    private TrackSquare GetSquareByOffset(TrackSquare square, int rowDelta, int columnDelta)
    {
        int newRow = squareRow(square) + rowDelta;
        int newCol = squareColumn(square) + columnDelta;

        // Example: manually check all squares
        if (newRow == 0 && newCol == 0) return square_0_0;
        if (newRow == 0 && newCol == 1) return square_0_1;
        if (newRow == 0 && newCol == 2) return square_0_2;
        if (newRow == 1 && newCol == 0) return square_1_0;
        if (newRow == 1 && newCol == 1) return square_1_1;
        if (newRow == 1 && newCol == 2) return square_1_2;
        if (newRow == 2 && newCol == 0) return square_2_0;
        if (newRow == 2 && newCol == 1) return square_2_1;
        if (newRow == 2 && newCol == 2) return square_2_2;

        return null; // out of bounds
    }

    // Optional helpers to get current square's row/column
    private int squareRow(TrackSquare square)
    {
        if (square == square_0_0 || square == square_0_1 || square == square_0_2) return 0;
        if (square == square_1_0 || square == square_1_1 || square == square_1_2) return 1;
        return 2;
    }
    private int squareColumn(TrackSquare square)
    {
        if (square == square_0_0 || square == square_1_0 || square == square_2_0) return 0;
        if (square == square_0_1 || square == square_1_1 || square == square_2_1) return 1;
        return 2;
    }



}
