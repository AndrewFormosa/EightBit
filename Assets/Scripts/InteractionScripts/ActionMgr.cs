using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionMgr : MonoBehaviour
{
    public string actionName;
    public string[] actionStringValues;
    public float[] actionFloatValues;
    public int[] actionIntValues;
    public bool[] actionBoolValues;
    public string debugText;


    public void ResetVariables()
    {
        actionName = "";
        actionStringValues = new string[]{};
        actionFloatValues = new float[] { };
        actionIntValues = new int[] { };
        actionBoolValues = new bool[] { };
        debugText = "";
    }

    public void ActionCalled()
    {
        SendMessage("DoActions");       
    }

    public void DoActions()
    {

    }
}
