using UnityEngine;
using UnityEngine.UI;

public class Cutscene2D : MonoBehaviour
{
    public Image cutsceneImage;
    public Sprite[] images;
    public float timeBetweenImages = 2f;
    public GameObject cutsceneCanvas;

    private int index = 0;

    void Start()
    {
        cutsceneCanvas.SetActive(true);
        cutsceneImage.sprite = images[0];
        InvokeRepeating(nameof(NextImage), timeBetweenImages, timeBetweenImages);
    }

    void NextImage()
    {
        index++;

        if (index >= images.Length)
        {
            CancelInvoke();
            cutsceneCanvas.SetActive(false); // stänger cutscene
            return;
        }

        cutsceneImage.sprite = images[index];
    }
}