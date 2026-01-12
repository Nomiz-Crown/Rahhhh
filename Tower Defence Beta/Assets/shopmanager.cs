using UnityEngine;
using UnityEngine.UI;
public class shopmanager : MonoBehaviour
{
    public static shopmanager instance;
    public int coins = 300;
    // public Upgrade[] upgrade;
    // Refrences
    public Text coinText;
    public GameObject shopUI;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

    }
    public void ToggleShop()
    {
        shopUI.SetActive(!shopUI.activeSelf);
    }
}
