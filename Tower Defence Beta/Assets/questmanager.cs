using UnityEngine;
using UnityEngine.UI;
public class questmanager : MonoBehaviour
{
    public static questmanager instance;
    


    public GameObject questui;

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
        questui.SetActive(!questui.activeSelf);
    }
    private void OnGUI()
    {
        
    }
}
[System.Serializable]
public class quest
{
  
    public Sprite Image;
    [HideInInspector] public int quantity;
    [HideInInspector] public GameObject itemRef;

}