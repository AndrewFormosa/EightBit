using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_DoDamage : MonoBehaviour
{
    private ActionMgr aM;
    private GameObject playerObj;
    public string actionName;
    public int chanceOfBreakageOutOf1000;

    //action specific variables.....
    public float damagePotential;

    void Start()
    {
         actionName = "Use";
        aM = this.GetComponent<ActionMgr>();
        playerObj = GameObject.FindGameObjectWithTag("Player");
    }

    public void DoActions()
    {

        if (aM.actionName == actionName)
        {
            GameObject closeObject = InteractionScript.closeObject;
            

            //do damage to closeobject.
            if (closeObject != null)
            {
                
                ActionMgr closeObjectAM = InteractionScript.closeObject.GetComponent<ActionMgr>();
                closeObjectAM.ResetVariables();
                closeObjectAM.actionName = "ReceiveDamage";
                closeObjectAM.actionFloatValues = new float[] {damagePotential};
                closeObjectAM.actionStringValues = new string[] { gameObject.GetComponent<ObjVar>().Name };
                closeObjectAM.ActionCalled();
                if (Random.Range(1, chanceOfBreakageOutOf1000) == 1)
                {
                    UIManager.PrintToDialog("YOU HAVE BROKEN: " + gameObject.GetComponent<ObjVar>().Name);
                    playerObj.GetComponentInChildren<MyStoreScript>().RemoveFromInventorySlot(InteractionScript.openSlot, true);
                    InteractionScript.EmptyHands();
                }
            }
        
        }

        UIManager.CallUpDateUI();
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
