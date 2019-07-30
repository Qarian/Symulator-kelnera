using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Customer : MonoBehaviour
{
	NavMeshAgent agent;
	[HideInInspector] public CustomersCluster cluster;
	[HideInInspector] public Interactive interactiveComponent;

	private void Begin()
	{
		agent = GetComponent<NavMeshAgent>();
		interactiveComponent = GetComponent<Interactive>();
		interactiveComponent.SetAction(InteractiveFunction);
	}

	// Function to run when player interact with customer
	void InteractiveFunction()
	{
		Debug.Log("customer selected");
		cluster.SelectCustomer();
	}

	// Set destination of NavMesh using Transform component
	public void SetDestination(Transform transform)
	{
		if(agent == null)
			Begin();
		agent.SetDestination(transform.position);
	}
}
