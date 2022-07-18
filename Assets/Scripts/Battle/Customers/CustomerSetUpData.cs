using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Customers/SetUpData")]
public class CustomerSetUpData : ScriptableObject
{
    public List<CustomerSetUp> customerSetUps;
    public List<CustomerData> customerDatas;
}

[System.Serializable]
public struct CustomerSetUp
{
    public Vector2 customerPosition;
    public int customerYRotate;
    public Vector2 notePosition;
    public int noteYRotate;
}