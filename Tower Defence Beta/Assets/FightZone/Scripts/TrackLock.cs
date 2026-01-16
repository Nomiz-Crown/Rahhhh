using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class TrackLock : MonoBehaviour
{
    [System.Serializable]
    public class EnemyEntry
    {
        public GameObject enemyPrefab;   // The prefab
        public int amount = 0;           // How many to spawn
    }

    [System.Serializable]
    public class Wave
    {
        public string waveName;                 // Name for clarity
        public EnemyEntry[] enemies = new EnemyEntry[3];  // Three enemy slots
    }


    [Header("Wave System")]
    public List<Wave> waves = new List<Wave>();
    public Transform spawnPoint;          // Where enemies will spawn
    public float spawnInterval = 1f;      // Time between enemy spawns
    public float timeBetweenWaves = 10f;  // Delay after wave is cleared

    private int currentWaveIndex = 0;
    private List<GameObject> activeEnemies = new List<GameObject>();

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
        StartCoroutine(StartWave(waves[currentWaveIndex]));
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

    private IEnumerator StartWave(Wave wave)
    {
        Debug.Log($"Starting wave: {wave.waveName}");
        activeEnemies.Clear();

        // Loop through all 3 enemy types
        foreach (var entry in wave.enemies)
        {
            if (entry == null || entry.enemyPrefab == null) continue;
    
            for (int i = 0; i < entry.amount; i++)
            {
                GameObject enemy = Instantiate(entry.enemyPrefab, spawnPoint.position, Quaternion.identity);
                activeEnemies.Add(enemy);
                EnemyMover mover = enemy.GetComponent<EnemyMover>();
                mover.isClone = true;

                yield return new WaitForSeconds(2f); // interval per enemy
            }
        }

        // Wait until all enemies are destroyed
        yield return new WaitUntil(() => activeEnemies.TrueForAll(e => e == null));

        Debug.Log($"Wave {wave.waveName} cleared!");

        // Wait before next wave
        yield return new WaitForSeconds(timeBetweenWaves);
 
        // Start next wave if available
        currentWaveIndex++;
        if (currentWaveIndex < waves.Count)
        {
            StartCoroutine(StartWave(waves[currentWaveIndex]));
        }
        else
        {
            Debug.Log("All waves completed!");
        }
    }
}
