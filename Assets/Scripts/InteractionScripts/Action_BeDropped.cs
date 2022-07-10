using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_BeDropped : MonoBehaviour
{
    private ActionMgr aM;
    private GameObject playerObj;
    public string actionName;

    //action specific variables.....

    void Start()
    {
         actionName = "Drop";
        aM = this.GetComponent<ActionMgr>();
    }

    public void DoActions()
    {
        if (aM.actionName == actionName)
        {
            Debug.Log("DROP");
            //drop the held item at the forward position and make the region for that position the parent of the item.
            GameObject dropItem = InteractionScript.heldItem;//set item to drop to that which is referenced as held
            InteractionScript.heldItem = null;  
            string regionKey = SpaceManager.RegionToKey(SpaceManager.RegionVector(InteractionScript.forwardPosition));// get the Key for the region into which the item will be dropped
            dropItem.transform.position = InteractionScript.forwardPosition;//place the item in the position infront of player   
            dropItem.transform.SetParent(SpaceManager.LoadedRegions[regionKey].transform);//make the game object a child of the region.
            InteractionScript.myStoreScript.RemoveFromInventorySlot(InteractionScript.openSlot, true);

            //make the item visible.
            dropItem.GetComponent<SpriteRenderer>().enabled = true;
            dropItem.GetComponent<Collider2D>().enabled = true;
            UIManager.PrintToDialog("DROPPED: " + gameObject.GetComponent<ObjVar>().Name);
            UIManager.CallUpDateUI();

        }
    }

 
}
