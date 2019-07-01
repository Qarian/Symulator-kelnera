using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Customer : MonoBehaviour
{
	NavMeshAgent agent;
	[HideInInspector] public CustomersCluster cluster;
	[HideInInspector] public Interactive interactiveComponent;

	private void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		interactiveComponent = GetComponent<Interactive>();
		interactiveComponent.SetAction(InteractiveFunction);
	}

	void InteractiveFunction()
	{
		cluster.SelectCustomer();
	}

	public void SetDestination(Transform transform)
	{
		agent.SetDestination(transform.position);
	}

	public void SetDestination(Vector3 position)
	{
		agent.SetDestination(position);
	}
}
