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
	[SerializeField] Transform customerTransform = default;
	[SerializeField] GameObject customerPrefab = default;

	[HideInInspector] public GameObject customer;
	[HideInInspector] public bool customerSelected = false;
	private Interactive customerInteractive;

	[Space]
	[SerializeField] float foodSpawnTime = 10f;
	[SerializeField] float newCustomerTime = 8f;
	private int score = 0;

	List<TableDetector> tables = new List<TableDetector>();
	//[HideInInspector] public List<TableDetector> freeTables = new List<TableDetector>();
	List<Transform> foodSpawnPoints = new List<Transform>();

	public static GameManager singleton;
    private void Awake()
    {
        singleton = this;
    }

    private void Start()
    {
		for (int i = 0; i < tablesTransform.childCount; i++)
		{
			tables.Add(tablesTransform.GetChild(i).GetChild(0).GetComponent<TableDetector>());
			//freeTables.Add(tables[i]);
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
			/*if (freeTables.Count > 0)
			{
				int tableId = Random.Range(0, freeTables.Count);
				freeTables[tableId].ActivateOrder();
				freeTables.RemoveAt(tableId);
			}*/
			if(customer == null)
			{
				customer = Instantiate(customerPrefab, customerTransform.position, Quaternion.identity);
			 	customerInteractive = customer.GetComponent<Interactive>();
				customerInteractive.SetAction(SelectCustomer);
			}
			yield return new WaitForSeconds(newCustomerTime);
		}
	}

	void SelectCustomer()
	{
		customerSelected = true;
		Destroy(customerInteractive);
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
