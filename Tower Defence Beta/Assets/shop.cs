using UnityEngine;

public class ShopMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject buyMenu;
    public GameObject sellMenu;

    void Start()
    {
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        mainMenu.SetActive(true);
        buyMenu.SetActive(false);
        sellMenu.SetActive(false);
    }

    public void ShowBuyMenu()
    {
        mainMenu.SetActive(false);
        buyMenu.SetActive(true);
        sellMenu.SetActive(false);
    }

    public void ShowSellMenu()
    {
        mainMenu.SetActive(false);
        buyMenu.SetActive(false);
        sellMenu.SetActive(true);
    }

    public void ExitShop()
    {
        Debug.Log("Exit shop");
        // t.ex. ladda annan scen
        // SceneManager.LoadScene("Game");
    }
}
