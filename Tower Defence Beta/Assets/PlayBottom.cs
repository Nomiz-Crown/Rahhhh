using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
public class PlayBottom : MonoBehaviour
{

  public void PlayGame()
    {
        Invoke(nameof(DelayedPlay), 1.5f);

       
        
    }
    void DelayedPlay()
    {
        // Transition code (example: load scene)
        SceneManager.LoadScene(1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}