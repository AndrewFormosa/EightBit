using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_EatenWhenHeld : MonoBehaviour
{
    private ActionMgr aM;
    private GameObject playerObj;
    private InteractionScript interaction;
    public PlayerStatusScript playerStatusScript;
    public string actionName;


    public PlayerStatus[] ReqiredStatus;
    public PlayerStatus[] AquiredStatus;



    //action specific variables.....

    void Start()
    {
         actionName = "Use";
        aM = this.GetComponent<ActionMgr>();
        playerObj = GameObject.FindGameObjectWithTag("Player");
        playerStatusScript = playerObj.GetComponent<PlayerStatusScript>();
        interaction = GameObject.FindObjectOfType<InteractionScript>();




    }

    public void DoActions()
    {




        if (aM.actionName == actionName && playerStatusScript.StatusRequirementMet(ReqiredStatus)) //check for action name & check that required status is met.
        {
            //GameObject closeObject = InteractionScript.closeObject;
            //ActionMgr closeObjectAM = InteractionScript.closeObject.GetComponent<ActionMgr>();



              playerStatusScript.AddOrChangePlayerStatusArrayValues(AquiredStatus);//Add or change player status.
            UIManager.dialogTextTyper.TypeMessage("YUM YUM!! YOU HAVE JUST EATEN: " + gameObject.GetComponent<ObjVar>().Name,false,null,1,true);
            //  UIManager.PrintToDialog(//Print to main dialog box.
              InteractionScript.myStoreScript.RemoveFromInventorySlot(InteractionScript.openSlot, true);              
              //InteractionScript.EmptyHands();
              
            

        }
    }
}
