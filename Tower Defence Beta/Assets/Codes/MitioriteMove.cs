using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MitioriteMove : MonoBehaviour
{
    public float maxSpeed = 5f;
    public float minSpeed = 1f;
    public float slowDistance = 3f;

    Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = transform.position.z; // keep depth

        float distance = Vector2.Distance(transform.position, mouseWorld);

        float speed = Mathf.Lerp(
            minSpeed,
            maxSpeed,
            Mathf.Clamp01(distance / slowDistance)
        );

        transform.position = Vector3.Lerp(
            transform.position,
            mouseWorld,
            speed * Time.deltaTime
        );
    }
}

