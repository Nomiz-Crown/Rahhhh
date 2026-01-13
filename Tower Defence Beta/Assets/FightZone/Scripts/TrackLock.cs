using UnityEngine;
using System.Collections.Generic;

public class TrackLock : MonoBehaviour
{
    [System.Serializable]
    public class TrackSquare
    {
        [Header("Paths")]
        public List<GameObject> topToBottom = new List<GameObject>();
        public bool topToBottomBlocked;

        public List<GameObject> topToRight = new List<GameObject>();
        public bool topToRightBlocked;

        public List<GameObject> topToLeft = new List<GameObject>();
        public bool topToLeftBlocked;

        public List<GameObject> leftToRight = new List<GameObject>();
        public bool leftToRightBlocked;

        public List<GameObject> leftToBottom = new List<GameObject>();
        public bool leftToBottomBlocked;

        public List<GameObject> rightToBottom = new List<GameObject>();
        public bool rightToBottomBlocked;

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
        // Lock everything
        SetSquare(square_0_0, false);
        SetSquare(square_0_1, false);
        SetSquare(square_0_2, false);

        SetSquare(square_1_0, false);
        SetSquare(square_1_1, false);
        SetSquare(square_1_2, false);

        SetSquare(square_2_0, false);
        SetSquare(square_2_2, false);

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

}
