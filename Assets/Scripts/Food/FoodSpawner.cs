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

        singleton = this;
    }

    public void OrderCubeFood(Color color, int quantity)
    {
        Debug.Log("Spawn " + quantity + " food");
        for (int i = 0; i < quantity; i++)
            StartCoroutine(spawnFood(cubeFoodPrefab, CustomersManager.singleton.foodSpawnTime, color));
    }

    private IEnumerator spawnFood(GameObject prefab, float time, Color color)
    {
        yield return new WaitForSeconds(time);
        // waiting for spawn point to be free
        WaitForSeconds waiting = new WaitForSeconds(1);
        bool spawned = false;
        while (!spawned)
        {
            for (int i = 0; i < spawnPoints.Count; i++)
            {
                if (spawnPoints[i].childCount == 0)
                {
                    GameObject go = Instantiate(prefab, spawnPoints[i].position + new Vector3(0, i * 0.5f, 0), Quaternion.identity);
                    go.transform.SetParent(spawnPoints[i]);
                    go.GetComponent<FoodScript>().SetColor(color);
                    spawned = true;
                    break;
                }
                else
                    yield return waiting;
            }
        }
    }
}
