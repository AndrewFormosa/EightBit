using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerminentRegionScript : MonoBehaviour
{
    public string PerminentRegionName;
    public DataManager dataManager;
    // Start is called before the first frame update
    void Start()
    {
        dataManager = FindObjectOfType<DataManager>();
        List<SaveableItemData> itemsData = dataManager.LoadItemsFromRegionFile(PerminentRegionName);
       // Debug.Log("Loaded" +itemsData[0].prefabID);
        if (itemsData!=null)
        {
            ItemLoader.LoadAllItems(itemsData, this.gameObject);
        }
    }

    public void OnDestroy()
    {
        Debug.Log("perminentDestroyed");
        List<SaveableItemData> regionalDataToSave = new List<SaveableItemData>();
        SaveableScipt[] savableObjects = gameObject.GetComponentsInChildren<SaveableScipt>();
        foreach (SaveableScipt savableObject in savableObjects)
        {

            regionalDataToSave.Add(savableObject.GetSavableData());
           //Debug.Log("Savable:" + savableObject);
            Destroy(savableObject.gameObject);
        }
        dataManager.SaveItemsToRegionFile(regionalDataToSave,PerminentRegionName);
       // Debug.Log("Saved:" + regionalDataToSave[0].prefabID);
    }

  
}
