using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioshop : MonoBehaviour
{
    GameObject MenuAudio;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void OnEnable()
    {
        audioSource.Play();

    }

    void OnDisable()
    {
        audioSource.Stop();
    }
}
