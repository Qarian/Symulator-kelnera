using UnityEngine;

public class Table : MonoBehaviour
{
	public Transform positions = default;
	public TableDetector tableDetector = default;
	[SerializeField] Interactive interactiveComponent = default;
	[SerializeField] GameObject sphere = default;
	[SerializeField] GameObject mesh = default;
	[HideInInspector] CustomersCluster customersAtTable;
	Interactive tableInteractive;
	CustomersCluster sittingCustomers;

	[HideInInspector] public int id = 0;
	public Color color = default;

	public CustomersCluster SittingCustomers {set => sittingCustomers = value; }

	private void Start()
	{
		SetColor();

		Interactive interactive = sphere.AddComponent(typeof(Interactive)) as Interactive;
		interactive.SetAction(PlaceOrder);
		tableInteractive = mesh.GetComponent<Interactive>();
		tableInteractive.SetAction(SitCustomers);
		sphere.SetActive(false);
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
		customersAtTable = GameManager.singleton.ChooseTable(this);
	}

	// Customers have chosen the meal
	public void ActivateOrder()
	{
		sphere.SetActive(true);
	}

	// Place order, function to add to interactive object
	private void PlaceOrder()
	{
		sphere.SetActive(false);
		GameManager.singleton.OrderFood(id, customersAtTable.numberOfCustomers);
	}
	
	public void EatFood(Rigidbody food)
	{
		Destroy(food.gameObject);
		if(customersAtTable.EatFood())
		{
			GameManager.singleton.FreeTable(this);
		}
	}
	#endregion

	public void Enable()
	{
		interactiveComponent.active = true;
	}

	public void Disable()
	{
		interactiveComponent.active = false;
	}

	// Set color of the table
	private void SetColor()
	{
		ColorScript.SetColor(sphere, color);
		ColorScript.SetColor(mesh, color);
	}
}
