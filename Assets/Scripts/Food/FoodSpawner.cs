using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    [SerializeField] GameObject cubeFoodPrefab = default;

    private List<Transform> spawnPoints = new List<Transform>();

    public static FoodSpawner singleton;

    private void Start()
    {
        foreach (Transform child in transform)
        {
            spawnPoints.Add(child);
        }
        
        if (spawnPoints.Count == 0)
            Debug.LogError("No food spawning points!");

        singleton = this;
    }

    public void OrderCubeFood(Color color, int quantity, int orderId)
    {
        for (int i = 0; i < quantity; i++)
            StartCoroutine(SpawnFood(cubeFoodPrefab, CustomersManager.singleton.foodSpawnTime, color, orderId));
    }

    private IEnumerator SpawnFood(GameObject prefab, float time, Color color, int orderId)
    {
        yield return new WaitForSeconds(time);
        // waiting for spawn point to be free
        WaitForSeconds waiting = new WaitForSeconds(1);
        while (true)
        {
            for (int i = 0; i < spawnPoints.Count; i++)
            {
                if (spawnPoints[i].childCount == 0)
                {
                    GameObject go = Instantiate(prefab, spawnPoints[i].position + new Vector3(0, i * 0.5f, 0), Quaternion.identity);
                    go.transform.SetParent(spawnPoints[i]);
                    go.GetComponent<FoodScript>().Init(color, orderId);
                    yield break;
                }
            }
            yield return waiting;
        }
    }
}
