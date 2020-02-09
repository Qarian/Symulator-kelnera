using UnityEngine;
using UnityEngine.AI;
using System;

[RequireComponent(typeof(NavMeshAgent))]
public class Customer : MonoBehaviour
{
    private NavMeshAgent agent;
    private Interactive interactiveComponent;
    private CustomersCluster cluster;

    private Action action;

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
        if (!(action is null) &&
            !agent.pathPending &&
            agent.remainingDistance < agent.stoppingDistance)
        {
            action();
            action = null;
        }
    }

    public void Disable()
    {
        interactiveComponent.active = false;
    }

    public void GoToTable(Transform chairTransform)
    {
        SetDestination(chairTransform, () => cluster.CustomerArrivedAtTable());
    }

    // Set destination of NavMesh using Transform component
    public void SetDestination(Transform transform, Action action = null)
    {
        agent.SetDestination(transform.position);
        this.action = action;
    }

}
