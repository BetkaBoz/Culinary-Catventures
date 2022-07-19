using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class CustomerFactory
{
    private Dictionary<string, Type> customersByName;

    public CustomerFactory(CustomerSetUpData customerSetUpData)
    {
        var customerTypes = Assembly.GetAssembly(typeof(Customer)).GetTypes()
            .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Customer)));

        customersByName = new Dictionary<string, Type>();

        //get number of customer types and add their names
        for (int i = 0; i < customerTypes.Count(); i++)
        {
            customersByName.Add(customerSetUpData.customerDatas[i].Name, customerTypes.ElementAt(i));
        }
    }

    public Customer GetCustomer(string customerType)
    {
        if (customersByName.ContainsKey(customerType))
        {
            Type type = customersByName[customerType];
            var customer = Activator.CreateInstance(type) as Customer;
            return customer;
        }
        return null;
    }

    internal IEnumerable<string> GetCustomerNames()
    {
        return customersByName.Keys;
    }

}
