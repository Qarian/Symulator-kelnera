using System.Collections;
using UnityEngine;

public class Order
{
    public Color Color { get; }

    private Table table;
    private int id;
    private int foodToEat;

    public Order(Color color, int id, Table table)
    {
        this.table = table;
        Color = color;
        this.id = id;
        foodToEat = table.SittingCustomers.numberOfCustomers;

        table.OrderSphereInteractive.SetAction(PlaceOrder);
        ColorScript.SetColor(table.OrderSphereInteractive.gameObject, color);
    }

    public IEnumerator PreparingOrder()
    {
        yield return new WaitForSeconds(CustomersManager.singleton.customersFoodChoosingTime);
        
        table.OrderSphereInteractive.gameObject.SetActive(true);
    }

    // Place order, function to add to interactive object
    private void PlaceOrder()
    {
        table.OrderSphereInteractive.gameObject.SetActive(false);
        table.TableDetector.AssignOrder(id, FoodOnTable);
        
        FoodSpawner.singleton.OrderCubeFood(Color, table.SittingCustomers.numberOfCustomers, id);
    }

    private void FoodOnTable(FoodScript foodScript)
    {
        // TODO: Pooling for food
        Object.Destroy(foodScript.gameObject);

        foodToEat--;
        if (foodToEat <= 0)
        {
            OrdersManager.CompleteOrder(Color);
            GameManager.singleton.RunCoroutine(CustomersManager.singleton.WaitForEating(table.SittingCustomers));
            table.TableDetector.gameObject.SetActive(false);
        }
    }

    public void CancelOrder()
    {
        OrdersManager.CompleteOrder(Color);
        table.TableDetector.gameObject.SetActive(false);
    }
}
