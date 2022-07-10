using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_StoreOpened : MonoBehaviour
{
    private ActionMgr aM;
    private GameObject playerObj;
    public string actionName;
    public GameObject multiItem_UIPrefab;
   
    public int singleClickedSlot;
    public int doubleClickedSlot;
    public MultiItem_UI_Script singleClickedUI;
    public MultiItem_UI_Script doubleClickedUI;

    //action specific variables.....


    void Start()
    {
        actionName = "OperateOn";
        aM = this.GetComponent<ActionMgr>();
        playerObj = GameObject.FindGameObjectWithTag("Player");
        MakeCollectable(StoreIsEmpty());
    }

   

    public void DoActions()
    {

        if (aM.actionName == actionName)
        {
            InteractionScript.playerInputAllowed = false; //stop player input taken.
            Debug.Log("OPEN THE STORE");
            GameObject storeUI = Instantiate(multiItem_UIPrefab, gameObject.transform);
            MultiItem_UI_Script StoreUIScript = storeUI.GetComponent<MultiItem_UI_Script>();
            ObjVar storeData = gameObject.GetComponent<ObjVar>();
            StoreUIScript.displayPosX = 150;
            StoreUIScript.SetUpDisplay(storeData.Name, storeData.stringData, storeData.intData);
            StoreUIScript.SlotSingClicked += slotSingleClicked;
            StoreUIScript.SlotDoubClicked += slotDoubleClicked;
            MakeCollectable(StoreIsEmpty());




            ObjVar MystoreData = FindObjectOfType<MyStoreScript>().gameObject.GetComponent<ObjVar>();
            GameObject myStore = MystoreData.gameObject;

            GameObject MystoreUI = Instantiate(multiItem_UIPrefab, myStore.transform);
            MultiItem_UI_Script myStoreUIScript = MystoreUI.GetComponent<MultiItem_UI_Script>();
            myStoreUIScript.displayPosX = -150;
            myStoreUIScript.SetUpDisplay(MystoreData.Name, MystoreData.stringData, MystoreData.intData);
            myStoreUIScript.SlotSingClicked += slotSingleClicked;
            myStoreUIScript.SlotDoubClicked += slotDoubleClicked;

        }
    }

    public void slotSingleClicked(int slotNumber, MultiItem_UI_Script UI_Script)
    {
        if (doubleClickedSlot == 0)
        {
            Debug.Log("Single Clicked:" + slotNumber + UI_Script.mainTitle.text);
            SelectItem(slotNumber, UI_Script);
        }
        else
        if (doubleClickedSlot == slotNumber && UI_Script == doubleClickedUI)
        {
            doubleClickedSlot = 0;
            UI_Script.DoubleClickedSlot = 0;
            doubleClickedUI = null;
            SelectItem(slotNumber, UI_Script);
            UI_Script.CallUpdateAllSlotGraphics();
        }
        else { TryAndTransferItem(doubleClickedUI, doubleClickedSlot, UI_Script, slotNumber); }
    }

    public void SelectItem(int slotNumber, MultiItem_UI_Script UI_Script)
    {
        UI_Script.SingleClickedSlot = slotNumber;
        UI_Script.CallUpdateAllSlotGraphics();
    }

    public void slotDoubleClicked(int slotNumber, MultiItem_UI_Script UI_Script)
    {
        if (doubleClickedSlot == 0)
        {
            if (slotNumber == doubleClickedSlot && UI_Script == doubleClickedUI)
            {
                doubleClickedSlot = 0;
                UI_Script.DoubleClickedSlot = 0;
                doubleClickedUI = null;
                SelectItem(slotNumber, UI_Script);
                UI_Script.CallUpdateAllSlotGraphics();
            }
            else if(UI_Script.itemsValues[slotNumber-1]>0)
            {
                Debug.Log("Double Clicked:" + slotNumber + UI_Script.mainTitle.text);
                doubleClickedSlot = slotNumber;
                UI_Script.DoubleClickedSlot = slotNumber;
                doubleClickedUI = UI_Script;
                UI_Script.CallUpdateAllSlotGraphics();
            }
        }
    }

    public void MakeCollectable(bool collectable)
    {
        gameObject.GetComponent<Action_BeCollected>().enabled = collectable;
    }

    public bool StoreIsEmpty()
    {
        ObjVar storeObjVar = gameObject.GetComponent<ObjVar>();
        int[] storeIntVales = storeObjVar.intData;
       foreach(int value in storeIntVales)
        {
            if (value > 0) { return false; }
        }
        return true;
    }


    public void TryAndTransferItem(MultiItem_UI_Script fromMIUI, int fromSlot, MultiItem_UI_Script toMIUI, int toSlot)
    {

        ObjVar fromStoreObjVar = fromMIUI.gameObject.GetComponentInParent<ObjVar>();
        ObjVar toStoreObjVar = toMIUI.gameObject.GetComponentInParent<ObjVar>();
        Debug.Log("FromSlotNo:" + fromSlot);
        string transferedItemName = fromStoreObjVar.stringData[fromSlot - 1];
        string selectedSlotName = toStoreObjVar.stringData[toSlot - 1];
        int selectedSlotMaximum = ItemLoader.IndexedPrefabMaxVolume(transferedItemName);
        int selectedSlotQty = toStoreObjVar.intData[toSlot - 1];
        if ((transferedItemName == selectedSlotName && selectedSlotQty < selectedSlotMaximum) || selectedSlotName == "")
        {
            Debug.Log("MOOOVE");
            //REMOVE FROM FROM SLOT
            fromStoreObjVar.intData[fromSlot - 1]--;
            if (fromStoreObjVar.intData[fromSlot - 1] < 1)
            {
                fromStoreObjVar.stringData[fromSlot - 1] = "";
                fromMIUI.DoubleClickedSlot = 0;
                doubleClickedSlot = 0;
                doubleClickedUI = null;
                fromMIUI.CallUpdateAllSlotGraphics();
                toMIUI.CallUpdateAllSlotGraphics();
            }
            //ADD TO TOSLOT
            toStoreObjVar.stringData[toSlot - 1] = transferedItemName;
            toStoreObjVar.intData[toSlot - 1]++;
            SelectItem(toSlot, toMIUI);
            //UPDATE COLLECTABLE
            MakeCollectable(StoreIsEmpty());
            //UPDATE GRAPHICS
            fromMIUI.CallUpdateAllSlotGraphics();
            toMIUI.CallUpdateAllSlotGraphics();
          
        }
    }

}



//specific Actions

//   actionName = "";
//   actionStringValues = null;
//   actionFloatValues = null;
//   actionIntValues = null;
//   actionBoolValues = null;
//   debugText = "";
//   object.aM.DoActions;
