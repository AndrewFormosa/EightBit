using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_Default : MonoBehaviour
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
        // actionName = "";
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

         


            //Get close object details.******************
            // GameObject closeObject = InteractionScript.closeObject;
           // if (closeObject != null)
           // {
            //    ActionMgr closeObjectAM = InteractionScript.closeObject.GetComponent<ActionMgr>();
            //    closeObjectAM.ResetVariables();
            //    closeObjectAM.actionName = "ReceiveDamage";
            //    closeObjectAM.actionFloatValues = ;
            //    closeObjectAM.ActionCalled();
          //  }




            //  playerStatusScript.AddOrChangePlayerStatusArrayValues(AquiredStatus);//Add or change player status.
            //  UIManager.PrintToDialog("" + gameObject.GetComponent<ObjVar>().Name);//Print to main dialog box.

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

}
