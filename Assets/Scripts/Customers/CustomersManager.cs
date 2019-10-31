using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class CustomersManager : MonoBehaviour
{
    [Header("References")]
    public Transform exit = default;
    [SerializeField] Transform tablesTransform = default;
    [SerializeField] Transform foodSpawnPointsTransform = default;
    [SerializeField] GameObject foodPrefab = default;
    [SerializeField] Queue queue = default;

    [Space]
    [Header("Times")]
    [SerializeField] float customersFoodChoosingTime = 4f;
    [SerializeField] float foodSpawnTime = 10f;
    public float customersEatingTime = 5f;
    [SerializeField] float newCustomerTime = 8f;

    public static CustomersCluster selectedCustomers;

    private List<Table> tables = new List<Table>();
    [HideInInspector] public List<Table> freeTables = new List<Table>();
    private List<Transform> foodSpawnPoints = new List<Transform>();

    public static CustomersManager singleton;
    public void Awake()
    {
        singleton = this;
    }

    private void Start()
    {
        for (int i = 0; i < tablesTransform.childCount; i++)
        {
            tables.Add(tablesTransform.GetChild(i).GetComponent<Table>());
            freeTables.Add(tables[i]);
            tables[i].id = i;
        }
        foreach (Transform child in foodSpawnPointsTransform)
        {
            foodSpawnPoints.Add(child);
        }

        //Start spawning customers
        StartCoroutine(NewCustomers());
    }

    IEnumerator NewCustomers()
    {
        while (true)
        {
            queue.GenerateNewCluster();
            yield return new WaitForSeconds(newCustomerTime);
        }
    }

    // Player select customers
    public void SelectCustomer(CustomersCluster customersCluster)
    {
        selectedCustomers = customersCluster;
        foreach (var table in freeTables)
        {
            table.Enable();
        }
    }

    // Player choose table for customers
    public CustomersCluster ChooseTable(Table table)
    {
        foreach (Table freeTable in freeTables)
        {
            freeTable.Disable();
        }
        CustomersCluster ret = selectedCustomers;
        selectedCustomers.AssignToTable(table);
        freeTables.Remove(table);
        selectedCustomers = null;
        queue.MoveClusters();
        StartCoroutine(TimeToOrder(table));
        return ret;
    }

    // Customers choose meal
    IEnumerator TimeToOrder(Table table)
    {
        yield return new WaitForSeconds(customersFoodChoosingTime);
        table.ActivateOrder();
    }

    public void OrderFood(int id, int quatity)
    {
        StartCoroutine(SpawnFood(id, quatity));
    }

    //Spawn food after set time
    IEnumerator SpawnFood(int id, int quatity)
    {
        yield return new WaitForSeconds(foodSpawnTime);
        for (int i = 0; i < quatity; i++)
        {
            GameObject go = Instantiate(foodPrefab, foodSpawnPoints[id].position + new Vector3(0, i * 0.5f, 0), Quaternion.identity);
            go.GetComponent<FoodScript>().SetColor(tables[id].color);
        }
    }

    public void FreeTable(Table table)
    {
        freeTables.Add(table);
        if (selectedCustomers != null)
        {
            table.Enable();
        }
        GameManager.singleton.ChangeScore(10);
    }
}
