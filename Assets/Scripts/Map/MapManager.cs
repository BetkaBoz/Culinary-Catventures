using UnityEngine;
using Random = UnityEngine.Random;

// TODO :
// -rn conf.MaxSectors governs both how many sectors there are & how many are visible
// -later on we should create maxVisibleSectors in config file and adjust MapManager to calculate conf.SectorDistance
//      from that OR make conf.SectorDistance adjustable from Editor, however, that will mean it will show differently
//      on different screen aspect ratios
public class MapManager : MonoBehaviour
{
    [SerializeField] MapConfig conf; // config file, contains most of thevariables this file would, but neatly tidied + allows us to swap config files for other levels
    [SerializeField] private GameObject gameMap;
    
    /// <summary>
    /// Instantiates new Island with given Sector and islandDum coordinates
    /// with automatic position calculation based on camera size and randomized horizontal position in calculated range
    /// </summary>
    private void CreateIsland(string name, int type, int sectorNum, int islandNum, int islandCnt) // type: 0-start, 1-finish, 2-random
    {
        GameObject Island = Instantiate(conf.IslandPrefab);
        Island.transform.SetParent(this.gameMap.transform); // puts Island as child of the Map game object
        switch (type) // type: 0-start, 1-finish, 2-random
        {
            case 0: Island.GetComponent<Island>().setStartType(); break;
            case 1: Island.GetComponent<Island>().setFinishType(); break;
            case 2: Island.GetComponent<Island>().setRandomIslandType(); break;
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
        Island.transform.SetPositionAndRotation(
            Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(xPosRangeStart, xPosRangeEnd),
                1f - (sectorNum + 0.5f) * conf.SectorDistance)), Quaternion.identity);
    }

    /// <summary>
    /// Creates new Island when there is only one island in the Sector
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

    // Start is called before the first frame update
    void Start()
    {
        conf.SectorDistance = (float) 1 / conf.MaxSectors; // in Viewport, not World of ScreenSpace
        SetUpIslands();
    }

    // Update is called once per frame
    void Update()
    {
    }
}