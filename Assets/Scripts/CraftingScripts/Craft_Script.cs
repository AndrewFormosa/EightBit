using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Craft_Script : MonoBehaviour
{
   public int targetSlot;//slotinto which the new item will be placed.
    
    public GameObject multiItemUIPrefab; // the mulitItemUI;
    public MyStoreScript myStoreScript;
    private GameObject crafterMIUI;
    public MultiItem_UI_Script crafterMIUI_Script;
    public MultiItem_UI_Script targetMIUI_Script;
    public List<string> allNamesInInventory;
    public List<int> allQtysInInventory;
    public List<string> allCraftableItemNames;
    
    public List<string> inventoryItemsGatheredForDestruction;
    public List<GameObject> craftablePrefabs;
    public List<GameObject> currentCraftablePrefabs;
    public List<string> currentCraftableItemNames;
    public List<int> currentCraftableQtys;

    public int singleSelectedSlot;
    public int doubleSelectedSlot;

    public GameObject ItemActionUIPrefab;
    public GameObject ItemActionUI;

    public GameObject player;
    public PlayerStatusScript playerStatus;

    // Start is called before the first frame update
    void Start()
    {
        myStoreScript = FindObjectOfType<MyStoreScript>();
        playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatusScript>();
        singleSelectedSlot = 0;
        doubleSelectedSlot = 0;

        CreateListOfCraftablePrefabs();//create a new list of all loadable prfabs that have a craftable script.
        CreateListOfAllInventory();//create a tally of everything in the player inventory
        CreateListOfCurrentlyCraftablePrefabs();
        crafterMIUI = Instantiate(multiItemUIPrefab, this.gameObject.transform);
        crafterMIUI_Script = crafterMIUI.GetComponent<MultiItem_UI_Script>();

        crafterMIUI_Script.SlotSingClicked += SlotSingleClicked;
        crafterMIUI_Script.SlotDoubClicked += SlotDoubleClicked;
        crafterMIUI_Script.CloseClicked += OnCloseClicked;

        crafterMIUI_Script.displayWidth = 600;
        crafterMIUI_Script.displayPosX = 0;
        crafterMIUI_Script.dispaySortingOrder=2;
        crafterMIUI_Script.SetUpDisplay("CRAFTING", currentCraftableItemNames.ToArray(), currentCraftableQtys.ToArray());
    }

    public void SlotSingleClicked(int slot, MultiItem_UI_Script MIUI)
    {
        if (doubleSelectedSlot == 0)
        {
            singleSelectedSlot = slot;
            MIUI.SingleClickedSlot = slot;
            MIUI.CallUpdateAllSlotGraphics();
        }
    }

    public void SlotDoubleClicked(int slot, MultiItem_UI_Script MIUI)
    {
        ItemActionUI = Instantiate(ItemActionUIPrefab, gameObject.transform);
        doubleSelectedSlot = slot;
        ItemAction_UI_Script itemAction_UI_Script = ItemActionUI.GetComponent<ItemAction_UI_Script>();
        itemAction_UI_Script.SetUpParameters(currentCraftablePrefabs[slot - 1].GetComponent<SpriteRenderer>().sprite.texture, currentCraftableQtys[slot - 1]);
        itemAction_UI_Script.craftMIUI = MIUI;
        itemAction_UI_Script.craftScript = this;

    }

    public void OnCloseClicked(MultiItem_UI_Script MIUI)
    {
        Destroy(gameObject);
    }

    public void OnDestroy()
    {
        targetMIUI_Script.SingleClickedSlot = 0;
        targetMIUI_Script.DoubleClickedSlot = 0;
        targetMIUI_Script.CallUpdateAllSlotGraphics();
    }

    public void OnCraftItem(int qtyToCraft)
    {
        Debug.Log("Craft " + qtyToCraft + " " +currentCraftableItemNames[doubleSelectedSlot-1]);
        GameObject itemToCraft = currentCraftablePrefabs[doubleSelectedSlot-1];

        //Remove the destroyable ingredents from the inventory     
        List<Ingrenient> itemToCraftIngredients = new List<Ingrenient>();
        itemToCraftIngredients = itemToCraft.GetComponent<Craftable>().IngredientsList;
        foreach (Ingrenient ingredient in itemToCraftIngredients)
        {
            if (ingredient.ingredientDestroyed)
            {        
                string itemNameToRemove = ingredient.ingredientItem.GetComponent<ObjVar>().Name;
                int qtyToRemove = ingredient.ingredientQuantity*qtyToCraft;
                myStoreScript.RemoveNamedItemsFromInventory(itemNameToRemove, qtyToRemove);
            }
        }

        //Add experience and points.
        Craftable craftable = itemToCraft.GetComponent<Craftable>();
        PlayerStatus[] statusAquired=craftable.gainedStatus;
        for (int i = 0; i < qtyToCraft; i++)
        {
            playerStatus.AddOrChangePlayerStatusArrayValues(statusAquired);
        }

        //add the craftedItemto the inventory.
        string itemToCraftName = itemToCraft.GetComponent<ObjVar>().Name;

        for (int i = 0; i < qtyToCraft; i++)
        {
            myStoreScript.AddItemToSlot(itemToCraftName, targetSlot);
         }


        UIManager.CallUpDateUI();
        targetMIUI_Script.CallUpdateAllSlotGraphics();
        Destroy(gameObject);
 
    }




    public int[] ArrayOfOnes(int arrayLength)
    {
        int[] singlesArray = new int[arrayLength];
        for (int i = 0; i < arrayLength; i++)
        {
            singlesArray[i] = 1;
        }
        return singlesArray;
    }

    public void CreateListOfCurrentlyCraftablePrefabs()
    {
        currentCraftablePrefabs = new List<GameObject>();//instantiate empty list of currently craftable prefabs based on what is in inventory.
        currentCraftableItemNames = new List<string>();//ininstantiate empty list of currently craftable prefabs Names based on what is in inventory.

        //go through the list of craftable prefabs
        for (int prefabNo = 0; prefabNo < craftablePrefabs.Count; prefabNo++)
        {
            Debug.Log("prefabNo:" + prefabNo);

            Craftable[] craftOptions = craftablePrefabs[prefabNo].GetComponents<Craftable>();

            foreach (Craftable craftable in craftOptions)
            {

                //Craftable craftable = craftablePrefabs[prefabNo].GetComponent<Craftable>();
                List<Ingrenient> allIngredients = craftable.IngredientsList;
                //status details
                PlayerStatus[] statusRequired = craftable.requiredStatus;

                int craftableQty = 9999;
                bool isCraftable = true; // initiate that the prefab is craftable then check if all requirements are met. 

                if (!playerStatus.StatusRequirementMet(statusRequired))
                {
                    isCraftable = false; craftableQty = 0;
                }

                //first check if you have the necessary experience....if not then it is not craftable.         
                //  for (int i = 0; i < statusRequired.Length; i++)

                //          //  {
                //   if (playerStatus.PlayerStatusContains(statusRequired[i].statusTitle.ToString()))
                //   {
                //       if (playerStatus.GetValueOfChosenStatus(statusRequired[i].statusTitle.ToString()) < (statusRequired[i].statusValue))
                //       {
                //           isCraftable = false; craftableQty = 0;
                //       }
                //   }
                //   else { isCraftable = false; craftableQty = 0; }
                //  }


                //go through list of indredients of each craftable prefab.
                for (int ingredientNo = 0; ingredientNo < allIngredients.Count; ingredientNo++)
                {
                    Ingrenient ingredient = allIngredients[ingredientNo];
                    string ingredientName = ingredient.ingredientItem.GetComponent<ObjVar>().Name;
                    int ingredentRequiredQty = ingredient.ingredientQuantity;
                    bool ingredientIsDestroyed = ingredient.ingredientDestroyed;

                    //check if inventrory contains the ingredient name
                    if (!allNamesInInventory.Contains(ingredientName)) { isCraftable = false; craftableQty = 0; } //if it doesnt then it is not craftable.
                    else
                    {
                        //check if inventory has required qty of that ingredient
                        int containedNameIndex = allNamesInInventory.IndexOf(ingredientName);
                        int inventoryQty = allQtysInInventory[containedNameIndex];
                        if (inventoryQty < ingredentRequiredQty) { isCraftable = false; craftableQty = 0; }
                        else // if there are enouth ingredients then determine the maximum number of items that can be crafted.
                        {
                            int posibleCrafableQty = (inventoryQty / ingredentRequiredQty) * craftable.QuantityProduced;
                            if (posibleCrafableQty < craftableQty && ingredientIsDestroyed) { craftableQty = posibleCrafableQty; }

                        } //if it doesnt then it is not craftable
                    }
                }

                //if after all checks, the item is craftable then add it to the lists and 
                if (isCraftable)
                {


                    currentCraftablePrefabs.Add(craftablePrefabs[prefabNo]);
                    currentCraftableItemNames.Add(craftablePrefabs[prefabNo].GetComponent<ObjVar>().Name);//Add the name to a string list to pass to the Multi Item UI.
                    int maximumCapacityOfSlot = craftablePrefabs[prefabNo].GetComponent<Action_BeCollected>().MaxQtyInInventorySlot;
                    if (craftableQty > maximumCapacityOfSlot) { craftableQty = maximumCapacityOfSlot; }//check that maximum slot capacity is not exceeded.********************
                    currentCraftableQtys.Add(craftableQty);
                }
            }
        }
    }




    public void CreateListOfCraftablePrefabs()
    {
        craftablePrefabs = new List<GameObject>();
        foreach(GameObject loadableObject in ItemLoader.LoadableObjects)
        {
           
            if (loadableObject.GetComponent<Craftable>() != null)
            {
                craftablePrefabs.Add(loadableObject);
            }
        }
    }

    public void CreateListOfAllInventory()
    {
       allNamesInInventory = new List<string>();
        allQtysInInventory = new List<int>();
        
        for (int i = 0; i < targetMIUI_Script.itemsNames.Length; i++)
        {           
            string name = targetMIUI_Script.itemsNames[i];
            int qty = targetMIUI_Script.itemsValues[i];
            if (qty > 0)
            {
                if (!allNamesInInventory.Contains(name))
                {
                    allNamesInInventory.Add(name);
                    allQtysInInventory.Add(qty);
                }
                else
                {
                    int indexOfItem = allNamesInInventory.IndexOf(name);
                    allQtysInInventory[indexOfItem] += qty;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
