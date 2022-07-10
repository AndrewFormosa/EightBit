using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_EffectsIfTouched : MonoBehaviour
{
   // public GameObject EffectingObjectPrefabs;
    public string ActionTriggered;
    public string[] stringValues;
    public int[] intValues;
    public float[] floatValues; 

  

    public void OnCollisionEnter2D(Collision2D collision)
    {
        //string collName = collision.gameObject.GetComponent<ObjVar>().Name;
        
        
          //  string killerName =EffectingObjectPrefabs.GetComponent<ObjVar>().Name;
       // if (killerName == collName)
       // {
            ActionMgr closeObjectAM = collision.gameObject.GetComponent<ActionMgr>();

            closeObjectAM.ResetVariables();
            closeObjectAM.actionName = ActionTriggered;
            closeObjectAM.actionFloatValues = floatValues;
            closeObjectAM.actionStringValues = stringValues;
            closeObjectAM.actionIntValues = intValues;
            closeObjectAM.ActionCalled();
      //  }
        
        


    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
      //  string collName = collision.gameObject.GetComponent<ObjVar>().Name;


      //  string killerName = EffectingObjectPrefabs.GetComponent<ObjVar>().Name;
      //  if (killerName == collName)
      //  {
            ActionMgr closeObjectAM = collision.gameObject.GetComponent<ActionMgr>();

            closeObjectAM.ResetVariables();
            closeObjectAM.actionName = ActionTriggered;
            closeObjectAM.actionFloatValues = floatValues;
            closeObjectAM.actionStringValues = stringValues;
            closeObjectAM.actionIntValues = intValues;
            closeObjectAM.ActionCalled();


       // }
    }
}
