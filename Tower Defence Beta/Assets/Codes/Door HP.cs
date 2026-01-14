using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorHP : MonoBehaviour
{

    public int DoorMaxHp = 100;
    public int DoorCurrentHp;
    // Start is called before the first frame update
    void Start()
    {
        DoorCurrentHp = DoorMaxHp;
    }


    public void takeDamage(int attack)
    {
        DoorCurrentHp -= attack;
        if( DoorCurrentHp <= 0)
        {
            SceneManager.LoadSceneAsync(0); 
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
