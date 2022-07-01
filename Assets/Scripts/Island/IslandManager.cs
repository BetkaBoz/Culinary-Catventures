using TMPro;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IslandManager : MonoBehaviour
{
    [SerializeField] public int time;
    [SerializeField] private TextMeshProUGUI  timeText;
    [SerializeField] private GameObject grabberPrefab;
    [SerializeField] private GameObject easterEggGrabberPrefab;

    //[SerializeField] private TextMeshProUGUI  coinText;
    //[SerializeField] private TextMeshProUGUI  repText; IN UILayer
    [SerializeField] private Light2D sun;
    [SerializeField] private GameObject playerLight;
    
    [SerializeField] private GameObject lights;
    //EVENT MANAGER
    private EventManager eventManager;
    
     //public int Time => time;



    private void Awake()
    {
        timeText.text = "Time: " + time;
        sun  = GameObject.FindGameObjectWithTag("Light").GetComponent<Light2D>();
        //playerLight  = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).gameObject;
        //playerLight = FindObjectOfType<Player>().GetComponentInChildren<Light2D>().gameObject;
        eventManager = FindObjectOfType<EventManager>();

        LightControl();
    }


    private void LockAllEvents()
    {
        
        //List<GameObject>;
        //GameObject[] tmpEvents = GameObject.FindGameObjectsWithTag("Event");
        //List<EventManager> Events = new List<EventManager>();
        Debug.Log("EVENTS ARE LOCKED!");
        foreach (Event @event in eventManager.allEvents)
        {
            if (!@event.isChallenge)
            {
                @event.isUsed = true;
                @event.GetComponentInChildren<Image>().color = new Color32(125,125,125,255);
            }
        }
    }
    
    
    //ZNIŽOVANIE ČASU O LOWERBY
    public void LowerTime(int lowerBy)
    {
        time -= lowerBy;

        LightControl();
        //AK VYPRŠÍ TAK SPAWNI RUKU
        if (time <= 0)
        {
            time = 0;
            LockAllEvents();
            Invoke(nameof(StartGrabber),2f);
           
            
        }
        timeText.text = "Time: " + time;
    }
    
    private void StartGrabber()
    {
        //GameObject grabber = Instantiate(grabberPrefab, transform.position, Quaternion.identity) as GameObject;
        //grabber.transform.position = new Vector3(grabber.transform.position.x, grabber.transform.position.y, 20);
        //Debug.Log(grabber.transform.position.z);
        Instantiate(grabberPrefab);
        Invoke(nameof(GrabberEasterEgg),2f);
    }

    private void GrabberEasterEgg()
    {
        //TODO EASTER EGG
        Debug.Log(nameof(GrabberEasterEgg)+"!");
        Instantiate(easterEggGrabberPrefab);
        Invoke(nameof(StartBattle),2f);

        
    }

    //MENENIE SVETIEL PODLA CASU NA MAPE
    private void LightControl()
    {
        sun.intensity = 0.2f + time * 0.2f ;

        if (sun.intensity < 0.6)
        {
            lights.SetActive(true);
            //playerLight.SetActive(true);
            
        }
        else
        {            
            lights.SetActive(false);
            //playerLight.SetActive(false);

        }
    }
    
    //SPUSTI SCÉNU BOJA
    public void StartBattle()
    {
        //Uloženie hodnôt do PLAYERPREFS
        //NOT GOOD
        //{PlayerPrefs.SetInt("reputation", 100);}
        
        SceneManager.LoadScene("Battle", LoadSceneMode.Single);
    }
}