using UnityEngine;
using Random = UnityEngine.Random;

public class MapManager : MonoBehaviour
{
    [SerializeField] MapConfig conf;
    [SerializeField] private GameObject gameMap;

    public MapManager()
    {
    }

    // public void setMaxSectors(int MaxSectors)
    // {
    //     this.maxSectors = MaxSectors;
    // }
    public void SetUpIslands()
    {
        // GameObject Island = Instantiate(islandPrefab);
        // Island.GetComponent<Island>().setRandomIslandType();
        // Island.name = Island.GetComponent<Island>().getIslandType() + " Island Top Left";
        // Island.transform.SetParent(gameMap.transform);
        // Island.transform.SetPositionAndRotation(Camera.main.ViewportToWorldPoint(new Vector3(0,1)), Quaternion.identity);

        // GameObject startIsland = Instantiate(islandPrefab);
        // startIsland.GetComponent<Island>().setStartType();
        // startIsland.name = "StartIsland";

        for (int sectorNum = 0; sectorNum < conf.MaxSectors; sectorNum++)
        {
            if (sectorNum == 0)
            {
                GameObject Island = Instantiate(conf.IslandPrefab);
                Island.GetComponent<Island>().setStartType();
                Island.name = "StartIsland";
                Island.transform.SetParent(this.gameMap.transform);

                float xPosRangeStart = 0;
                float xPosRangeEnd = conf.WrapperWidth;
                float distance = xPosRangeEnd - xPosRangeStart;
                distance = conf.Padding / 2 * distance;
                xPosRangeStart = xPosRangeStart + distance;
                xPosRangeEnd = xPosRangeEnd - distance;

                Island.transform.SetPositionAndRotation(
                    Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(xPosRangeStart, xPosRangeEnd),
                        1f - (sectorNum + 0.5f) * conf.SectorDistance)), Quaternion.identity);

                continue;
            }
            if (sectorNum == conf.MaxSectors - 1)
            {
                GameObject Island = Instantiate(conf.IslandPrefab);
                Island.GetComponent<Island>().setFinishType();
                Island.name = "FinishIsland";
                Island.transform.SetParent(this.gameMap.transform);

                float xPosRangeStart = 0;
                float xPosRangeEnd = conf.WrapperWidth;
                float distance = xPosRangeEnd - xPosRangeStart;
                distance = conf.Padding / 2 * distance;
                xPosRangeStart = xPosRangeStart + distance;
                xPosRangeEnd = xPosRangeEnd - distance;

                Island.transform.SetPositionAndRotation(
                    Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(xPosRangeStart, xPosRangeEnd),
                        1f - (sectorNum + 0.5f) * conf.SectorDistance)), Quaternion.identity);

                continue;
            }

            int islandCnt = Random.Range(1, 4); // pocet ostrovov 1-3
            for (int islandNum = 0; islandNum < islandCnt; islandNum++)
            {
                GameObject Island = Instantiate(conf.IslandPrefab);
                Island.GetComponent<Island>().setRandomIslandType();
                Island.name = Island.GetComponent<Island>().getIslandType() + " Island" + sectorNum + "-" + islandNum;
                Island.transform.SetParent(this.gameMap.transform);

                float xPosRangeStart = conf.WrapperWidth / islandCnt * islandNum;
                float xPosRangeEnd = conf.WrapperWidth / islandCnt * (islandNum + 1);
                float distance = xPosRangeEnd - xPosRangeStart;
                distance = conf.Padding / 2 * distance;
                xPosRangeStart = xPosRangeStart + distance;
                xPosRangeEnd = xPosRangeEnd - distance;

                Island.transform.SetPositionAndRotation(
                    Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(xPosRangeStart, xPosRangeEnd),
                        1f - (sectorNum + 0.5f) * conf.SectorDistance)), Quaternion.identity);
                // Debug.Log(new Vector3(xPosRangeStart, ((sectorNum + 1) * this.sectorDistance)/(this.sectorDistance * (maxSectors-2))));
                //
                // float distance = xPosRangeEnd - xPosRangeStart;
                // distance = this.padding / 2 * distance;
                // xPosRangeStart = xPosRangeStart + distance;
                // xPosRangeEnd = xPosRangeEnd - distance;
                //
                // float xPos = Random.Range(xPosRangeStart, xPosRangeEnd);
                // float yPos = (sectorNum + 1) * this.sectorDistance; // Camera.main.WorldToViewportPoint()
                //
                // Vector3 positionVector = Camera.main.WorldToViewportPoint(new Vector3(xPos, yPos));
                //
                // Debug.Log("position Vector of " + Island.name + ": " + positionVector.x + "x" + positionVector.y);
                //
                // Island.transform.SetPositionAndRotation(positionVector, Quaternion.identity);
            }
        }

        // GameObject FinishIsland = Instantiate(islandPrefab);
        // FinishIsland.GetComponent<Island>().setFinishType();
        // FinishIsland.name = "FinishIsland";
    }

    // Awake is called before Start
    // private void Awake()
    // {
    //     Debug.Log(this.transform.localPosition.x);
    //     Debug.Log(this.transform.localPosition.y);
    //     GameObject Island = Instantiate(islandPrefab);
    //     Island.GetComponent<Island>().setRandomIslandType();
    //     Island.name = Island.GetComponent<Island>().getIslandType() + " Island Top Left";
    //     Island.transform.SetParent(gameMap.transform);
    //     Island.transform.SetPositionAndRotation(Camera.main.ViewportToWorldPoint(new Vector3(0,1)), Quaternion.identity);
    //
    //     // float xPos = -Camera.main.pixelWidth / 2;
    //     // float yPos = -Camera.main.pixelHeight/2;
    //     // Island.transform.SetPositionAndRotation(new Vector3(xPos, yPos), Quaternion.identity);
    //     
    //     Island = Instantiate(islandPrefab);
    //     Island.GetComponent<Island>().setRandomIslandType();
    //     Island.name = Island.GetComponent<Island>().getIslandType() + " Island Top Right";
    //     Island.transform.SetParent(gameMap.transform);
    //     Island.transform.SetPositionAndRotation(Camera.main.ViewportToWorldPoint(new Vector3(1,1)), Quaternion.identity);
    //
    //     Island = Instantiate(islandPrefab);
    //     Island.GetComponent<Island>().setRandomIslandType();
    //     Island.name = Island.GetComponent<Island>().getIslandType() + " Island Bottom Left";
    //     Island.transform.SetParent(gameMap.transform);
    //     Island.transform.SetPositionAndRotation(Camera.main.ViewportToWorldPoint(new Vector3(0,0)), Quaternion.identity);
    //     
    //     Island = Instantiate(islandPrefab);
    //     Island.GetComponent<Island>().setRandomIslandType();
    //     Island.name = Island.GetComponent<Island>().getIslandType() + " Island Bottom Right";
    //     Island.transform.SetParent(gameMap.transform);
    //     Island.transform.SetPositionAndRotation(Camera.main.ViewportToWorldPoint(new Vector3(1,0)), Quaternion.identity);
    // }

    // private void FindBoundries()
    // {
    //     float width = 1 / (Camera.main.WorldToViewportPoint(new Vector3(1, 1, 0)).x - .5f);
    //     float height = 1 / (Camera.main.WorldToViewportPoint(new Vector3(1, 1, 0)).y - .5f);
    //     
    //     //another answer
    //     Vector3 leftMidPoint = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth / 2, 0f));
    //     Vector3 rightMidPoint = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight / 2));
    //     Vector3 bottomMidPoint = Camera.main.ScreenToWorldPoint(new Vector3(0f, Camera.main.pixelHeight / 2));
    //     Vector3 topMidPoint = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight));
    //
    // }

    // Start is called before the first frame update
    void Start()
    {
        // RectTransform rt = (RectTransform) gameMap.transform;
        // Debug.Log(rt.rect.width);
        // Debug.Log(rt.rect.height);
        // Debug.Log(Screen.width);
        // Debug.Log(Screen.height);
        // Debug.Log(Camera.main.pixelWidth);
        // Debug.Log(Camera.main.pixelHeight);

        // Debug.Log(Camera.main.o);

        // this.sectorDistance = (float) Screen.height / (this.maxSectors + 1);
        // Debug.Log(this.sectorDistance);
        conf.SectorDistance = (float) 1 / conf.MaxSectors; // in Viewport, not World of ScreenSpace

        Debug.Log(conf.SectorDistance);
        SetUpIslands();
    }

    // Update is called once per frame
    void Update()
    {
    }
}