using UnityEngine;

public class TableDetector : MonoBehaviour
{
	[SerializeField] Table tableComponent = default;

	// References to food components
    Rigidbody targetFoodRb;
	FoodScript targetFoodScript;

	private void Update()
	{
		// if food is on table, check if it isn't moving
		if (targetFoodRb)
		{
			if (targetFoodScript.taken == false && targetFoodRb.velocity == Vector3.zero)
				FoodOnTable();
		}
	}
	
	// Function to run when food is on table
    private void FoodOnTable()
    {
		tableComponent.EatFood(targetFoodRb);
	}

	// Check if food entered trigger
    private void OnTriggerEnter(Collider collider)
    {
		targetFoodScript = collider.GetComponent<FoodScript>();
		if (targetFoodScript && targetFoodScript.color == tableComponent.color)
		{
			targetFoodRb = collider.GetComponent<Rigidbody>();
		}
	}

	// Check if food exit trigger
	private void OnTriggerExit(Collider other)
	{
		if(other.GetComponent<Rigidbody>() == targetFoodRb)
		{
			targetFoodRb = null;
			targetFoodScript = null;
		}
	}
}
