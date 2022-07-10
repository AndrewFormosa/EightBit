using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadableScript : MonoBehaviour
{
   
    public void ConfigureItem(SaveableItemData itemData)
    {
        if (itemData != null)
        {
            gameObject.transform.position = new Vector3(itemData.posX, itemData.posY);
            ObjVar objVar = gameObject.GetComponent<ObjVar>();
            objVar.intData = itemData.intDataCollection;
            objVar.stringData = itemData.stringDataCollection;
        }
       
    }
}
