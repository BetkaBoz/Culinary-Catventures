using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventWindowControl : MonoBehaviour
{
/*
    [SerializeField] private Sprite spriteEventImg;
    [SerializeField] private TextMeshProUGUI  eventWindowName;
    [SerializeField] private TextMeshProUGUI  eventWindowText;
    [SerializeField] private TextMeshProUGUI eventFirstButtonText;
    [SerializeField] private TextMeshProUGUI eventSecondButtonText;
    [SerializeField] private TextMeshProUGUI eventThirdButtonText;
*/

    #region Variables and getters
        [SerializeField] private GameObject  windowName;
        [SerializeField] private GameObject  windowText;
        [SerializeField] private GameObject  windowImage;
        [SerializeField] private GameObject  firstButton;
        [SerializeField] private GameObject  secondButton;
        [SerializeField] private GameObject  thirdButton;
        
        private IslandManager islandManager;
        private int timeCost = 1;

        public GameObject FirstButton => firstButton;
        public GameObject SecondButton => secondButton;
        public GameObject ThirdButton => thirdButton;
        
        
        //EVENT WINDOW SPRITES
        [SerializeField] private Sprite  homelessCatSprite;
        [SerializeField] private Sprite  diceCatSprite;
        [SerializeField] private Sprite  stumbleSprite;
        [SerializeField] private Sprite  perfectTomatoesSprite;
        [SerializeField] private Sprite  caveSprite;
        [SerializeField] private Sprite  stuckMerchantSprite;
        [SerializeField] private Sprite  thievesSprite;
        [SerializeField] private Sprite  gatherSprite;

        #endregion
   
    
    
    private void Awake()
    {
        islandManager = FindObjectOfType<IslandManager>();
    }


    public void Init(int timeCost)
    {
        //Debug.Log("TimeCost initialized to " + timeCost);
        this.timeCost = timeCost;
    }
    //LEN PRI POKRACOVANI S 3. BUTTONOM
    public void Continue()
    {
        islandManager.LowerTime(-1);
    }
    public void CloseEvent()
    {
        //RemoveAllListeners();
        Time.timeScale = 1;
        islandManager.LowerTime(timeCost);
        HideWindow();
        //Debug.Log("CLOSE");
    }
    //ZMAZE VSETKY AKCIE NA TLACIDLACH OKREM ZATVORENIE OKNA
    //(LEBO TO JE NASTAVENE V INSPEKTOROVI LEN NA TRETIE TLACIDLO)
    public void RemoveAllListeners()
    {
        firstButton.GetComponent<Button>().onClick.RemoveAllListeners();
        secondButton.GetComponent<Button>().onClick.RemoveAllListeners();
        thirdButton.GetComponent<Button>().onClick.RemoveAllListeners();
    }

    #region EventWindowSetters
    private void SetWindowName(string text)
    {
        if (!string.IsNullOrEmpty(text))
        {
            windowName.GetComponent<TMP_Text>().text = text;
        }
    }
    private void SetWindowText(string text)
    {
        windowText.GetComponent<TMP_Text>().text = text;
    }
    private void SetFirstButtonText(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            firstButton.SetActive(false);
        }
        else
        {
            firstButton.SetActive(true);
            firstButton.GetComponentInChildren<TMP_Text>().text = text;
        }
    }
    private void SetSecondButtonText(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            secondButton.SetActive(false);
        }
        else
        {
            secondButton.SetActive(true);
            secondButton.GetComponentInChildren<TMP_Text>().text = text;
        }
    }
    
    private void SetThirdButtonText(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            thirdButton.SetActive(false);
        }
        else
        {
            thirdButton.SetActive(true);
            thirdButton.GetComponentInChildren<TMP_Text>().text = text;
        }
    }
    //CAKA SA NA SPRITES
    private void AssignEventWindowSprite(String eventName)
    {
        if (string.IsNullOrEmpty(eventName)) return;
        Image imageComponent = windowImage.GetComponent<Image>();
        switch (eventName)
        {
            case "Homeless cat":
                imageComponent.sprite =  homelessCatSprite;
                break;
            case "Dice cat":
                imageComponent.sprite =  diceCatSprite;
                break;
            case "Stumble":
                imageComponent.sprite =  stumbleSprite;
                break;
            case "Perfect tomatoes":
                imageComponent.sprite =  perfectTomatoesSprite;
                break;
            case "Cave":
                imageComponent.sprite =  caveSprite;
                break;
            case "Stuck merchant":
                imageComponent.sprite =  stuckMerchantSprite;
                break;
            case "Thieves":
                imageComponent.sprite =  thievesSprite;
                break;
            //TO ISTE AKO THIEVES EVENT LEN INY OBRAZOK?
            case "Robbers":
                imageComponent.sprite =  thievesSprite;
                break;
            case "Gather":
                imageComponent.sprite =  gatherSprite;
                break;
            
            default:
                Debug.Log("What are you doing here CRIMINAL SCUM?");
                break;
        }
    }

    
    #endregion

    public void SetUpEventWindow(string name,string text,string firstBtn = "",string secondBtn = "",string thirdBtn = "LEAVE")
    {
        //EventWindowControl eventWindowControl = eventWindow.GetComponent<EventWindowControl>();
        SetWindowName(name);
        SetWindowText(text);
        SetFirstButtonText(firstBtn);
        SetSecondButtonText(secondBtn);
        SetThirdButtonText(thirdBtn);
        AssignEventWindowSprite(name);
        RemoveAllListeners();
        ShowWindow();
    }
    
    public void ShowWindow()
    {
        gameObject.SetActive(true);
    }
    public void HideWindow()
    {
        gameObject.SetActive(false);
    }


    


}
