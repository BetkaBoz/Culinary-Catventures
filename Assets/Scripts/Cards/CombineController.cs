using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombineController : MonoBehaviour
{
    public TextAsset comboListJson;
    [SerializeField] private ComboList comboList = new ComboList();
    [SerializeField] private CardSlot findSlot;
    [SerializeField] private GameManager gm;
    //[SerializeField] private ManouverTargetController targetController; 
    [SerializeField] private Button combineBttn;

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
            //targetController.setPos(false);
            combineBttn.gameObject.SetActive(true);
            combineBttn.interactable = false;
        }
        else
        {
            //targetController.setPos(true);
            combineBttn.interactable = false;
            combineBttn.gameObject.SetActive(false);
            findSlot.gameObject.SetActive(false);
            this.gameObject.SetActive(false);
        }
    }

    //Functions calls LookForCard to see if the selected combination is in the JSON file
    //If yes then show found card 
    public void FindCard(string[] find)
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
        Card foundCard = null;
        if (found != "NOTHING")
        {
            Debug.Log(found);
            //Right now we can only combine food, might change it later if we decide to combine manouvers
            foundCard = Resources.Load<FoodBase>("Scriptable Objects/" + found);
            if (foundCard != null)
            {
                if (!findSlot.gameObject.activeSelf)
                {
                    findSlot.gameObject.SetActive(true);
                }
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
    }

    //Functions checks if the combination of selected cards is in the JSON file
    //Apparently HashSets are the quickest way to compare arrays
    private string LookForCard(string[] find)
    {
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
        gm.CombineCards(findSlot.GetCard());
    }
}
