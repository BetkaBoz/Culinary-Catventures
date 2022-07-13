using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class CustomerFactory
{
    private static Dictionary<string, Type> customersByName;

    public CustomerFactory()
    {

        var customerTypes = Assembly.GetAssembly(typeof(Customer)).GetTypes()
            .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Customer)));

        customersByName = new Dictionary<string, Type>();

        foreach (var type in customerTypes)
        {
            var customer = Activator.CreateInstance(type) as Customer;
            customersByName.Add(customer.Name, type);
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

    internal static IEnumerable<string> GetCustomerNames()
    {
        return customersByName.Keys;
    }

}
