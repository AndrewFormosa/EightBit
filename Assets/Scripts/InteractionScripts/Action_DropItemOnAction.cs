using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_DropItemOnAction : MonoBehaviour
{
    private ActionMgr aM;
    private GameObject playerObj;
    private InteractionScript interaction;
    public PlayerStatusScript playerStatusScript;
    public string[] actionNames;
    public ItemDropProfile[] itemsToDrop;


    public PlayerStatus[] ReqiredStatus;
    public PlayerStatus[] AquiredStatusOnAction;
    public PlayerStatus[] AquiredStatusWhenDroped;



    //action specific variables.....
        void Start()
    {
        
        aM = this.GetComponent<ActionMgr>();
        playerObj = GameObject.FindGameObjectWithTag("Player");
        playerStatusScript = playerObj.GetComponent<PlayerStatusScript>();
        interaction = GameObject.FindObjectOfType<InteractionScript>();




    }

    public void DoActions()
    {


        for(int i = 0; i < actionNames.Length;i++)
        {
            if (aM.actionName == actionNames[i] && playerStatusScript.StatusRequirementMet(ReqiredStatus)) //check for action name & check that required status is met.
            {
                playerStatusScript.AddOrChangePlayerStatusArrayValues(AquiredStatusOnAction);
                UIManager.CallUpDateUI();

                for (int j = 0; j < itemsToDrop.Length;j++)
                {
                    float randomNo = Random.Range(0f, 1f);
                    if (randomNo < itemsToDrop[j].chanceOfDrop)
                    {
                        for (int k = 0; k < itemsToDrop[j].noOfItemsDroped; k++)
                        {
                            string key = SpaceManager.RegionToKey(SpaceManager.RegionVector(this.transform.position));
                            GameObject dropedObject = Instantiate(itemsToDrop[j].itemPrefab, new Vector3(this.transform.position.x + (0.2f * k), this.transform.position.y + (0.2f * k), this.transform.position.z), Quaternion.identity);
                            dropedObject.transform.SetParent(SpaceManager.LoadedRegions[key].transform);
                        }
                        playerStatusScript.AddOrChangePlayerStatusArrayValues(AquiredStatusWhenDroped);
                        UIManager.dialogTextTyper.TypeMessage(itemsToDrop[j].messageOnDrop, false, null, 1, true);
                       // UIManager.PrintToDialog(itemsToDrop[j].messageOnDrop);
                    }
                  

                }
                
                
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

[System.Serializable]
public class ItemDropProfile
{
    public GameObject itemPrefab;
    public int noOfItemsDroped;
    public float chanceOfDrop;
    public string messageOnDrop;

}