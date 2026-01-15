using UnityEngine;
using System.Collections;

public class TrackLock : MonoBehaviour
{
    public GameObject mover;           // Prefab to clone
    public Transform spawnPoint;        // Where clones spawn
    public float spawnInterval = 2f;

    [System.Serializable]
    public class TrackSquare
    {
        public string label;
        public GameObject lockObject;
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

        // Start spawning clones
        if (mover != null && spawnPoint != null)
        {
            StartCoroutine(SpawnClonesContinuously());
        }
    }

    void SetSquare(TrackSquare square, bool open)
    {
        if (square == null) return;

        square.isOpen = open;

        // Open = lock OFF, Closed = lock ON
        if (square.lockObject != null)
            square.lockObject.SetActive(!open);
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

    IEnumerator SpawnClonesContinuously()
    {
        while (true)
        {
            GameObject clone = Instantiate(mover, spawnPoint.position, spawnPoint.rotation);

            EnemyMover enemyMover = clone.GetComponent<EnemyMover>();
            if (enemyMover != null)
            {
               enemyMover.isClone = true;
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
