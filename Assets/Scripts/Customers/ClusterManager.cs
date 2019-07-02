using UnityEngine;
using System.Collections.Generic;

public class ClusterManager : MonoBehaviour
{
	[SerializeField] GameObject customerCluster = default;
	private List<CustomersCluster> clusters = new List<CustomersCluster>();
	[HideInInspector] public int waitingClusters = 0;

	public static Vector3[] queuePositions;

	int customersToSpawn = 2;

	public void GenerateNewCluster()
	{
		// get queue positions
		if (queuePositions == null)// first time
		{
			Transform queueContainer = GameObject.FindGameObjectWithTag("Queue").transform;
			int queuePointsNumber = queueContainer.childCount;
			GameManager.maxQueueLength = queuePointsNumber;
			queuePositions = new Vector3[queuePointsNumber];
			for (int i = 0; i < queuePointsNumber; i++)
			{
				queuePositions[i] = queueContainer.GetChild(i).position;
			}
		}

		CustomersCluster cluster = Instantiate(customerCluster).GetComponent<CustomersCluster>();
		clusters.Add(cluster);
		cluster.Instantiate(customersToSpawn, waitingClusters);//(Random.Range(1, 5), waitingClusters);
		customersToSpawn++;
		if (customersToSpawn > 4)
			customersToSpawn = 1;
		waitingClusters++;
	}

	public void MoveClusters()
	{
		waitingClusters--;
		foreach (CustomersCluster cluster in clusters)
		{
			cluster.MoveCustomers();
		}
	}
}
