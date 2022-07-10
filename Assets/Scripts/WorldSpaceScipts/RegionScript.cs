using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class RegionScript : MonoBehaviour
{
    public float circularMultiplier;
    public RegionTerrain defaultTerrain;
    public RegionTerrain[] regionTerrains;
    public string regionKey;
    public Vector3 regionVector;
    public int regionHeight;
    public int regionWidth;
    public List<Vector3> maze1Regions;
    public List<Vector3> maze2Regions;


    public GameObject[] objects;
    public int noOfObjects;
    public int terrain = 0;
    public RegionTerrain chosenTerrain;
   

    public DataManager dataManager;
  

    // Start is called before the first frame update
    void Start()
    {
        dataManager = FindObjectOfType<DataManager>();
      //  Random.InitState((int)(regionVector.x + (1000 * regionVector.y)));
    }
    
  
    public void Create()
    {
        if (regionVector != new Vector3(0, 0, 0))
        {
            if (maze1Regions.Contains(regionVector))
            {
            FindObjectOfType<Region_MazeCreate>().CreateRegion(regionWidth, regionHeight, this.gameObject);
            }else if (maze2Regions.Contains(regionVector))
            {
             FindObjectOfType<Region_Maze2Create>().CreateRegion(regionWidth, regionHeight, this.gameObject);
            }
            else
            {
            FillRegion();
            }      
        }
    }

    private void OnDestroy()
    {
        List<SaveableItemData> regionalDataToSave = new List<SaveableItemData>();
        SaveableScipt[] savableObjects = gameObject.transform.GetComponentsInChildren<SaveableScipt>();  
        foreach (SaveableScipt savableObject in savableObjects)
        {
           
            regionalDataToSave.Add(savableObject.GetSavableData());
            Destroy(savableObject.gameObject);
        }
        dataManager.SaveItemsToRegionFile(regionalDataToSave, regionKey);
    }



    public void FillRegion()
    
    {
        UnityEngine.Random.InitState((int)(regionVector.x + (1000 * regionVector.y)));
        //select random terrain.
        int rt = UnityEngine.Random.Range(1,1000);
        chosenTerrain = regionTerrains[0];
        int count = 0;
        for(int i = 0; i < regionTerrains.Length; i++)
        {
            int chance = regionTerrains[i].terainChanceIn1000;
            if (rt > (count+chance))
            {
                if ((i+1)<regionTerrains.Length)
                {
                    chosenTerrain = regionTerrains[i + 1]; 
                }
                else
                {
                    chosenTerrain = defaultTerrain;
                }
            }
            count += chance;
        }



        //Instantiate objects at positions within region
        for (int x = 0; x < regionWidth; x++)
        {
            for (int y = 0; y < regionHeight; y++)
            {
                float posX = x - (regionWidth / 2f);
                float posY = y - (regionHeight / 2f);

                GameObject nextItem = probableItem(x, y);
                if (nextItem != null)
                {
                    GameObject thing = Instantiate(nextItem, this.transform, false);
                    thing.transform.localPosition = new Vector3(posX, posY, 0);
                    
                }
                }
        }
    }

    //Select random item
    public GameObject probableItem(int x, int y)
    {
        GameObject obj;
        if (chosenTerrain.borderItemMain != null)
        {
            if (x == 0 || x == (regionWidth - 1) || y == 0 || y == (regionHeight - 1))
            {
                if (UnityEngine.Random.Range(0f, 1f) >chosenTerrain.borderItemChanceOfSecond)
                {
                    return chosenTerrain.borderItemMain;
                }
                else
                {
                    return chosenTerrain.borderItemSecond;
                }
            }
        }



        int rnd = UnityEngine.Random.Range(1, 1000);
        if (chosenTerrain.probableItems.Length > 0)
        {
            obj = chosenTerrain.defaultItem.item;


            if (!chosenTerrain.probableItems[0].circular || withinCircle(x,y,rnd)) // if pattern is circular
            {
                obj = chosenTerrain.probableItems[0].item;
            }
            
            int count = 0;
            for (int i = 0; i < chosenTerrain.probableItems.Length; i++)
            {
                int chance = chosenTerrain.probableItems[i].intemChanceIn1000;
                if (rnd > (count + chance))
                {                 
                    if ((i + 1) < chosenTerrain.probableItems.Length)
                    {
                        if (!chosenTerrain.probableItems[i + 1].circular || withinCircle(x,y,rnd))
                        {
                            obj = chosenTerrain.probableItems[i + 1].item;
                        }                 
                  }
                    else
                    {
                        obj = chosenTerrain.defaultItem.item;
                    }
                    
                }
                count += chance;
            }
        } else {obj = chosenTerrain.defaultItem.item; }
     
        return obj;
    }

    public bool withinCircle(int x, int y, int randNo)
    {
        if (randNo > (circularMultiplier*(Mathf.Pow((x - (regionWidth / 2)), 2) * (Mathf.Pow((y - (regionHeight / 2)), 2)))))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

[System.Serializable]
public class RegionTerrain
{
    public string terrainName;
    public int terainChanceIn1000;
    public TerrainItem defaultItem;
    public TerrainItem[] probableItems;
    public GameObject borderItemMain;
    public GameObject borderItemSecond;
    public float borderItemChanceOfSecond;
    
}

[System.Serializable]
public class TerrainItem
{
    public GameObject item;
    public int intemChanceIn1000;
    public bool circular;
  
}


