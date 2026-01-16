using UnityEngine;

public class TrackSquareTrigger : MonoBehaviour
{
    public key keyManager;
    public TrackLock trackLock;
    public TrackLock.TrackSquare square;

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        if (other.CompareTag("Player"))
        {
            triggered = true;

            trackLock.TryOpenSquare(square);

            // Start the next wave manually
            trackLock.StartNextWaveManual();
            trackLock.endWave = false;

            // Reset the key cleanly
            keyManager.ResetKey();
        }
    }
}
