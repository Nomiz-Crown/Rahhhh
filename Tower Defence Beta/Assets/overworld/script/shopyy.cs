using UnityEngine;

public class shopyy : MonoBehaviour
{
    [SerializeField] private GameObject guiObject;

    private void Start()
    {
        guiObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            guiObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            guiObject.SetActive(false);
        }
    }
}
