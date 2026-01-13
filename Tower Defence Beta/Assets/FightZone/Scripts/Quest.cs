using UnityEngine;

[System.Serializable]
public class Quest
{
    public string questGiver;  // Name of NPC giving the quest
    public string objective;   // What the player needs to do
    public string reward;      // Reward for completing the quest

    // Optional: constructor for easy runtime creation
    public Quest(string questGiver, string objective, string reward)
    {
        this.questGiver = questGiver;
        this.objective = objective;
        this.reward = reward;
    }
}
