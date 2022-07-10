using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class DataManager : MonoBehaviour
{
   // public ItemLoader itemLoader;
    public string gameSaveName;
    public string regionPreFix;
    

    public void SaveItemsToRegionFile(List<SaveableItemData> itemData, string regionKey)
    {
        File.Delete(Application.persistentDataPath + "/" + regionKey + ".dat");
        FileStream file = new FileStream(Application.persistentDataPath + "/"+regionKey+".dat", FileMode.Create);      
        //Binery Formatter -- allows us to write data to file
        BinaryFormatter formatter = new BinaryFormatter();
        //serialization file to write to the file.
        // formatter.Serialize(file, gameManagerScp.myStats);
        formatter.Serialize(file, itemData);
        file.Close();
    }

    public List<SaveableItemData> LoadItemsFromRegionFile(string regionKey)
    {
        FileStream file = new FileStream(Application.persistentDataPath  +"/"+ regionKey + ".dat", FileMode.OpenOrCreate);
        if (file.Length != 0)
        {

            BinaryFormatter formatter = new BinaryFormatter();
            List<SaveableItemData> itemData = (List<SaveableItemData>)formatter.Deserialize(file);
            file.Close();
            return itemData;
        }
        else
        {
            file.Close();
            return null;
        }      
    }

  

    public List<string> SavedRegionsList()
    {
        List<string> regionNames = new List<string>();
        string[] availableRegions = (Directory.GetFiles(Application.persistentDataPath,"*.dat"));
        foreach (string name in availableRegions)
        {
            string regionName = Path.GetFileName(name);
            regionNames.Add(regionName);
        }
        return regionNames;
    }

    public static void SaveInventory(SaveableItemData inventoryData)
    {
        File.Delete(Application.persistentDataPath + "/inventory.dat");
        FileStream file = new FileStream(Application.persistentDataPath + "/inventory.dat", FileMode.Create);
        //Binery Formatter -- allows us to write data to file
        BinaryFormatter formatter = new BinaryFormatter();
        //serialization file to write to the file.
        // formatter.Serialize(file, gameManagerScp.myStats);
        formatter.Serialize(file, inventoryData);
        file.Close();
    }

    public static SaveableItemData LoadInventory()
    {
        FileStream file = new FileStream(Application.persistentDataPath + "/inventory.dat", FileMode.OpenOrCreate);
        if (file.Length!=0)
            
        {
            BinaryFormatter formatter = new BinaryFormatter();
        
            SaveableItemData inventoryData = (SaveableItemData)formatter.Deserialize(file);
            file.Close();
            return inventoryData;
        }else
        {
            file.Close();
            return null;
        }
    }

    public static void SavePlayerStatus(SaveableItemData playerStatus)
    {
        File.Delete(Application.persistentDataPath + "/playerStatus.dat");
        FileStream file = new FileStream(Application.persistentDataPath + "/playerStatus.dat", FileMode.Create);
        //Binery Formatter -- allows us to write data to file
        BinaryFormatter formatter = new BinaryFormatter();
        //serialization file to write to the file.
        // formatter.Serialize(file, gameManagerScp.myStats);
        formatter.Serialize(file, playerStatus);
        file.Close();
    }

    public static SaveableItemData LoadPlayerStatus()
    {
        FileStream file = new FileStream(Application.persistentDataPath + "/playerStatus.dat", FileMode.OpenOrCreate);
        if (file.Length!= 0)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            SaveableItemData playerStatus = (SaveableItemData)formatter.Deserialize(file);
            file.Close();
            return playerStatus;
        }
        else { file.Close();return null; }
    }




}



[System.Serializable]
public class SaveableItemData
{
   
    public string prefabID;
    public ObjectCatagories.Categories category;
    public float posX;
    public float posY;
    public float posZ;
    public string[] stringDataCollection;
    public int[] intDataCollection;
    public bool[] boolDataCollection;
}
