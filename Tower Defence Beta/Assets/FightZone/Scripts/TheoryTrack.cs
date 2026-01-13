using System.Collections.Generic;
using UnityEngine;

public class TheoryTrack : MonoBehaviour
{
    // Grid size
    private int rows = 3;
    private int cols = 3;

    // Enemy position
    private int currentRow = 0;
    private int currentCol = 1;

    // Goal
    private int goalRow = 2;
    private int goalCol = 1;

    private bool reachedGoal = false;

    void Start()
    {
        Debug.Log($"Enemy starts at [{currentRow},{currentCol}]");
    }

    void Update()
    {
        if (reachedGoal) return;

        // Get possible moves
        List<string> possibleMoves = new List<string>();

        if (currentCol > 0) possibleMoves.Add("LEFT");
        if (currentCol < cols - 1) possibleMoves.Add("RIGHT");
        if (currentRow < rows - 1) possibleMoves.Add("DOWN");

        if (possibleMoves.Count == 0)
        {
            Debug.Log("No valid moves! Enemy is stuck.");
            reachedGoal = true;
            return;
        }

        // Pick a random move
        string move = possibleMoves[Random.Range(0, possibleMoves.Count)];

        // Apply move
        switch (move)
        {
            case "LEFT":
                currentCol -= 1;
                break;
            case "RIGHT":
                currentCol += 1;
                break;
            case "DOWN":
                currentRow += 1;
                break;
        }

        Debug.Log($"Enemy moves {move} to [{currentRow},{currentCol}]");

        // Check if goal reached
        if (currentRow == goalRow && currentCol == goalCol)
        {
            reachedGoal = true;
            Debug.Log("Enemy reached the goal [2,1]!");
        }
    }
}
