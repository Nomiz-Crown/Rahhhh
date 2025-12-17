using System.Collections;
using UnityEngine;

public class enemy : MonoBehaviour
{
    public GameObject objectToClone;
    public Transform[] spawnPoints;      // 4 spawn points for now
    public float targetY = 7.7f;
    public float moveSpeed = 3f;
    public float spawnInterval = 5f;

    private int currentSpawnIndex = 0;

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnAtNextPoint();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnAtNextPoint()
    {
        Transform spawnPoint = spawnPoints[currentSpawnIndex];

        GameObject clone = Instantiate(
            objectToClone,
            spawnPoint.position,
            Quaternion.identity
        );

        StartCoroutine(MoveUpAndDestroy(clone));

        // jag HATAR
        currentSpawnIndex++;

        // wahh
        if (currentSpawnIndex >= spawnPoints.Length)
        {
            currentSpawnIndex = 0;
        }
    }

    IEnumerator MoveUpAndDestroy(GameObject obj)
    {
        while (obj != null && obj.transform.position.y < targetY)
        {
            Vector3 pos = obj.transform.position;
            pos.y = Mathf.MoveTowards(pos.y, targetY, moveSpeed * Time.deltaTime);
            obj.transform.position = pos;

            yield return null;
        }

        Destroy(obj);
    }
}
