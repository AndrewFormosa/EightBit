using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_Operate : MonoBehaviour
{
    private ActionMgr aM;
    private GameObject playerObj;
    public string actionName;

    //action specific variables.....
   

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
                closeObjectAM.actionName = "OperateOn";
                closeObjectAM.ActionCalled();
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
}
