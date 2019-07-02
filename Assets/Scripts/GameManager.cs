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

	public static CustomersCluster selectedCustomers;

	[Space]
	[SerializeField] float customersFoodChoosingTime = 4f;
	[SerializeField] float foodSpawnTime = 10f;
	[SerializeField] float newCustomerTime = 8f;
	[SerializeField] Vector2Int money = default;
	private int score = 0;

	private List<Table> tables = new List<Table>();
	private List<Table> freeTables = new List<Table>();
	private List<Transform> foodSpawnPoints = new List<Transform>();

	[HideInInspector] public static int maxQueueLength = 1;
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

	#region Customers
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

	// Player select customers
	public void SelectCustomer(CustomersCluster customersCluster)
	{
		selectedCustomers = customersCluster;
		foreach (var table in freeTables)
		{
			table.Enable();
		}
	}

	// Player choose table for customers
	public CustomersCluster ChooseTable(Table table)
	{
		CustomersCluster ret = selectedCustomers;
		foreach (Table freeTable in freeTables)
		{
			freeTable.Disable();
		}
		selectedCustomers.AssignToTable(table);
		freeTables.Remove(table);
		selectedCustomers = null;
		clusterManager.MoveClusters();
		StartCoroutine(TimeToOrder(table));
		return ret;
	}

	// Klienci wybierają jedzenie
	IEnumerator TimeToOrder(Table table)
	{
		yield return new WaitForSeconds(customersFoodChoosingTime);
		table.ActivateOrder();
	}
	
    public void OrderFood(int id, int quatity)
    {
        StartCoroutine(SpawnFood(id, quatity));
    }
	
	//Spawn food after set time
    IEnumerator SpawnFood(int id, int quatity)
    {
        yield return new WaitForSeconds(foodSpawnTime);
		for (int i = 0; i < quatity; i++)
		{
			GameObject go = Instantiate(foodPrefab, foodSpawnPoints[id].position + new Vector3(0,i * 0.5f,0), Quaternion.identity);
			go.GetComponent<FoodScript>().SetColor(tables[id].color);
		}
	}

	public void FreeTable(Table table)
	{
		freeTables.Add(table);
		if (selectedCustomers != null)
		{
			table.Enable();
		}
		ChangeScore(10);
	}
	#endregion

	public void ChangeScore(int change)
	{
		score += change;
		moneyUI.text = score.ToString();
	}

}
