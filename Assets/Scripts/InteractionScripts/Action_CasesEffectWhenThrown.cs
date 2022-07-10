using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_CasesEffectWhenThrown : MonoBehaviour
{
    // public GameObject EffectingObjectPrefabs;
    public float minimumVelocity;
    public string ActionTriggered;
   
    public string[] stringValues;
    public int[] intValues;
    public float[] floatValues;



    public void OnCollisionEnter2D(Collision2D collision)
    {

        if (gameObject.GetComponent<Rigidbody2D>().velocity.magnitude > minimumVelocity)
        {
            if (!collision.gameObject.CompareTag("Player"))
            {
                ActionMgr closeObjectAM = collision.gameObject.GetComponent<ActionMgr>();

                closeObjectAM.ResetVariables();
                closeObjectAM.actionName = ActionTriggered;
                closeObjectAM.actionFloatValues = floatValues;
                closeObjectAM.actionStringValues = stringValues;
                closeObjectAM.actionIntValues = intValues;
                closeObjectAM.ActionCalled();
            }
        }




    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.GetComponent<Rigidbody2D>().velocity.magnitude > minimumVelocity)
        {
            if (!collision.gameObject.CompareTag("Player"))
            {
                ActionMgr closeObjectAM = collision.gameObject.GetComponent<ActionMgr>();

                closeObjectAM.ResetVariables();
                closeObjectAM.actionName = ActionTriggered;
                closeObjectAM.actionFloatValues = floatValues;
                closeObjectAM.actionStringValues = stringValues;
                closeObjectAM.actionIntValues = intValues;
                closeObjectAM.ActionCalled();
            }
        }
    }
}
