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
        
        [SerializeField] private IslandManager islandManager;
        private int timeCost = 1;

        public GameObject FirstButton => firstButton;
        public GameObject SecondButton => secondButton;
        public GameObject ThirdButton => thirdButton;
        #endregion
   
    
    /*
    private void Awake()
    {
        //Zbytočné, je to v prefabe
        //Prehľadá všetky child elementy a priradí ich do premenných
        
        foreach (Transform child in transform)
        {
            switch (child.name)
            {
                case "EventName":
                    WindowName = child.gameObject;
                    break;
                case "EventText":
                    WindowText = child.gameObject;
                    break;
                case "EventImage":
                    WindowImage = child.gameObject;
                    break;
                case "FirstButton":
                    FirstButton = child.gameObject;
                    break;
                case "SecondButton":
                    SecondButton = child.gameObject;
                    break;
                case "ThirdButton":
                    ThirdButton = child.gameObject;
                    break;
                default:
                    Debug.Log("What are you doing here?");
                    
                    break;
            }
            //Debug.Log(child.name);

        }
      
    }
 */
    void Start()
    {
        
        
        //fstBtn = transform.Find("FirstButton").GetComponent<Button>();
        //scdBtn = transform.Find("SecondButton").GetComponent<Button>();
        //trdBtn = transform.Find("ThirdButton").GetComponent<Button>();
        
        islandManager = FindObjectOfType<IslandManager>();
        // Takto vyzera pridavanie funkcii buttonom zo scriptu
        //  bude sa hodit, ak sa budu funkcie na buttonoch menit za behu hry, napriklad v reakcii na hracov input
        //fstBtn.onClick.AddListener(delegate { CloseEvent(); });
        //scdBtn.onClick.AddListener(delegate { CloseEvent(); });
        //trdBtn.onClick.AddListener(delegate { CloseEvent(); });

        
        //firstButton.GetComponent<Button>().onClick.AddListener(delegate { CloseEvent(); });
        //secondButton.GetComponent<Button>().onClick.AddListener(delegate { CloseEvent(); });
        //thirdButton.GetComponent<Button>().onClick.AddListener(delegate { CloseEvent(); });

         
    }
    
    
    public void Init(int timeCost)
    {
        //Debug.Log("TimeCost initialized to " + timeCost);
        this.timeCost = timeCost;
    }
    
    
    public void CloseEvent()
    {
        RemoveAllListeners();
        Time.timeScale = 1;
        islandManager.LowerTime(timeCost);
        HideWindow();
        Debug.Log("CLOSE");
        //GameObject.FindGameObjectsWithTag("EventWindow")[0].SetActive(false);
    }
    //ZMAZE VSETKY AKCIE NA TLACIDLACH OKREM ZATVORENIE OKNA
    //(LEBO TO JE NASTAVENE V INSPEKTOROVI)
    public void RemoveAllListeners()
    {
        firstButton.GetComponent<Button>().onClick.RemoveAllListeners();
        secondButton.GetComponent<Button>().onClick.RemoveAllListeners();
        thirdButton.GetComponent<Button>().onClick.RemoveAllListeners();
    }

    public void SetWindowName(string text)
    {
        if (!string.IsNullOrEmpty(text))
        {
            windowName.GetComponent<TMP_Text>().text = text;
        }
    }
    public void SetWindowText(string text)
    {
        windowText.GetComponent<TMP_Text>().text = text;
    }
    public void SetFirstButtonText(string text)
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
    public void SetSecondButtonText(string text)
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
    
    public void SetThirdButtonText(string text)
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
    
    public void SetUpEventWindow(string name,string text,string firstBtn = "",string secondtBtn = "",string thirdtBtn = "")
    {
        //EventWindowControl eventWindowControl = eventWindow.GetComponent<EventWindowControl>();
        SetWindowName(name);
        SetWindowText(text);
        SetFirstButtonText(firstBtn);
        SetSecondButtonText(secondtBtn);
        SetThirdButtonText(thirdtBtn);
    }
    
    public void ShowWindow()
    {
        gameObject.SetActive(true);
        //transform.position += Vector3.forward * 2000;
    }
    public void HideWindow()
    {
        gameObject.SetActive(false);
        //transform.position -= Vector3.forward * 2000;
    }

    public void Continue()
    {
        islandManager.LowerTime(-timeCost);

        ShowWindow();
        //ZVISI CAS,- a - je + 
        //Time.timeScale = 0;

        //transform.position += Vector3.forward * 2000;
    }


}
