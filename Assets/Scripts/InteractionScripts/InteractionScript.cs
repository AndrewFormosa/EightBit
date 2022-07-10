using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InteractionScript : MonoBehaviour
{
    private ObjVar objVar;
    public static GameObject closeObject;
    public static Vector3 forwardPosition;
    public GameObject emptyHands;
    public static GameObject hands;
    public GameObject personalStoreObject;
    public static GameObject PersonalStoreObject;

    public static MyStoreScript myStoreScript;

    public static bool playerInputAllowed;
    public static GameObject heldItem;
    public static int openSlot;
    public GameObject playerHolding;
    public static GameObject PlayerHolding;

    public GameObject debugHeldItem;
    public GameObject debugCloseObject;

  
  


    // Start is called before the first frame update
    void Start()
    {
        myStoreScript = FindObjectOfType<MyStoreScript>();

        objVar = this.GetComponent<ObjVar>();
        forwardPosition = new Vector3(0, 0, 0);
        PlayerHolding = playerHolding;

        hands = emptyHands;
        playerInputAllowed = true;
        PersonalStoreObject = personalStoreObject;
        heldItem = Instantiate(emptyHands, playerHolding.transform);
        
   }

    


    public static void EmptyHands()
    {
        Destroy(heldItem);

        heldItem =  Instantiate(hands,PlayerHolding.transform) ;UIManager.CallUpDateUI();
        UIManager.CallUpDateUI();
    }

    public static void HoldSelectedItem(GameObject selectedItem)
     {
        if (selectedItem != null)
        {          
            Destroy(heldItem);               
            heldItem = selectedItem;
            heldItem.transform.SetParent(PlayerHolding.transform);
            heldItem.GetComponent<SpriteRenderer>().enabled = false;
            heldItem.GetComponent<Collider2D>().enabled = false;
        }
        else
        {
           EmptyHands();
           
        }
        UIManager.CallUpDateUI();
     }

    // Update is called once per frame
    void Update()
    {


        if (playerInputAllowed)//check that player input is allowed - ie no UI is open.
        {

            if (Input.GetKeyDown(KeyCode.Keypad8))
            {
                ObjVar storeObjVar = personalStoreObject.GetComponent<ObjVar>();
                if (openSlot < storeObjVar.stringData.Length)
                {
                    openSlot++;
                    string nextObject = storeObjVar.stringData[openSlot - 1];
                    myStoreScript.CreateItemAndToHold(nextObject);
                    UIManager.CallUpDateUI();
                }
            }

            if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                ObjVar storeObjVar = personalStoreObject.GetComponent<ObjVar>();
                if (openSlot > 1)
                {
                    openSlot--;
                    string nextObject = storeObjVar.stringData[openSlot - 1];
                    myStoreScript.CreateItemAndToHold(nextObject);
                    UIManager.CallUpDateUI();
                }
            }





            if (Input.GetKeyDown(KeyCode.T)) { OpenInventory(); } //DEBUG TESTER

            debugCloseObject = closeObject;/////*********DEBUG
            debugHeldItem = heldItem;
            if (!objVar.isMoving) // if player in not moving then allow to try and select an object or a position.
            {
                //if space key pressed and if there is an object in front of the player then select the object and act on the obejct.
                if (Input.GetKeyDown(KeyCode.Return))//ACT WITH HELD OBJECT
                {
                    PrepareInteractionVariables();
                    Use();
                }
                //if return key pressed and if there is not object in front of the player then select the position and act on position.
                if (Input.GetKeyDown(KeyCode.Space))//DROP AND COLLECT
                {
                    PrepareInteractionVariables();
                    GetAndDrop();

                }

            }
        }

    }

   

    public void PrepareInteractionVariables()
    {
        closeObject = GetCloseObject();
        forwardPosition = GetForwardPosition();
    }


    public GameObject GetCloseObject()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + objVar.directionVector, objVar.directionVector, 0.1f);
        if (hit)
        {
            return hit.transform.gameObject;
        }
        else
        { return null; }

    }

    public Vector3 GetForwardPosition()
    {
        Vector3 fPosition = new Vector3(0, 0, 0);
        Vector3 offSet = new Vector3(0.4f, 0.4f, 0);
        if (transform.position.x < 0) { offSet.x = -0.4f; }
        if (transform.position.y < 0) { offSet.y = -0.4f; }
        fPosition.x = (int)((transform.position.x + offSet.x + objVar.directionVector.x));
        fPosition.y = (int)((transform.position.y + offSet.y + objVar.directionVector.y));
        return fPosition;
    }

    public void GetAndDrop()
    {
        if (closeObject == null)
        {
            ActionMgr actionManager =heldItem.GetComponent<ActionMgr>();
            actionManager.ResetVariables();
            actionManager.actionName="Drop";
            actionManager.ActionCalled();
        }
        if (closeObject != null && closeObject!=gameObject)
        {

            ActionMgr actionManager = closeObject.GetComponent<ActionMgr>();
            actionManager.ResetVariables();
            actionManager.actionName = "Collect";
            actionManager.ActionCalled();
          //  GameObject.FindGameObjectWithTag("UI").GetComponent<Text_Typer_Script>().TypeMessage("object Collected", true, ()=>{OnTypeComplete();},1f);
        }
   
     }

 //  public void OnTypeComplete()
  //  {
 //      GameObject.FindGameObjectWithTag("UI").GetComponent<Text_Typer_Script>().TypeMessage("TypeComplete", false, null,1f);
  //  }



    public void Use()
    {
        Debug.Log("try&Use");
        ActionMgr actionManager = InteractionScript.heldItem.GetComponent<ActionMgr>();
        string usedItem = actionManager.gameObject.GetComponent<ObjVar>().Name;
        actionManager.ResetVariables();
        UIManager.dialogTextTyper.TypeMessage("YOU TRY TO USE: " + usedItem, false, null, 1);
        actionManager.actionName = "Use";

        actionManager.ActionCalled();
    }

    public void OpenInventory()
    {
        Debug.Log("try open inventory"); MyStoreScript myStoreScript = personalStoreObject.GetComponent<MyStoreScript>();
        myStoreScript.OpenUpMyStore();

    }

}
