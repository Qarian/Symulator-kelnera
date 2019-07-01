using UnityEngine;

public class TableDetector : MonoBehaviour
{
	[SerializeField] Table tableComponent = default;

    Rigidbody targetFoodRb;
	FoodScript targetFoodScript;

	private void Update()
	{
		if (targetFoodRb != null)
		{
			if (targetFoodScript.taken == false && targetFoodRb.velocity == Vector3.zero)
				FoodOnTable();
		}
	}
	
    private void FoodOnTable()
    {

	}

    private void OnTriggerEnter(Collider collider)
    {
		targetFoodScript = collider.GetComponent<FoodScript>();
		if (targetFoodScript != null && targetFoodScript.color == tableComponent.color)
		{
			targetFoodRb = collider.GetComponent<Rigidbody>();
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if(other.GetComponent<Rigidbody>() == targetFoodRb)
		{
			targetFoodRb = null;
			targetFoodScript = null;
		}
	}

}
