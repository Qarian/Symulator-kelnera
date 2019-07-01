using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] Transform tablesTransform = default;
    [SerializeField] Transform foodSpawnPointsTransform = default;
	[SerializeField] TextMeshProUGUI moneyUI = default;
    [SerializeField] GameObject foodPrefab = default;

	[HideInInspector] public bool customerSelected = false;

	[Space]
	[SerializeField] static float customersFoodChoosingTime = 4f;
	[SerializeField] float foodSpawnTime = 10f;
	[SerializeField] float newCustomerTime = 8f;
	[SerializeField] Vector2Int money = default;
	private int score = 0;

	private List<Table> tables = new List<Table>();
	[HideInInspector] public List<Table> freeTables = new List<Table>();
	private List<Transform> foodSpawnPoints = new List<Transform>();

	[HideInInspector] public int maxQueueLength = 1;
	private ClusterManager clusterManager;

	public static GameManager singleton;
    private void Awake()
    {
        singleton = this;
    }

    private void Start()
    {
		clusterManager = GetComponent<ClusterManager>();

		for (int i = 0; i < tablesTransform.childCount; i++)
		{
			tables.Add(tablesTransform.GetChild(i).GetComponent<Table>());
			freeTables.Add(tables[i]);
			tables[i].id = i;
		}
        foreach (Transform child in foodSpawnPointsTransform)
        {
            foodSpawnPoints.Add(child);
        }
		score = 0;
		moneyUI.text = score.ToString();

		StartCoroutine(NewCustomers());
    }

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
			Application.Quit();
	}

	IEnumerator NewCustomers()
	{
		while (true)
		{
			if (clusterManager.waitingClusters < maxQueueLength)
			{
				clusterManager.GenerateNewCluster();
			}
			yield return new WaitForSeconds(newCustomerTime);
		}
	}

	public void SelectCustomer()
	{
		customerSelected = true;
		foreach (var table in freeTables)
		{
			table.Enable();
		}
	}

	public void ChangeScore(int change)
	{
		score += change;
		moneyUI.text = score.ToString();
	}

    public void OrderFood(int id)
    {
        StartCoroutine(SpawnFood(id));
    }
	//Spawn food after set time
    IEnumerator SpawnFood(int id)
    {
        yield return new WaitForSeconds(foodSpawnTime);
        GameObject go = Instantiate(foodPrefab, foodSpawnPoints[id].position, Quaternion.identity);
		go.GetComponent<FoodScript>().SetColor(tables[id].color);
	}
}
