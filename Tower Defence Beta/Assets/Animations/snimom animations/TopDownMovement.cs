using UnityEngine;

public class TopDownMovementNoAnimator : MonoBehaviour
{
    public float moveSpeed = 5f;
    public SpriteRenderer sr;
    public Sprite[] upFrames;
    public Sprite[] downFrames;
    public Sprite[] leftFrames;
    public Sprite[] rightFrames;
    public float frameRate = 0.1f;

    private Vector2 movement;
    private Vector2 lastDir;
    private float frameTimer;
    private int frameIndex;

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement = movement.normalized;

        if (movement != Vector2.zero)
            lastDir = movement;

        Sprite[] currentFrames = GetCurrentFrames();

        if (movement != Vector2.zero)
        {
            frameTimer += Time.deltaTime;
            if (frameTimer >= frameRate)
            {
                frameTimer = 0f;
                frameIndex++;
            }
            sr.sprite = currentFrames[frameIndex % currentFrames.Length];
        }
        else
        {
            frameIndex = 0;
            if (currentFrames.Length > 0)
                sr.sprite = currentFrames[0];
        }
    }

    void FixedUpdate()
    {
        transform.position += (Vector3)(movement * moveSpeed * Time.fixedDeltaTime);
    }

    Sprite[] GetCurrentFrames()
    {
        if (movement.y > 0 || lastDir.y > 0) return upFrames;
        if (movement.y < 0 || lastDir.y < 0) return downFrames;
        if (movement.x < 0 || lastDir.x < 0) return leftFrames;
        if (movement.x > 0 || lastDir.x > 0) return rightFrames;
        return downFrames;
    }
}
