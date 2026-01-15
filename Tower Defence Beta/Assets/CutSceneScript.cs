using UnityEngine;
using UnityEngine.UI;
using TMPro;

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Cutscene2D : MonoBehaviour
{
    public Image cutsceneImage;
    public Sprite[] images;
    public string[] cutsceneTexts;
    public float timeBetweenImages = 2f;
    public GameObject cutsceneCanvas;
    public TMP_Text cutsceneText;

    private int index = 0;
    private RectTransform imageRect;
    private Vector3 originalPos;

    void Start()
    {
        cutsceneCanvas.SetActive(true);

        imageRect = cutsceneImage.GetComponent<RectTransform>();
        originalPos = imageRect.localPosition;

        ShowFrame(0);
        InvokeRepeating(nameof(NextImage), timeBetweenImages, timeBetweenImages);
    }

    void NextImage()
    {
        index++;

        if (index >= images.Length)
        {
            CancelInvoke();
            cutsceneCanvas.SetActive(false);
            return;
        }

        ShowFrame(index);
    }

    void ShowFrame(int i)
    {
        cutsceneImage.sprite = images[i];
        cutsceneText.text = cutsceneTexts[i];

        // shake only on second image
        if (i == 1)
            StartCoroutine(ScreenShake(4f, 10f));
    }

    IEnumerator ScreenShake(float duration, float strength)
    {
        float time = 0f;

        while (time < duration)
        {
            imageRect.localPosition =
                originalPos + (Vector3)Random.insideUnitCircle * strength;

            time += Time.deltaTime;
            yield return null;
        }

        imageRect.localPosition = originalPos;
    }
}


 