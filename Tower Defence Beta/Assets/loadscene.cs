using UnityEngine;
using UnityEngine.SceneManagement;

public class loadscene : MonoBehaviour
{
    [SerializeField] Animator transitionanim;
    private void Start()
    {
        Debug.Log("loadscene script active on " + gameObject.name);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("2D Trigger entered by: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player detected! Starting scene load coroutine");
            StartCoroutine(LoadSceneAfterDelay());
        }
    }

    private System.Collections.IEnumerator LoadSceneAfterDelay()
    {
        Debug.Log("Waiting 1 seconds before loading scene...");
        yield return new WaitForSeconds(1f);
       
        Debug.Log("Loading scene Zone1 now");
        SceneManager.LoadScene("Zone1");
        transitionanim.SetTrigger("start");

    }
}
