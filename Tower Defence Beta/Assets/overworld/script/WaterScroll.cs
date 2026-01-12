using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class WaterScroll : MonoBehaviour
{
    [Header("Scroll Settings")]
    public Vector2 scrollSpeed = new Vector2(0.02f, 0.01f);

    [Header("Brightness Settings")]
    [Range(0f, 3f)]
    public float brightness = 1f; // multiplies the sprite color

    private SpriteRenderer sr;
    private Material mat;
    private Vector2 offset;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        // Create a new material instance for this SpriteRenderer to scroll the texture
        mat = new Material(sr.material);
        sr.material = mat;
    }

    void Update()
    {
        // Scroll the texture
        offset += scrollSpeed * Time.deltaTime;
        mat.mainTextureOffset = offset;

        // Adjust brightness
        sr.color = Color.white * brightness;
    }
}
