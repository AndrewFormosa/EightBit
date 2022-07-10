using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_ReceiveDamage : MonoBehaviour
{
    private ActionMgr aM;
    private GameObject playerObj;
    public string actionName;

    //action specific variables.....
    public float damageLimit;
    public float TotalDamageRecieved;
    public bool destroyOnDeath;
    public bool collectOnDeath;
    public bool dropItemOnDeath;
    public GameObject ItemDropedOnDeath;
    public int qtyDropedOnDeath;
    public float damageReceived;
    private string bestItem;
    public GameObject bestItemObject;
    public int bestItemMultiplier;
    public bool onlyBestItem;
    public ItemDropProfile[] otherItemsDropedOnDeath;




    public PlayerStatus[] ReqiredStatus;
    public PlayerStatus[] AquiredStatus;
    public PlayerStatusScript playerStatusScript;
    void Start()
    {

         actionName = "ReceiveDamage";
        aM = this.GetComponent<ActionMgr>();
        playerObj = GameObject.FindGameObjectWithTag("Player");
        playerStatusScript = playerObj.GetComponent<PlayerStatusScript>();
        if (bestItemObject != null)
        {
            bestItem = bestItemObject.GetComponent<ObjVar>().Name;
        }
        else bestItem = "";

    }

    public void DoActions()
    {

        if (aM.actionName == actionName && playerStatusScript.StatusRequirementMet(ReqiredStatus))
        {

            if (aM.actionStringValues[0] == bestItem || (!onlyBestItem))
            {

                //GameObject closeObject = InteractionScript.closeObject;
                //ActionMgr closeObjectAM = InteractionScript.closeObject.GetComponent<ActionMgr>();

                damageReceived = 1f; //default

                if (aM.actionFloatValues.Length > 0) { damageReceived = aM.actionFloatValues[0]; }

                if(aM.actionStringValues[0] == bestItem)
                {
                    damageReceived = damageReceived * bestItemMultiplier;
                }

                if (InteractionScript.heldItem != null)
                {
                    TotalDamageRecieved += damageReceived;

                    if (TotalDamageRecieved > damageLimit)
                    {
                        Death();
                    }
                    gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, ((damageLimit - (TotalDamageRecieved / 2)) / damageLimit));

                }
            }
        }
    }

    public void Death()
    {
        if (destroyOnDeath) { Destroy(gameObject); }
        if (collectOnDeath)
        {
            ResetAttributes();
            aM.ResetVariables();
            aM.actionName = "CollectOnDeath";
            aM.ActionCalled();
        }
        if (dropItemOnDeath)
        {
            for (int i = 0; i < qtyDropedOnDeath; i++)
            {
                string key = SpaceManager.RegionToKey(SpaceManager.RegionVector(this.transform.position));
                GameObject dropedObject = Instantiate(ItemDropedOnDeath, new Vector3(this.transform.position.x+(0.2f*i), this.transform.position.y+(0.2f*i), this.transform.position.z), Quaternion.identity);
                dropedObject.transform.SetParent(SpaceManager.LoadedRegions[key].transform);
               
            }
            UIManager.dialogTextTyper.TypeMessage("YOU HAVE DESTROYED: " + gameObject.GetComponent<ObjVar>().Name+ ", LEAVING BEHIND: " + ItemDropedOnDeath.GetComponent<ObjVar>().Name, false, null, 1, true);
            playerStatusScript.AddOrChangePlayerStatusArrayValues(AquiredStatus);


            for (int j = 0; j < otherItemsDropedOnDeath.Length; j++)
            {
                float randomNo = Random.Range(0f, 1f);
                if (randomNo < otherItemsDropedOnDeath[j].chanceOfDrop)
                {
                    for (int k = 0; k < otherItemsDropedOnDeath[j].noOfItemsDroped; k++)
                    {
                        string key = SpaceManager.RegionToKey(SpaceManager.RegionVector(this.transform.position));
                        GameObject dropedObject = Instantiate(otherItemsDropedOnDeath[j].itemPrefab, new Vector3(this.transform.position.x + (0.2f * k), this.transform.position.y + (0.2f * k), this.transform.position.z), Quaternion.identity);
                        dropedObject.transform.SetParent(SpaceManager.LoadedRegions[key].transform);
                    }
                    UIManager.dialogTextTyper.TypeMessage(otherItemsDropedOnDeath[j].messageOnDrop, false, null, 1, true);
                    // UIManager.PrintToDialog(itemsToDrop[j].messageOnDrop);
                }
            }


                Destroy(gameObject);
        }

    }

    public void ResetAttributes()
    {
        TotalDamageRecieved = 0;
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




