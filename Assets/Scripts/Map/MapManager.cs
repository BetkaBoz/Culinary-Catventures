using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Random = UnityEngine.Random;

// TODO :
// -rn conf.MaxSectors governs both how many sectors there are & how many are visible
// -later on we should create maxVisibleSectors in config file and adjust MapManager to calculate conf.SectorDistance
//      from that OR make conf.SectorDistance adjustable from Editor, however, that will mean it will show differently
//      on different screen aspect ratios
public class MapManager : MonoBehaviour
{
    [SerializeField] MapConfig
        conf; // config file, contains most of thevariables this file would, but neatly tidied + allows us to swap config files for other levels

    [SerializeField] private GameObject gameMap;
    private List<List<Island>> islands = new List<List<Island>>();

    /// <summary>
    /// Instantiates new Island with given Sector and islandDum coordinates
    /// with automatic position calculation based on camera size and randomized horizontal position in calculated range
    /// </summary>
    private void CreateIsland(string name, int type, int sectorNum, int islandNum,
        int islandCnt) // type: 0-start, 1-finish, 2-random
    {
        GameObject Island = Instantiate(conf.IslandPrefab);
        Island.transform.SetParent(this.gameMap.transform); // puts Island as child of the Map game object
        switch (type) // type: 0-start, 1-finish, 2-random
        {
            case 0:
                Island.GetComponent<Island>().setStartType();
                break;
            case 1:
                Island.GetComponent<Island>().setFinishType();
                break;
            case 2:
                Island.GetComponent<Island>().setRandomIslandType();
                break;
        }

        Island.name = Island.GetComponent<Island>().getIslandType() + name + sectorNum + "-" + islandNum;

        // position calculation:
        float xPosRangeStart = conf.WrapperWidth / islandCnt * islandNum;
        float xPosRangeEnd = conf.WrapperWidth / islandCnt * (islandNum + 1);
        float distance = xPosRangeEnd - xPosRangeStart;
        distance = conf.Padding / 2 * distance;
        xPosRangeStart += distance;
        xPosRangeEnd -= distance;

        // setting position relative to the camera (Viewport has values from bottom left corner [0,0] to top right [1,1])
        // x position randomly placed in calculated range
        // y position inverted so we generate Islands from top to bottom and shifted for half a sectorDistance
        Vector2 islandPos = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(xPosRangeStart, xPosRangeEnd),
            1f - (sectorNum + 0.5f) * conf.SectorDistance));
        Island.transform.SetPositionAndRotation(islandPos, Quaternion.identity);

        // islands.Add(Island);
        islands[sectorNum].Add(Island.GetComponent<Island>());
    }

    /// <summary>
    /// Creates new Island when we need only one island in the Sector
    /// </summary>
    private void CreateIsland(string name, int type, int sectorNum)
    {
        CreateIsland(name, type, sectorNum, 0, 1);
    }

    /// <summary>
    /// Creates new Island on the grid of vertical Sectors and horizontal islandNums
    /// </summary>
    public void SetUpIslands()
    {
        PrepareIslandsList();
        for (int sectorNum = 0; sectorNum < conf.MaxSectors; sectorNum++)
        {
            if (sectorNum == 0)
            {
                CreateIsland("Island", 0, sectorNum);
                continue;
            }

            if (sectorNum == conf.MaxSectors - 1)
            {
                CreateIsland("Island", 1, sectorNum);
                continue;
            }

            int islandCnt = Random.Range(1, 4); // random number of Islands in the Sector 1-3
            for (int islandNum = 0; islandNum < islandCnt; islandNum++)
            {
                CreateIsland("Island", 2, sectorNum, islandNum, islandCnt);
            }
        }
    }

    public void SetUpEdges() // so edgy
    {
        for (int sectorNum = 0; sectorNum < conf.MaxSectors - 1; sectorNum++)
        {
            for (int islandOrder = 0; islandOrder < islands[sectorNum].Count; islandOrder++)
            {
                Island currentIsland = islands[sectorNum][islandOrder];
                for (int nextIslandIndex = 0; nextIslandIndex < islands[sectorNum + 1].Count; nextIslandIndex++)
                {
                    if (sectorNum == 0)
                    {
                        currentIsland.NextIslands.Add(islands[sectorNum + 1][nextIslandIndex]);
                        // islands[sectorNum + 1][nextIslandIndex].PreviousIslands.Add(startIsland);
                        CreateEdge(currentIsland, islands[sectorNum + 1][nextIslandIndex]);
                    }
                }

                // PODMIENKY:
                // prvy ostrov vedie do vsetkych nasledovnych
                // predposledne ostrovy pojdu vsetky do posledneho
                // hranicny ostrov vzdy ide do hranicneho ostrova
                // ak niesme v hranicnom ostrove, ideme do ostrova, ktory este nema parenta, ak maju vsetky parenta, vyberieme nahodne

                // if (sectorNum == conf.MaxSectors - 1)
                // {
                //     CreateEdge(sectorNum, conf.MaxSectors - 1);
                //     continue;
                // }
            }
        }
    }

    // TODO : spravit z GameObjectu Edge prefab, ktory bude mat LineRenderer a texturu ciary, animaciu atd.
    private void CreateEdge(Island currentIsland, Island nextIsland)
    {
        // vytvor Path medzi vsetkymi islandmi s naming scheme ako ostrovy
        GameObject Edge = new GameObject();
        Edge.transform.SetParent(currentIsland.transform); // puts Island as child of the Map game object
        Edge.name = "Edge" + nextIsland.name.Substring(nextIsland.name.Length - 3);
        Debug.Log("Creating edge " + Edge.name);
        // Island.name = Island.GetComponent<Island>().getIslandType() + name + sectorNum + "-" + islandNum;
        //
        // // position calculation:
        // float xPosRangeStart = conf.WrapperWidth / islandCnt * islandNum;
        // float xPosRangeEnd = conf.WrapperWidth / islandCnt * (islandNum + 1);
        // float distance = xPosRangeEnd - xPosRangeStart;
        // distance = conf.Padding / 2 * distance;
        // xPosRangeStart += distance;
        // xPosRangeEnd -= distance;
        //
        // // setting position relative to the camera (Viewport has values from bottom left corner [0,0] to top right [1,1])
        // // x position randomly placed in calculated range
        // // y position inverted so we generate Islands from top to bottom and shifted for half a sectorDistance
        // Vector2 islandPos = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(xPosRangeStart, xPosRangeEnd),
        //     1f - (sectorNum + 0.5f) * conf.SectorDistance));
        // Island.transform.SetPositionAndRotation(islandPos, Quaternion.identity);
        //
        // // islands.Add(Island);
        // islands[sectorNum].Add(Island);
    }

    private void PrepareIslandsList()
    {
        for (int i = 0; i < conf.MaxSectors; i++)
        {
            islands.Add(new List<Island>());
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        conf.SectorDistance = (float) 1 / conf.MaxSectors; // in Viewport, not World of ScreenSpace
        SetUpIslands();
        SetUpEdges();
    }

    private void Start()
    {
        // PrintIslands();
    }

    private void PrintIslands()
    {
        Debug.Log(islands);
        for (int i = 0; i < islands.Count; i++)
        {
            Debug.Log("Sektor: " + i);
            for (int j = 0; j < islands[i].Count; j++)
            {
                Debug.Log(islands[i][j].name);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}