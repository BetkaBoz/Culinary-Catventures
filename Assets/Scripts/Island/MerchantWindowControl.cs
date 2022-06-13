using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MerchantWindowControl : MonoBehaviour
{
    [SerializeField] private List<GameObject> merchantCards;
    [SerializeField] private List<CardBaseInfo> allScriptableObjects;
    // Start is called before the first frame update
    
    void Start()
    {
        GetAllFoodCards();
        GenerateCards();
    }   
/*
    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 70, 150, 30), "Change texture"))
        {
            // change texture on cube
            Texture2D texture = (Texture2D)textures[Random.Range(0, textures.Length)];
            go.GetComponent<Renderer>().material.mainTexture = texture;
        }
    }
    */
    
    // Update is called once per frame
    
    private void GetAllFoodCards()
    {
        CardBaseInfo[] allScriptableObjectsTemp = Resources.LoadAll<CardBaseInfo>("Scriptable Objects/");
        foreach (var t in allScriptableObjectsTemp)
        {
            if (t.CardType != "Manoeuvre" )
            {
                allScriptableObjects.Add(t);
            }
        }
    }
    private void GenerateCards()
    {
        foreach (GameObject card in merchantCards)
        {
            Sprite artwork = card.GetComponent<Image>().sprite;
            Text nutritionalValue = card.GetComponentInChildren<Text>();
            TextMeshProUGUI price = card.GetComponentInChildren<TextMeshProUGUI>();

            //artwork = baseCard.Artwork;
            nutritionalValue.text = "12";
            price.text = "5";

        }
    }
    
    /*public void GetDataFromBase(CardBaseInfo baseCard)
    {
        
        foreach (GameObject card in merchantCards)
        {   
            
            
            if(baseCard.CardType == "Manoeuvre")
                cardEffect = baseCard.CardEffect;
            cardType = baseCard.CardType;
            cardName = baseCard.CardName;
            canTarget = baseCard.CanTarget;
            energyCost = baseCard.EnergyCost;
            nutritionPoints = baseCard.NutritionPoints;
        }
        
        

        //if(cardEffect != null)
        //{
        //    cardEffect.Amount = baseCard.Amount;
        //    cardEffect.DiscardAmount = baseCard.DiscardAmount;
        //}
    }*/
}
