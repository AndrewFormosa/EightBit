using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveableScipt : MonoBehaviour
{
 
    public SaveableItemData GetSavableData()
    {
        SaveableItemData itemData = new SaveableItemData();
        ObjVar thisItemData = gameObject.GetComponent<ObjVar>();
       
        itemData.prefabID = thisItemData.Name;
        itemData.posX = gameObject.transform.position.x;
        itemData.posY = gameObject.transform.position.y;
        itemData.intDataCollection = thisItemData.intData;
        itemData.stringDataCollection = thisItemData.stringData;
        itemData.category = thisItemData.catrgory;
        return itemData;


    }

}
