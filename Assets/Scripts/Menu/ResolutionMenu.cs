using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private int width;
    private int height;

    public void SetWidth(int width)
    {
        this.width = width;
    }
    public void SetHeight(int height)
    {
        this.height = height;
    }
    

    public void SetResolution()
    {
        Screen.SetResolution(this.width, this.height, CheckIfFullscreen());
    }
    
    public bool CheckIfFullscreen()
    {
        if (Screen.fullScreen)
        {
            return true;
        }
        else
        {
            return false;

        }
    }
}
