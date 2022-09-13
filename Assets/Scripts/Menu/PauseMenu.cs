using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuSection;
    [SerializeField] private GameObject optionsMenuSection;
    [SerializeField] private Volume volume;
    [SerializeField] private Slider gammaSlider;

    [SerializeField] private LiftGammaGain liftGammaGain;
    
    private void Awake() {
        //if (!volume.profile.TryGet(out liftGammaGain)) throw new System.NullReferenceException(nameof(liftGammaGain));
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        volume = FindObjectOfType<Volume>();
        if (!volume)
        {
            enabled = false;
        }
        else
        {
            volume.profile.TryGet(out liftGammaGain);
            
        }
        
         
    }

    // Update is called once per frame
    void Update()
    {
        

        //var gammaValue = liftGammaGain.gamma.value;
        //gammaValue.w = gammaSlider.value;
    }

    public void StartPauseMenu()
    {
        StartCoroutine(LoadPauseMenu());
        Time.timeScale = 0;
        EventManager.IsInEvent = true;
    }
    public void StopPauseMenu()
    {
        StartCoroutine(UnloadPauseMenu());
        Time.timeScale = 1;
        EventManager.IsInEvent = false;

    }
    public void StartMainMenu()
    {
        StartCoroutine(LoadMainMenu());
        Time.timeScale = 1;
        EventManager.IsInEvent = false;
    }
    public void OpenOptionsMenu()
    {
        pauseMenuSection.SetActive(false);
        optionsMenuSection.SetActive(true);
        GetOptions();
    }
    private void GetOptions()
    {
        var gammaValue = liftGammaGain.gamma.value.w;
        Debug.Log("GAMMA : "+gammaValue);
        gammaSlider.value = gammaValue;
        
        gammaSlider.onValueChanged.AddListener (delegate {
            
            liftGammaGain.gamma.Override(new Vector4(1f, 1f, 1f, gammaSlider.value));
            //Debug.Log("SLIDER : "+gammaSlider.value);

            
        });   
    }
    public void Back()
    {
        pauseMenuSection.SetActive(true);
        optionsMenuSection.SetActive(false);
    }
    public void SetToDefaultValues()
    {
        liftGammaGain.gamma.Override(new Vector4(1f, 1f, 1f, 1f));

    }
    
    IEnumerator LoadPauseMenu()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Pause Menu", LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        Debug.Log("Oh nou I'm paused :(");
    }

    IEnumerator UnloadPauseMenu()
    {
        Debug.Log("Oh nou I'm unpaused :(");

        AsyncOperation asyncLoad = SceneManager.UnloadSceneAsync("Pause Menu");
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

    }
    
    IEnumerator LoadMainMenu()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Menu");
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        Debug.Log("Oh nou I'm in menu :(");
    }
    
}
