using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_AllowObjectsToPass : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private GameObject playerObj;
    private InteractionScript interaction;
    private PlayerStatusScript playerStatusScript;
    private MyStoreScript myStoreScript;

  
    public PlayerStatus[] ReqiredStatus;
    public GameObject[] requiredItemsInInventory;
   


    // Start is called before the first frame update
    void Start()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        myStoreScript = GameObject.FindObjectOfType<MyStoreScript>();
        playerStatusScript = playerObj.GetComponent<PlayerStatusScript>();
        interaction = GameObject.FindObjectOfType<InteractionScript>();
        rigidBody = gameObject.GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private bool ItemCanPass(Collision2D collision)
    {
        bool canPass = false;
        if(collision.gameObject.tag == "Player")
        {
            canPass = true;
            if (ReqiredStatus.Length > 0)
            {
                if (!playerStatusScript.StatusRequirementMet(ReqiredStatus)){
                    canPass=false;
                }
            }

            if (requiredItemsInInventory.Length > 0)
            {
                foreach (GameObject requiredItem in requiredItemsInInventory)
                {
                    if (!myStoreScript.StoreContainsItem(requiredItem)) { canPass = false; }
                }
            }
        }

        return canPass;

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (ItemCanPass(collision))
        {
            gameObject.GetComponent<BoxCollider2D>().isTrigger=true;
        }
    }  private void OnCollisionStay2D(Collision2D collision)
    {
        if (ItemCanPass(collision))
        {
            gameObject.GetComponent<BoxCollider2D>().isTrigger=true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
      
            gameObject.GetComponent<BoxCollider2D>().isTrigger=false;
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

      
            gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
       
    }



}
