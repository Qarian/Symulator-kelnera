using UnityEngine;

public class CustomersCluster : MonoBehaviour
{
	[SerializeField] GameObject customerPrefab = default;

	private Transform[] customersTransforms;
	private Customer[] customers;
	[HideInInspector] public int numberOfCustomers;
	private int order;
	private int foodsEaten = 0;

	public void Instantiate(int numberOfCustomers, int order)
	{
		this.order = order;
		// Set container position
		transform.position = ClusterManager.queuePositions[GameManager.maxQueueLength - 1];

		// Get positions for customers
		this.numberOfCustomers = numberOfCustomers;
		customersTransforms = new Transform[numberOfCustomers];
		Transform tmp = transform.GetChild(0).GetChild(numberOfCustomers - 1);
		for (int i = 0; i < numberOfCustomers; i++)
		{
			customersTransforms[i] = tmp.GetChild(i);
		}

		// Create customers
		customers = new Customer[numberOfCustomers];
		for (int i = 0; i < numberOfCustomers; i++)
		{
			customers[i] = Instantiate(customerPrefab, customersTransforms[i].position, Quaternion.identity, GameManager.singleton.transform).GetComponent<Customer>();
			customers[i].cluster = this;
		}

		// Move Customers to Start of queue
		transform.position = ClusterManager.queuePositions[order];
		for (int i = 0; i < numberOfCustomers; i++)
		{
			customers[i].SetDestination(customersTransforms[i]);
		}
	}

	public void MoveCustomers()
	{
		order--;
		if (order >= 0)
		{
			transform.position = ClusterManager.queuePositions[order];
			for (int i = 0; i < numberOfCustomers; i++)
			{
				customers[i].SetDestination(customersTransforms[i]);
			}
		}
	}

	public void SelectCustomer()
	{
		if (GameManager.selectedCustomers == null)
		{
			GameManager.singleton.SelectCustomer(this);
			foreach (var customer in customers)
			{
				customer.interactiveComponent.active = false;
			}
		}
	}

	public void AssignToTable(Table table)
	{
		for (int i = 0; i < numberOfCustomers; i++)
		{
			customers[i].SetDestination(table.positions.GetChild(i));
		}
		transform.position = table.transform.position;
	}

	// Customer eats food. returns true if it was last food
	public bool EatFood()
	{
		foodsEaten++;
		if(foodsEaten == numberOfCustomers)
		{
			foreach (var customer in customers)
			{
				Destroy(customer.gameObject);
			}
			return true;
		}
		return false;
	}
}
