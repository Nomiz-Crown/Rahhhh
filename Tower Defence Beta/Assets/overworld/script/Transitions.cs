using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Transitions : MonoBehaviour
{
    public TrackLock trackLock;

    [Header("UI")]
    public GameObject transition;
    public List<Sprite> frames = new List<Sprite>();
    public float framesPerSecond = 12f;

    [Header("Teleport")]
    public Transform teleportTarget;

    private Image transitionImage;
    private Coroutine animationRoutine;

    private void Start()
    {
        transition.SetActive(false);
        transitionImage = transition.GetComponent<Image>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && trackLock.endWave == true)
        {
            if (animationRoutine != null) 
            StopCoroutine(animationRoutine);
            animationRoutine = StartCoroutine(PlayTransitionSequence(other.transform));
        }
    }

    private IEnumerator PlayTransitionSequence(Transform player)
    {
        yield return PlayAnimation(frames);

        if (teleportTarget != null) 
            player.position = teleportTarget.position;

        List<Sprite> reversedFrames = new List<Sprite>(frames);
        reversedFrames.Reverse();
        yield return PlayAnimation(reversedFrames);

        transition.SetActive(false);
    }

    private IEnumerator PlayAnimation(List<Sprite> spriteFrames)
    {
        transition.SetActive(true);
        float delay = 1f / framesPerSecond;

        for (int i = 0; i < spriteFrames.Count; i++)
        {
            transitionImage.sprite = spriteFrames[i];
            yield return new WaitForSeconds(delay);
        }
    }
}
