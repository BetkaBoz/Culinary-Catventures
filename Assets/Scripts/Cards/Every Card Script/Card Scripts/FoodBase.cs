using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Food", menuName ="Cards/Food")]
public class FoodBase : Card
{
    [SerializeField] private string foodType;
    public string FoodType
    {
        get
        {
            return foodType;
        }
    }
    public override void CardEffect(GameManager gm, RaycastHit2D hit)
    {
        hit.transform.GetComponentInParent<Customer>().Feed((ushort)this.NutritionPoints);
    }
}
