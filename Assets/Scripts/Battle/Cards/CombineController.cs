using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombineController : MonoBehaviour
{
    public TextAsset comboListJson;
    [SerializeField] ComboList comboList = new ComboList();
    [SerializeField] CardSlot findSlot;
    [SerializeField] GameManager gm;
    //[SerializeField] private ManouverTargetController targetController; 
    [SerializeField] Button combineBttn;
    [SerializeField] GameObject cardPrefab;
    [SerializeField] Text combineText;
    Card result = null;

    //This class is used to load individual entries in the JSON file
    [System.Serializable]
    public class ComboInfo
    {
        public int numOfCards;
        public string[] cardsUsed;
        public string cardProduced;

        public string[] GetcardsUsed()
        {
            return cardsUsed;
        }

        public string GetcardProduced()
        {
            return cardProduced;
        }

        public int GetNumOfCards()
        {
            return numOfCards;
        }

        public override string ToString()
        {
            return "Length: " + numOfCards.ToString() + "cards used"+ cardsUsed + "produced" + cardProduced;
        }
    }

    //This class stores every entry from the JSON file containing all of the card combinations
    [System.Serializable]
    public class ComboList
    {
        public ComboInfo[] comboInfo;
    }

    public void Start()
    {
        //right now it's here because it's simple to use
        //maybe make this more secure later
        comboList = JsonUtility.FromJson<ComboList>(comboListJson.text);
    }

    //public void testFind()
    //{
    //    Debug.Log();
    //}

    //Make it so DiscardLayer (change the name later), is set to combine mode
    public void ToggleCombine()
    {
        if (!gm.combinePhase)
        {
            this.gameObject.SetActive(true);
            combineText.gameObject.SetActive(true);
            //targetController.setPos(false);
            combineBttn.onClick.RemoveAllListeners();
            combineBttn.onClick.AddListener(CombineCard);
            combineBttn.interactable = false;
        }
        else
        {
            //targetController.setPos(true);
            combineBttn.interactable = false;
            result = null;
            findSlot.gameObject.SetActive(false);
            this.gameObject.SetActive(false);
            combineText.gameObject.SetActive(false);
        }
    }

    //Functions calls LookForCard to see if the selected combination is in the JSON file
    //If yes then show found card 
    public void FindCard(string[] find, string[] types)
    {
        //This is here because there are no 1 card combination
        //Also it will reset the shown card (aka hides it)
        if (find.Length <= 1)
        {
            if (findSlot.gameObject.activeSelf)
            {
                findSlot.gameObject.SetActive(false);
            }
            combineBttn.interactable = false;
            return;
        }
        string found = LookForCard(find);
        Debug.Log(found);
        if (found != "NOTHING")
        {
            LoadPreviewCard(found);
        }
        else
        {
            Debug.Log("Yo I'm here");
            switch (GetPileType(types))
            {
                case "Meat":
                    LoadPreviewCard("MeatPile");
                    break;
                case "Vegetarian":
                    LoadPreviewCard("VeggiePile");
                    break;
                default:
                    LoadPreviewCard("FoodPile");
                    break;
            }
            result.NutritionPoints = gm.GetComboNP(true);
            result.EnergyCost = gm.GetComboEnergy();
            findSlot.SetCard(result);
            //if (findSlot.gameObject.activeSelf)
            //{
            //    findSlot.gameObject.SetActive(false);
            //}
            //combineBttn.interactable = false;
        }
    }

    private void LoadPreviewCard(string target)
    {
        if (result != null)
            result.DeletionCheck(true);
        GameObject cardTemp;
        Card foundCard = null;
        CardBaseInfo foundCardBase = null;
        //Right now we can only combine food, might change it later if we decide to combine manouvers4
        foundCardBase = Resources.Load<CardBaseInfo>("Scriptable Objects/Food/" + target);
        if (foundCardBase != null)
        {
            if (!findSlot.gameObject.activeSelf)
            {
                findSlot.gameObject.SetActive(true);
            }
            cardTemp = Instantiate(cardPrefab);
            foundCard = cardTemp.GetComponent<Card>();
            foundCard.GetDataFromBase(foundCardBase);
            findSlot.SetCard(foundCard);
            combineBttn.interactable = true;
        }
        else
        {
            //This is here just in case (program never got here so far)
            if (findSlot.gameObject.activeSelf)
            {
                findSlot.gameObject.SetActive(false);
            }
            combineBttn.interactable = false;
        }
        Debug.Log(foundCard);
        result = foundCard;
    }

    private string GetPileType(string[] types)
    {
        bool isMeat = false;
        bool isVeggie = false;
        for(int i = 0; i < types.Length; i++)
        {
            if (types[i] == "Meat")
                isMeat = true;
            if (types[i] == "Vegetarian")
                isVeggie = true;
            if (types[i] == "Mix")
            {
                isVeggie = true;
                isMeat = true;
                break; //no need to look further we know it will be mixed food
            }
        }
        if (isMeat == isVeggie)
            return "Mix";
        if (isMeat)
            return "Meat";
        return "Vegetarian";
    }

    //Functions checks if the combination of selected cards is in the JSON file
    //Apparently HashSets are the quickest way to compare arrays
    private string LookForCard(string[] find)
    {
        //return "NOTHING"; //This is here until we finish rest of combo cards
        HashSet<string> target = new HashSet<string>(find);
        HashSet<string> usedCards;
        
        //for (int i = 0; i < find.Length; i++)
        //{
        //    Debug.Log(find[i]);
        //}

        for (int i = 0; i < comboList.comboInfo.Length; i++)
        {
            usedCards = new HashSet<string>(comboList.comboInfo[i].GetcardsUsed());

            if (usedCards.SetEquals(target))
            {
                return comboList.comboInfo[i].GetcardProduced();
            }
            //for (int j = 0; j < comboList.comboInfo[i].GetNumOfCards(); j++)
            //{
            //}
        }
        return "NOTHING";
    }

    //Combines the card and sends it to players hand
    public void CombineCard()
    {
        gm.CombineCards(result);
    }
}
