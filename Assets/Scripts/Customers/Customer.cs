using UnityEngine;
using UnityEngine.AI;
using System;

[RequireComponent(typeof(NavMeshAgent))]
public class Customer : MonoBehaviour
{
    NavMeshAgent agent;
    Interactive interactiveComponent;
    CustomersCluster cluster;

    Action action;

    public CustomersCluster Cluster {
        set { 
            cluster = value;
            interactiveComponent.SetAction(cluster.SelectCustomer);
        }
    }

    private void Awake()
	{
		agent = GetComponent<NavMeshAgent>();
		interactiveComponent = GetComponent<Interactive>();
	}

    public void Update()
    {
        if (!(action is null) && agent.remainingDistance > 0f && agent.remainingDistance < 1f)
        {
            action();
            action = null;
        }
    }

    public void Disable()
    {
        interactiveComponent.active = false;
    }

    // Set destination of NavMesh using Transform component
    public void SetDestination(Transform transform, Action action = null)
    {
        agent.SetDestination(transform.position);
        this.action = action;
    }

}
