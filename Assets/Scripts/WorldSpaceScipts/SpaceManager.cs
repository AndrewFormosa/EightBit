using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class SpaceManager : MonoBehaviour
{
    public GameObject player;
    public GameObject regionPrefab;
    public DataManager dataManager;
    //public ItemLoader itemLoader;
    public int regionWidth;
    public int regionHeight;
    public static int regionW;// accessable
    public static int regionH;// accessable

    public static Dictionary<string, GameObject> LoadedRegions = new Dictionary<string, GameObject>(); // accessable

    public  List<String> regionsAvailbale;

    public Vector3[] activeRegionVectors = new Vector3[9];
    public Vector3[] requiredRegionVectors = new Vector3[9];
    public static Vector3 currentRegion; //accessable
    private float currentBoundryTop;
    private float currentBoundryBottom;
    private float currentBoundryLeft;
    private float currentBoundryRight;

    public static Action<Vector3> NewRegionCreated;


    public static string RegionToKey(Vector3 region)
    {

        return (region.x + "A" + region.y);
    }

    // Start is called before the first frame update
    void Start()
    {
        
        regionsAvailbale = dataManager.SavedRegionsList();
        regionW = regionWidth;
        regionH = regionHeight;

        player = GameObject.FindGameObjectWithTag("Player");

       CreateNewRegion(new Vector3(0, 0, 0)); //need to set up as configure world will not pick up this as a required region.
        SetRegionAndBoundries(new Vector3(0, 0, 0));
        ConfigureVisibleWorld();

    }


    // Update is called once per frame
    void Update()
    {
        //check player position and reconfigure visible world when player move beyond set boundries.
        Vector3 playerPosition = player.transform.position;
        float px = playerPosition.x;
        float py = playerPosition.y;
        if (px > currentBoundryRight || px < currentBoundryLeft || py > currentBoundryTop || py < currentBoundryBottom)
        {
           
         
            SetRegionAndBoundries(playerPosition);
            ConfigureVisibleWorld();
        
        }
    }

    public void SetRegionAndBoundries(Vector3 playerPosition)
    {
        currentRegion = RegionVector(playerPosition);
        currentBoundryTop = (RegionCenter(currentRegion).y) + (regionHeight / 2f);
        currentBoundryBottom = (RegionCenter(currentRegion).y) - (regionHeight / 2f);
        currentBoundryLeft = (RegionCenter(currentRegion).x) - (regionWidth / 2f);
        currentBoundryRight = (RegionCenter(currentRegion).x) + (regionWidth / 2f);
    }

    public void ConfigureVisibleWorld()
    {

        //evaluate the required surrounding regions for the new player position
        requiredRegionVectors = SurroundingRegionVectors(currentRegion);

        //deactivate any regions which are currently active but not included within the required regions.
        List<Vector3> requiredRegionsList = requiredRegionVectors.ToList<Vector3>();
        for (int i = 0; i < 9; i++)//look through all of the active regions
        {
            if (!requiredRegionsList.Contains(activeRegionVectors[i])) //if any active regions are not on the REQUIRED list then destroy the region and remove from the LoadedRegions list.
            {            
                Destroy(LoadedRegions[RegionToKey(activeRegionVectors[i])].gameObject);
                LoadedRegions.Remove(RegionToKey(activeRegionVectors[i]));         
            }
        }
        //look through all of the requires regions.
        List<Vector3> currentActiveList = activeRegionVectors.ToList<Vector3>();
        for (int i = 0; i < 9; i++)
        {
            if (!currentActiveList.Contains(requiredRegionVectors[i]))//if any of the required regions are not currently active then create the region.
            {
                CreateNewRegion(requiredRegionVectors[i]); 
            }
        }
        //update the array of active regions.
        activeRegionVectors = requiredRegionVectors;

    }

    public void CreateNewRegion(Vector3 regionVector)
    {
        GameObject newRegion = Instantiate(regionPrefab, RegionCenter(regionVector), Quaternion.identity); //create the region.

        RegionScript regionScript = newRegion.GetComponent<RegionScript>();
        regionScript.regionVector = regionVector;
        regionScript.regionHeight = regionHeight;
        regionScript.regionWidth = regionWidth;
        regionScript.regionKey = RegionToKey(regionVector);
        LoadedRegions.Add(RegionToKey(regionVector), newRegion);
        if (regionsAvailbale.Contains(RegionToKey(regionVector) + ".dat"))//if the region is available within the hard drive data file then load off the objects into the region.
        {
            List<SaveableItemData> itemsData = dataManager.LoadItemsFromRegionFile(RegionToKey(regionVector));
            ItemLoader.LoadAllItems(itemsData, newRegion.gameObject);
        }
        else
        {
            regionScript.Create();//otherwise create new objects to fill the region and add this region to the list of those availale on the hard drive.
            regionsAvailbale.Add(RegionToKey(regionVector) + ".dat");
            if (NewRegionCreated != null)
            {
                NewRegionCreated(regionVector);
            }

        }   
    }

    public static Vector3 RegionVector(Vector3 position)
    {
        int x = (int)((position.x + ((regionW / 2f) * Mathf.Sign(position.x))) / regionW);
        int y = (int)((position.y + ((regionH / 2f) * Mathf.Sign(position.y))) / regionH);
        return new Vector3(x, y, 0);
    }

    public static Vector3 RegionCenter(Vector3 region)
    {
        float x = region.x * regionW;
        float y = region.y * regionH;
        return new Vector3(x, y, 0);
    }

    public static Vector3[] SurroundingRegionVectors(Vector3 region)
    {
        int[] surroundXAddition = { -1, 0, 1, -1, 0, 1, -1, 0, 1 };
        int[] surroundYAddition = { -1, -1, -1, 0, 0, 0, 1, 1, 1 };
        Vector3[] surroundingRegions = new Vector3[9];
        for (int i = 0; i < 9; i++)
        {
            surroundingRegions[i] = new Vector3(region.x + surroundXAddition[i], region.y + surroundYAddition[i], 0);
        }
        return surroundingRegions;
    }


    public static bool insideBoundry(Vector3 regionCenter, Vector3 objectPosition)
    {
        float pX = objectPosition.x;
        float pY = objectPosition.y;
        float tb = regionCenter.y + (regionH / 2f);
        float bb = regionCenter.y - (regionH / 2f);
        float rb = regionCenter.x + (regionW / 2f);
        float lb = regionCenter.x - (regionW / 2f);
        if (pX > lb && pX < rb && pY < tb && pY > bb) { return true; } else { return false; }
    }
}
