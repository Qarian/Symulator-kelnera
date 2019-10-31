using UnityEngine;

public class Table : MonoBehaviour
{
	public Transform chairPositions = default;
	public TableDetector tableDetector = default;
	[SerializeField] GameObject orderSphere = default;
	[SerializeField] GameObject mesh = default;
	Interactive tableInteractive;
	CustomersCluster sittingCustomers;

	[HideInInspector] public int id = 0;
	public Color color = default;

	private void Start()
	{
        SetColor();

        Interactive interactiveSphere = orderSphere.AddComponent<Interactive>();
        interactiveSphere.SetAction(PlaceOrder);
		tableInteractive = mesh.AddComponent<Interactive>();
        tableInteractive.SetAction(SitCustomers);
        Disable();
		orderSphere.SetActive(false);
	}

	// Set color when changed value in inspector
	private void OnValidate()
	{
		SetColor();
	}

	#region customers
	// Put customers at table
	void SitCustomers()
	{
        sittingCustomers = CustomersManager.singleton.ChooseTable(this);
	}

	// Customers have chosen the meal
	public void ActivateOrder()
	{
		orderSphere.SetActive(true);
	}

	// Place order, function to add to interactive object
	private void PlaceOrder()
	{
        orderSphere.SetActive(false);
        Debug.Log(sittingCustomers);
        CustomersManager.singleton.OrderFood(id, sittingCustomers.numberOfCustomers);
	}
	
	public void EatFood(Rigidbody food)
	{
		Destroy(food.gameObject);
		if(sittingCustomers.EatFood())
		{
            CustomersManager.singleton.FreeTable(this);
		}
	}
	#endregion

	public void Enable()
	{
        tableInteractive.active = true;
	}

	public void Disable()
	{
        tableInteractive.active = false;
	}

	// Set color of the table
	private void SetColor()
	{
		ColorScript.SetColor(orderSphere, color);
		ColorScript.SetColor(mesh, color);
	}
}
