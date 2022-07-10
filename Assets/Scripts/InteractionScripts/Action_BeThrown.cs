using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_BeThrown : MonoBehaviour
{
    private ActionMgr aM;
    private GameObject playerObj;
    private InteractionScript interaction;
    private PlayerStatusScript playerStatusScript;

    public string actionName;

    public GameObject InfinateThrownItem;
    public float throwForce;
    public bool destroyAfterTime;
    public float destoryTime;
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


            Vector3 playerDirection = playerObj.GetComponent<ObjVar>().directionVector;
            Vector3 playerPosition = playerObj.transform.position;


            Debug.Log("Throw");
            //drop the held item at the forward position and make the region for that position the parent of the item.

          
            GameObject dropItem = InteractionScript.heldItem;//set item to drop to that which is referenced as held
           
            if (!(InfinateThrownItem == null))
            {
               dropItem = Instantiate(InfinateThrownItem);//set item to drop to that which is referenced as held
            }


            if (InfinateThrownItem == null)
            {
                InteractionScript.heldItem = null;
                InteractionScript.myStoreScript.RemoveFromInventorySlot(InteractionScript.openSlot, true);
            }

         
            string regionKey = SpaceManager.RegionToKey(SpaceManager.RegionVector(InteractionScript.forwardPosition));// get the Key for the region into which the item will be dropped
            dropItem.transform.position = InteractionScript.forwardPosition;//place the item in the position infront of player   
            dropItem.transform.SetParent(SpaceManager.LoadedRegions[regionKey].transform);//make the game object a child of the region.
        

            //make the item visible.
            dropItem.GetComponent<SpriteRenderer>().enabled = true;
            dropItem.GetComponent<Collider2D>().enabled = true;
            dropItem.transform.rotation = Quaternion.AngleAxis(Rotation(playerDirection, -45f), new Vector3(0, 0, 1));
            dropItem.GetComponent<Rigidbody2D>().AddForce(playerDirection*throwForce,ForceMode2D.Impulse);

            UIManager.PrintToDialog("DROPPED: " + gameObject.GetComponent<ObjVar>().Name);
            UIManager.CallUpDateUI();

            if (destroyAfterTime)
            {
                StartCoroutine("DestroyAfterTime", dropItem);
            }

            playerStatusScript.AddOrChangePlayerStatusArrayValues(AquiredStatus);//Add or change player status.


        }
    }

    public IEnumerator DestroyAfterTime(GameObject dropedItem)
    {
        Debug.Log("Destoryy");
        yield return new WaitForSeconds(destoryTime);
        Destroy(dropedItem);        
    }


    public float Rotation(Vector3 directionVector, float startingAngle)
    {
        float angle = 0;
        float[] x = {1f,1f,0f,-1f,-1f,-1f,0f,1f };
        float[] y = { 0f, -1f, -1f, -1f, 0f, 1f, 1f, 1f };
        float[] angles = { 0f, -45f, -90f, -135f, -180f, -225f, -270f, -315f };
        for (int i = 0; i < 8; i++)
        {
         if (directionVector.x == x[i] && directionVector.y == y[i])
            {
                angle = angles[i];
            }
        }
 
        return angle+startingAngle;
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
