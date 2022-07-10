using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_BeCollected : MonoBehaviour
{
    private ActionMgr aM;
    private GameObject playerObj;
    private InteractionScript interaction;
    public PlayerStatusScript playerStatusScript;
    public string actionName;

    public int MaxQtyInInventorySlot;
    public bool onlyCollectOnDeath;
    public bool HoldAferCollection;

    public PlayerStatus[] ReqiredStatus;
    public PlayerStatus[] AquiredStatus;

  



    void Start()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        playerStatusScript = playerObj.GetComponent<PlayerStatusScript>();
        interaction = GameObject.FindObjectOfType<InteractionScript>();
        actionName = "Collect";
        if (onlyCollectOnDeath)
        { actionName = "CollectOnDeath"; }
        aM = this.GetComponent<ActionMgr>();
        playerObj = GameObject.FindGameObjectWithTag("Player");
        
    }

    //test
  
    public void DoActions()
    {

        if (aM.actionName == actionName && playerStatusScript.StatusRequirementMet(ReqiredStatus))
        {
            if (this.enabled)
            {

                if (InteractionScript.myStoreScript.InventorySpaceAvailable(gameObject))
                {
                    int selecteInventorySlot = InteractionScript.myStoreScript.GetBestFreeInventorySlot(gameObject);
                    Debug.Log("COLLLECT" + actionName);

                    InteractionScript.myStoreScript.AddItemToSlot(gameObject, selecteInventorySlot);


                    UIManager.PrintToDialog("COLLECTED: " + gameObject.GetComponent<ObjVar>().Name);

                    if (HoldAferCollection)
                    {
                        InteractionScript.HoldSelectedItem(gameObject);
                        InteractionScript.openSlot = selecteInventorySlot;
                    }
                    else
                    {
                        Destroy(this.gameObject);
                    }
                }
            }else{ UIManager.PrintToDialog("YOU CANT COLLECT THIS "+gameObject.GetComponent<ObjVar>().Name); }

            UIManager.CallUpDateUI();
        }
       
    }

}
