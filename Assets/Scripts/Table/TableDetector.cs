using System.Collections.Generic;
using UnityEngine;

public class TableDetector : MonoBehaviour
{
	[SerializeField] private Table tableComponent = default;

	// References to food components
	private List<FoodScript> targetFoodScripts = new List<FoodScript>();

	private void Update()
	{
		// if food is on table, check if it isn't moving
		foreach (FoodScript food in targetFoodScripts)
		{
			if (food.rigidbody.velocity == Vector3.zero)
			{
				FoodOnTable(food);
				break;
			}
		}
	}

	// Function to run when food is on table
	private void FoodOnTable(FoodScript food)
	{
		targetFoodScripts.Remove(food);
		tableComponent.EatFood(food);
	}

	// Check if food entered trigger
	private void OnTriggerEnter(Collider collider)
	{
		FoodScript target = collider.GetComponent<FoodScript>();
		if (target && target.color == tableComponent.color)
		{
			targetFoodScripts.Add(target);
		}
	}

	// Check if food exit trigger
	private void OnTriggerExit(Collider other)
	{
		FoodScript removedFood = other.GetComponent<FoodScript>();
		if (targetFoodScripts.Contains(removedFood))
		{
			targetFoodScripts.Remove(removedFood);
		}
	}
}
