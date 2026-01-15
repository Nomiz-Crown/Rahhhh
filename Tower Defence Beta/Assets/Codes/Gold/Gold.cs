using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Gold : MonoBehaviour
{
    public int GoldCoinHave = 0;
    public Sprite GoldCoin;
    public TextMeshProUGUI goldText;
    // Start is called before the first frame update
    void Start()
    {
        UpdateGoldUI();
    }
    public void AddGold(int amount)
    {
        GoldCoinHave += amount;
        UpdateGoldUI();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    void UpdateGoldUI()
    {
        goldText.text = GoldCoinHave.ToString();
    }

}
