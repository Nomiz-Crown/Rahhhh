using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorHpForLoosingScene : MonoBehaviour
{
    public int DoorMaxHp = 1000;
    public int DoorCurrentHp;
    // Start is called before the first frame update
    void Start()
    {
        DoorCurrentHp = DoorMaxHp;
    }


    public void takeDamage(int attack)
    {
        DoorCurrentHp -= attack;
    }
    // Update is called once per frame
    void Update()
    {

    }
}
