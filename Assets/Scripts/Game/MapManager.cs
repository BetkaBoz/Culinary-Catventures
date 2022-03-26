using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class MapManager : MonoBehaviour
{
    [SerializeField] private int MaxSectors = 5;

    [SerializeField] private float wrapperWidth = Screen.width;
    [SerializeField] [Range(0, 1)] private float padding = 0.2F;
    
    [SerializeField] private GameObject islandPrefab;
    
    private float sectorDistance;
    [SerializeField] private GameObject GameMap;

    public MapManager()
    {
        this.sectorDistance = Screen.height / (this.MaxSectors + 1);
    }
    
    
    public void setMaxSectors(int MaxSectors)
    {
        this.MaxSectors = MaxSectors;
    }
    
    
    
    public void SetUpIslands()
    {
        // Island dp;
        GameObject StartIsland = Instantiate(islandPrefab);
        StartIsland.GetComponent<Island>().setStartType();
        StartIsland.name = "StartIsland";
        // dp = StartIsland.AddComponent<Island>();
        
        // dp.setStartType();
        
        
        for (int sectorNum = 0; sectorNum < MaxSectors-2; sectorNum++)
        {
            int islandCnt = Random.Range(1, 4);
            for (int islandNum = 0; islandNum < islandCnt; islandNum++)
            {
                // GameObject Island = new GameObject("Island");
                // dp = Island.AddComponent<Island>();
                // dp.setRandomIslandType();
                
                GameObject Island = Instantiate(islandPrefab);
                Island.GetComponent<Island>().setRandomIslandType();
                Island.name = Island.GetComponent<Island>().getIslandType() + " Island" + sectorNum + "-" + islandNum ;
                Island.transform.parent = GameMap.transform;
                
                float xPosRangeStart = this.wrapperWidth / islandCnt * islandNum;
                float xPosRangeEnd = this.wrapperWidth / islandCnt * (islandNum + 1);

                float distance = xPosRangeEnd - xPosRangeStart;
                distance = this.padding / 2 * distance;
                xPosRangeStart = xPosRangeStart + distance;
                xPosRangeEnd = xPosRangeEnd - distance;

                float xPos = Random.Range(xPosRangeStart, xPosRangeEnd);
                float yPos = (sectorNum + 1) * this.sectorDistance;
                
                Island.transform.SetPositionAndRotation(new Vector3(xPos, yPos), Quaternion.identity);
            }
        }
        // GameObject FinishIsland = new GameObject("FinishIsland");
        // dp = FinishIsland.AddComponent<Island>();
        //
        // dp.setFinishType();
        
        GameObject FinishIsland = Instantiate(islandPrefab);
        FinishIsland.GetComponent<Island>().setFinishType();
        FinishIsland.name = "FinishIsland";
    }
    
    // Start is called before the first frame update
    void Start()
    {
    

        SetUpIslands();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
