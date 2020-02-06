using UnityEngine;
using System.Collections.Generic;

public class Queue : MonoBehaviour
{
	[SerializeField] GameObject customerCluster = default;
    [SerializeField] GameObject barrier = default;
	
	public static Vector3[] queuePositions;

    private List<CustomersCluster> clusters = new List<CustomersCluster>();

    int maxWaitingClusters;
	int currentWaitingClusters = 0;

	private void Start()
	{
		barrier.SetActive(false);
	}

	// Instantiate cluster of 1 - 4 customers 
	public bool GenerateNewCluster()
	{
		// get queue positions
		if (queuePositions == null)// first time
		{
			maxWaitingClusters = transform.childCount;
			queuePositions = new Vector3[maxWaitingClusters];
			for (int i = 0; i < maxWaitingClusters; i++)
			{
				queuePositions[i] = transform.GetChild(i).position;
			}
		}

		if (currentWaitingClusters == maxWaitingClusters)
            return false;

		CustomersCluster cluster = Instantiate(customerCluster, queuePositions[maxWaitingClusters-1], Quaternion.identity).GetComponent<CustomersCluster>();
		clusters.Add(cluster);
		cluster.Create(Random.Range(1, 5), currentWaitingClusters);
        currentWaitingClusters++;
        return true;
	}

	// Move all clusters in queue 
	public void TakeCluster(CustomersCluster takenCustomer)
	{
        currentWaitingClusters--;
		Debug.Log(takenCustomer.numberOfCustomers);
		clusters.Remove(takenCustomer);
		foreach (CustomersCluster cluster in clusters)
		{
			cluster.MoveCustomers();
		}
	}

    public void CloseQueue()
    {
        foreach (CustomersCluster cluster in clusters)
        {
			Debug.Log(cluster.numberOfCustomers);

			cluster.LeaveRestaurant();
        }
        barrier.SetActive(true);
    }
}
