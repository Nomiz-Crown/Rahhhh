using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrenchQuestManager : MonoBehaviour
{
    public static WrenchQuestManager instance;

    public bool hasWrench = false;
    public bool questCompleted = false;

    void Awake()
    {
        instance = this;
    }

    public void CompleteQuest()
    {
        questCompleted = true;
        Debug.Log("Quest klar! Spelaren fick item.");
    }
}
