using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class OptionsMenu : MonoBehaviour
{

    public TextMeshProUGUI fullscreenText;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Screen.fullScreen)
        {
            fullscreenText.text = "Fullscreen: Yes";

        }
        else
        {
            fullscreenText.text = "Fullscreen: No";

        }
    }


    public void ChangeScreenMode()
    {
        if (Screen.fullScreen)
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
    }




}
