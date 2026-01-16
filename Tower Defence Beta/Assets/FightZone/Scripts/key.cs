using UnityEngine;

public class key : MonoBehaviour
{
    public TrackLock trackLock;
    public GameObject keyy;

    [Header("Key State")]
    public bool keyCollected = false;

    private bool spawned = false; // prevents auto-reactivation

    void Update()
    {
        // Spawn the key once when the wave ends
        if (trackLock != null && trackLock.endWave && !keyCollected && !spawned)
        {
            keyy.SetActive(true);
            spawned = true;
        }
    }

    public void ResetKey()
    {
        keyCollected = false;
        keyy.SetActive(false);
        spawned = false; // allow it to spawn again later
    }
}
