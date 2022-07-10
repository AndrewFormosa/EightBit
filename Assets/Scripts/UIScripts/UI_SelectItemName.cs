using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SelectItemName : MonoBehaviour
{
    public Text textFieldA;
    public Text textFieldB;
    public MultiItem_UI_Script storeScript;
    // Start is called before the first frame update
    void Start()
    {
        storeScript.UpdateAllSlotsGraphics += UpDateText;
    }
    public void UpDateText()
    {
        string nameText = "";
        string descriptionText = "";

        if (storeScript.SingleClickedSlot != 0)
        {
            string nameID = storeScript.itemsNames[storeScript.SingleClickedSlot - 1];
            if (nameID != "")
            {
                nameText = nameID;
                descriptionText = ItemLoader.IndexedPrefab(nameID).GetComponent<ObjVar>().Description;
            }
        }
        textFieldA.text = nameText;
        textFieldB.text = descriptionText;

    }
}
