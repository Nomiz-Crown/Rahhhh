using UnityEngine;
using UnityEngine.SceneManagement;

public class loadscene : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("2D Trigger entered by: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player detected! Loading scene Zone1");
            SceneManager.LoadScene("Zone1");
        }
        else
        {
            Debug.Log("Object is not tagged Player");
        }
    }
}
