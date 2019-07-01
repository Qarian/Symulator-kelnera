using UnityEngine;

public class CustomersCluster : MonoBehaviour
{
	[SerializeField] GameObject customerPrefab = default;

	private Transform[] customersTransforms;
	private Customer[] customers;
	private int numberOfCustomers;
	private int order;

	public void Instantiate(int numberOfCustomers, int order)
	{
		
		this.order = order;
		// Set container position
		transform.position = ClusterManager.queuePositions[order];

		// Get positions for customers
		this.numberOfCustomers = numberOfCustomers;
		customersTransforms = new Transform[numberOfCustomers];
		Transform tmp = transform.GetChild(numberOfCustomers - 1);
		for (int i = 0; i < numberOfCustomers; i++)
		{
			customersTransforms[i] = tmp.GetChild(i);
		}

		// Create customers
		customers = new Customer[numberOfCustomers];
		for (int i = 0; i < numberOfCustomers; i++)
		{
			customers[i] = Instantiate(customerPrefab, customersTransforms[i].position, Quaternion.identity, GameManager.singleton.transform).GetComponent<Customer>();
			customers[i].SetDestination(customersTransforms[i]);
			customers[i].cluster = this;
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
		foreach (var customer in customers)
		{
			customer.interactiveComponent.active = false;
		}
		GameManager.singleton.SelectCustomer();
	}

	public void AssignToTable(Table table)
	{
		for (int i = 0; i < numberOfCustomers; i++)
		{
			customers[i].SetDestination(table.positions.GetChild(i));
		}
		transform.position = table.transform.position;
	}
}
