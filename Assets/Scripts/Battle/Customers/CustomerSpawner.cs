using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] CustomerSetUpData data;
    [SerializeField] Transform customerParent;
    [SerializeField] Transform noteParent;
    [SerializeField] CustomerView customerViewPrefab;
    [SerializeField] NoteView noteViewPrefab;
    [SerializeField] GameManager gm;

    CustomerFactory customerFactory;

    public void Start()
    {
        customerFactory = new CustomerFactory(data);
        List<CustomerData> customerDatas = data.customerDatas;
        List<CustomerSetUp> customerSetUps = data.customerSetUps;

        Clear();

        //spawn random customer and note at define parent position
        foreach(CustomerSetUp customerSetUpsElement in customerSetUps)
        {
            int randomIndex = UnityEngine.Random.Range(0, customerDatas.Count);

            CustomerData customerData = customerDatas[randomIndex];

            Customer customer = customerFactory.GetCustomer(customerData.Name);

            customer.SetUp(gm, customerData);

            Instantiate(customerViewPrefab, customerParent).SetUp(customer, customerSetUpsElement);
            Instantiate(noteViewPrefab, noteParent).SetUp(customer, customerSetUpsElement);
        }
    }

    private void Clear()
    {
        for(int i = customerParent.childCount-1; i >= 0; i--)
        {
            Destroy(customerParent.GetChild(i).gameObject);
        }

        for (int i = noteParent.childCount - 1; i >= 0; i--)
        {
            Destroy(noteParent.GetChild(i).gameObject);
        }
    }
}
