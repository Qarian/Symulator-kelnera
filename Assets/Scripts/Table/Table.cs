using UnityEngine;

public class Table : MonoBehaviour
{
	public Transform chairPositions = default;
	[SerializeField] GameObject orderSphere = default;
	[SerializeField] GameObject mesh = default;
	[SerializeField] private TableDetector tableDetector = default;
	Interactive tableInteractive;
	
	public CustomersCluster SittingCustomers { get; private set; }

	public Interactive OrderSphereInteractive { get; private set; }
	public TableDetector TableDetector => tableDetector;

	[HideInInspector] public Order currentOrder;

	private void Start()
	{
		OrderSphereInteractive = orderSphere.AddComponent<Interactive>();
		tableInteractive = mesh.AddComponent<Interactive>();
        tableInteractive.SetAction(SitCustomers);
        Disable();

        orderSphere.SetActive(false);
		tableDetector.gameObject.SetActive(false);
	}
	
	// Put customers at table
	void SitCustomers()
	{
        SittingCustomers = CustomersManager.singleton.ChooseTable(this);
        currentOrder = OrdersManager.NewOrder(this);
	}

	public void ResetTable()
	{
		if (!(currentOrder is null))
			currentOrder.CancelOrder();
		tableDetector.gameObject.SetActive(false);
		orderSphere.SetActive(false);
		SittingCustomers = null;
		CustomersManager.singleton.FreeTable(this);
	}

	public void Enable()
	{
        tableInteractive.active = true;
	}

	public void Disable()
	{
        tableInteractive.active = false;
	}
}
