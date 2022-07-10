using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyStoreScript : MonoBehaviour
{
    public GameObject multiItem_UIPrefab;
    public GameObject crafterPrefab;
    public SaveableScipt saveableScript;
    public LoadableScript loadableScript;
 
   // public ItemLoader itemLoader;

    // Start is called before the first frame update
    void Start()
    {
       loadableScript.ConfigureItem(DataManager.LoadInventory());

    }

    void OnDestroy()
    {
        DataManager.SaveInventory(saveableScript.GetSavableData());    
    }


    public void OpenUpMyStore()
    {
        InteractionScript.playerInputAllowed = false; //stop player input taken.
        Debug.Log("OPEN MY STORE");
        GameObject storeUI = Instantiate(multiItem_UIPrefab, gameObject.transform);
        MultiItem_UI_Script StoreUIScript = storeUI.GetComponent<MultiItem_UI_Script>();
        ObjVar storeData = gameObject.GetComponent<ObjVar>();
        StoreUIScript.displayPosX = 0f;
        StoreUIScript.displayWidth = 600f;
        StoreUIScript.PlayerInventoryStore = true;
        StoreUIScript.SetUpDisplay(storeData.Name, storeData.stringData, storeData.intData);
        StoreUIScript.SlotSingClicked += slotSingleClicked;
        StoreUIScript.SlotDoubClicked += slotDoubleClicked;   
    }

  



    public void slotSingleClicked(int slotNumber, MultiItem_UI_Script UI_Script)
    {
        if (UI_Script.DoubleClickedSlot == 0)
        {
            Debug.Log("Single Clicked:" + slotNumber + UI_Script.mainTitle.text);
            SelectAndHoldItem(slotNumber, UI_Script);
        }else
        if (UI_Script.DoubleClickedSlot==slotNumber)
        {
            UI_Script.DoubleClickedSlot = 0;
            SelectAndHoldItem(slotNumber, UI_Script);
            UI_Script.CallUpdateAllSlotGraphics();
        }
        else { TryAndTransferItem(UI_Script, UI_Script.DoubleClickedSlot, UI_Script, slotNumber); }
    }

    public void SelectAndHoldItem(int slotNumber, MultiItem_UI_Script UI_Script)
    {
        UI_Script.SingleClickedSlot = slotNumber;
        CreateItemAndToHold(UI_Script.itemsNames[slotNumber - 1]);
        InteractionScript.openSlot = slotNumber;
        UI_Script.CallUpdateAllSlotGraphics();
    }


    public void TryAndTransferItem(MultiItem_UI_Script fromMIUI, int fromSlot, MultiItem_UI_Script toMIUI, int toSlot)
    {

        ObjVar fromStoreObjVar = fromMIUI.gameObject.GetComponentInParent<ObjVar>();
        ObjVar toStoreObjVar = toMIUI.gameObject.GetComponentInParent<ObjVar>();
        Debug.Log("FromSlotNo:" + fromSlot);
        string transferedItemName = fromStoreObjVar.stringData[fromSlot - 1];
        string selectedSlotName = toStoreObjVar.stringData[toSlot - 1];
        int selectedSlotMaximum = ItemLoader.IndexedPrefabMaxVolume(transferedItemName);
        int selectedSlotQty = toStoreObjVar.intData[toSlot-1];
        if((transferedItemName==selectedSlotName && selectedSlotQty < selectedSlotMaximum)||selectedSlotName=="")
           {
            Debug.Log("MOOOVE");
            RemoveFromInventorySlot(fromSlot, false);
            AddItemToSlot(transferedItemName, toSlot);           
            if (fromStoreObjVar.intData[fromSlot - 1] < 1)
            {
                fromMIUI.DoubleClickedSlot = 0;
            }
            SelectAndHoldItem(toSlot, toMIUI);
           }

    }


    public void slotDoubleClicked(int slotNumber, MultiItem_UI_Script UI_Script)
    {
        if (SlotEmpty(slotNumber))
        {
            GameObject crafter = Instantiate(crafterPrefab, gameObject.transform);
            crafter.GetComponent<Craft_Script>().targetSlot = slotNumber;
            crafter.GetComponent<Craft_Script>().targetMIUI_Script = UI_Script;
                 
        }
        else
        {
            if (slotNumber == UI_Script.DoubleClickedSlot)
            {
                UI_Script.DoubleClickedSlot = 0;
                SelectAndHoldItem(slotNumber, UI_Script);
                UI_Script.CallUpdateAllSlotGraphics();
            }
            else
            {
                Debug.Log("Double Clicked:" + slotNumber + UI_Script.mainTitle.text);
                UI_Script.DoubleClickedSlot = slotNumber;
                UI_Script.CallUpdateAllSlotGraphics();
            }
        }
    }

    public void CreateItemAndToHold(string itemNameID)
    {
        if (itemNameID != "")
        {
            GameObject newItem = Instantiate(ItemLoader.IndexedPrefab(itemNameID));
            InteractionScript.HoldSelectedItem(newItem);
        }
        else { InteractionScript.HoldSelectedItem(null); }
    }

    public void RemoveFromInventorySlot(int slot, bool holdNextItem)
    {
        ObjVar storeObjVar = this.gameObject.GetComponent<ObjVar>();
        string[] storedItemNames = storeObjVar.stringData;//array of stored item names
        int[] storedItemQtys = storeObjVar.intData;//array of stored item quantitys

        storedItemQtys[slot - 1]--;
        if (storedItemQtys[slot - 1] < 1)
        {
            storedItemNames[slot - 1] = "";
        }

        if (holdNextItem)
        {
            CreateItemAndToHold(storedItemNames[slot - 1]);

        }

    }


    public void RemoveNamedItemsFromInventory(string itemNameToRemove, int qtyToRemove)
    {
        ObjVar storeObjVar = this.gameObject.GetComponent<ObjVar>();
        string[] storedItemNames = storeObjVar.stringData;//array of stored item names
        int[] storedItemQtys = storeObjVar.intData;//array of stored item quantitys
        int qtyRemaining = qtyToRemove;

        for (int i=0; i < storedItemNames.Length; i++)//go through full inventory
        {
            if (qtyToRemove > 0)//is there anything left to remove
            {
                if (storedItemNames[i] == itemNameToRemove)//if check if this item is held here
                {
                    int ActualQtyRemoved = Mathf.Min(qtyToRemove, storedItemQtys[i]);
                    storedItemQtys[i] -= ActualQtyRemoved;
                    if (storedItemQtys[i] < 1) { storedItemNames[i] = ""; }
                    qtyToRemove -= ActualQtyRemoved;
                }                                
            }
        }

        
        Debug.Log("removedNamedItems");
    }


    //FUNCTIONS TO EXAMINE STORAGE CONTAINER AND CONTENTS
    public int GetBestFreeInventorySlot(GameObject collectedItem)
    {      
        string collectedItemName =collectedItem.GetComponent<ObjVar>().Name;//name of collected item
        int maxQtyForItem = collectedItem.GetComponent<Action_BeCollected>().MaxQtyInInventorySlot;// most of collected item in a slot

        ObjVar storeObjVar = this.gameObject.GetComponent<ObjVar>();
        string[] storedItemNames = storeObjVar.stringData;//array of stored item names
        int[] storedItemQtys = storeObjVar.intData;//array of stored item quantitys

        for(int i = 0; i < storedItemNames.Length; i++)
        {
            if(storedItemNames[i]==collectedItemName && storedItemQtys[i] < maxQtyForItem) { return i + 1; }
        }
        for (int i = 0; i < storedItemNames.Length; i++)
        {
            if (storedItemQtys[i]<1) { return i + 1; }
        }
        return 0;
    }

    public bool InventorySpaceAvailable(GameObject collectedItem)
    {
        if (GetBestFreeInventorySlot(collectedItem) != 0) { return true; } else { return false; }
    }

    public void AddItemToBestSlot(GameObject collectedItem)
    {
        int bestSlot = GetBestFreeInventorySlot(collectedItem);
        string collectedItemName = collectedItem.GetComponent<ObjVar>().Name;//name of collected item

        ObjVar storeObjVar = this.gameObject.GetComponent<ObjVar>();
        string[] storedItemNames = storeObjVar.stringData;//array of stored item names
        int[] storedItemQtys = storeObjVar.intData;//array of stored item quantitys

        if (InventorySpaceAvailable(collectedItem))
        {
            storedItemNames[bestSlot - 1] = collectedItemName;
            storedItemQtys[bestSlot - 1]++;
        }
    }

    public void AddItemToSlot(GameObject collectedItem, int slot)
    {
        string collectedItemName = collectedItem.GetComponent<ObjVar>().Name;//name of collected item
        ObjVar storeObjVar = this.gameObject.GetComponent<ObjVar>();
        string[] storedItemNames = storeObjVar.stringData;//array of stored item names
        int[] storedItemQtys = storeObjVar.intData;//array of stored item quantitys

        if (InventorySpaceAvailable(collectedItem))
        {
            storedItemNames[slot - 1] = collectedItemName;
            storedItemQtys[slot - 1]++;
        }
    }

    public void AddItemToSlot(string collectedItem, int slot)
    {
        string collectedItemName = collectedItem;//name of collected item
        ObjVar storeObjVar = this.gameObject.GetComponent<ObjVar>();
        string[] storedItemNames = storeObjVar.stringData;//array of stored item names
        int[] storedItemQtys = storeObjVar.intData;//array of stored item quantitys

            storedItemNames[slot - 1] = collectedItemName;
            storedItemQtys[slot - 1]++;
    }

    public bool SlotEmpty(int slot)
    {   ObjVar storeObjVar = this.gameObject.GetComponent<ObjVar>();
        int[] storedItemQtys = storeObjVar.intData;//array of stored item quantitys
        if (storedItemQtys[slot - 1] < 1) { return true; } else { return false; }
    }

    public  bool StoreContainsItem(GameObject item)
    {
        ObjVar storeData = gameObject.GetComponent<ObjVar>();
        string searchItem = item.GetComponent<ObjVar>().Name;

        foreach (string itemName in storeData.stringData)
        {
            if (itemName == searchItem) { return true; }
        }
        return false;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
