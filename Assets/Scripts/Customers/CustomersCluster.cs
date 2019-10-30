using UnityEngine;

public class CustomersCluster : MonoBehaviour
{
	[SerializeField] GameObject customerPrefab = default;

	private Transform[] customersTransforms;
	private Customer[] customers;
	[HideInInspector] public int numberOfCustomers;
	private int orderInQueue;
	private int foodsEaten = 0;

	// Instantiate new cluster of customers
	public void Create(int numberOfCustomers, int order)
	{
		this.orderInQueue = order;

		// Get positions for customers
		this.numberOfCustomers = numberOfCustomers;
		Debug.Log("Created " + numberOfCustomers + " new customers");
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
		transform.position = Queue.queuePositions[order];
		for (int i = 0; i < numberOfCustomers; i++)
		{
			customers[i].SetDestination(customersTransforms[i]);
		}
	}

	// Move cluster to next position in queue
	public void MoveCustomers()
	{
		orderInQueue--;
		if (orderInQueue >= 0)
		{
			transform.position = Queue.queuePositions[orderInQueue];
			for (int i = 0; i < numberOfCustomers; i++)
			{
				customers[i].SetDestination(customersTransforms[i]);
			}
		}
	}

	// disable interactive component on all customers
	public void SelectCustomer()
	{
		if (CustomersManager.selectedCustomers == null)
		{
            CustomersManager.singleton.SelectCustomer(this);
			for (int i = 0; i < numberOfCustomers; i++)
			{
				customers[i].interactiveComponent.active = false;
			}
		}
	}

	// Assign every customer their place at table
	public void AssignToTable(Table table)
	{
		for (int i = 0; i < numberOfCustomers; i++)
		{
			customers[i].SetDestination(table.chairPositions.GetChild(i));
		}
		transform.position = table.transform.position;
	}

	// Customer eats food. returns true if it was last food
	public bool EatFood()
	{
		foodsEaten++;
		if(foodsEaten == numberOfCustomers)
			return true;
		return false;
	}

    void DeleteCustomers()
    {
        for (int i = 0; i < numberOfCustomers; i++)
        {
            Destroy(customers[i].gameObject);
        }
    }
}
