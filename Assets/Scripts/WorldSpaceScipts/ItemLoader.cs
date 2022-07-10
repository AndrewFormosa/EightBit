using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLoader : MonoBehaviour
{
    public GameObject[] inspectorLoadableObjects;
    public static GameObject[] LoadableObjects;

    public static void LoadAllItems(List<SaveableItemData> itemsData, GameObject parent)
    {
      
        int noOfObjects = itemsData.Count;      
        foreach(SaveableItemData itemData in itemsData)
        {
            string reference = itemData.prefabID;
        
            GameObject loadedObject = Instantiate(IndexedPrefab(reference),parent.transform);
           
            loadedObject.GetComponent<LoadableScript>().ConfigureItem(itemData);
            
        }
      

    }

    private void Start()
    {
        LoadableObjects = inspectorLoadableObjects;

    }

 
    //Get attributes from prefabs.
    public static GameObject IndexedPrefab(string itemID)
    {
       
        foreach(GameObject obj in LoadableObjects)
        {
          
            if (obj.GetComponent<ObjVar>().Name == itemID)
            {
              
                return obj;
            }
        }
        return null;
    }

    public static Texture IndexedPrefabTexture(string itemID)
    {     
        foreach(GameObject obj in LoadableObjects)
        {          
            if (obj.GetComponent<ObjVar>().Name == itemID)
            {
                return obj.GetComponent<SpriteRenderer>().sprite.texture;
            }
        }
        return null;
    }

    public static int IndexedPrefabMaxVolume(string itemID)
    {
        foreach (GameObject obj in LoadableObjects)
        {
            if (obj.GetComponent<ObjVar>().Name == itemID)
            {
                return obj.GetComponent<Action_BeCollected>().MaxQtyInInventorySlot;
            }
        }
        return 9999;
    }
}
