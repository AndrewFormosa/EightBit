using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementScript : MonoBehaviour
{
    public float spriteSpeed;
    private Vector3 directionVector;
    public bool faceFrontToRest;
    private GameObject player;
    private Vector3 myPosition;

    private Vector3 raycastDirection;
    public bool walkingTowards;
    public bool inspectorItemInFront;
    public bool inspectorItemToLeft;
    private int walkAwayCount;
    private int walkAwayDistance;

    private ObjVar objVar;


    // Start is called before the first frame update
    void Start()
    {
    
        player = GameObject.FindGameObjectWithTag("Player");
        objVar = gameObject.GetComponent<ObjVar>();
        directionVector = new Vector3(1, 1, 0);
        raycastDirection = new Vector3(1, 1, 0);
        walkingTowards = true;
        StartCoroutine("CheckIfStuck");
        objVar.isMoving = true;
    }



    // Update is called once per frame
    void Update()
    {
        if (walkingTowards)
        {
            //CREATE THE IDEAL DIRECTION VECTOR FOR MOVING TOWARDS THE PLAYER
            //compare this position with the players position to create the actual direction vector.
            myPosition = player.transform.position;

            directionVector = (myPosition - gameObject.transform.position);
            //force the direction vector elements to take a -1,0 or 1 value.
            float v = 1 * Mathf.Sign(directionVector.y);
            float h = 1 * Mathf.Sign(directionVector.x);
            if (directionVector.x > -0.1 && directionVector.x < 0.1) { h = 0; gameObject.transform.position = new Vector3(player.transform.position.x, gameObject.transform.position.y, 0); }
            if (directionVector.y > -0.1 && directionVector.y < 0.1) { v = 0; gameObject.transform.position = new Vector3(gameObject.transform.position.x, player.transform.position.y, 0); }
            directionVector.y = v; directionVector.x = h;
            //if (!objVar.isMoving) { walkingTowards = false; SetNewDirection(); walkAwayCount = 0; }
            if (!objVar.isMoving)
             {
                StartCoroutine("WalkAround");

             }

          

        }


       

        if (Input.GetKeyDown(KeyCode.U))
        {
            walkingTowards = false;
            TurnClockwise();
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            walkingTowards = false;
            TurnAntiClockwise();
        }


     


      //  else
     //   {
       //     Debug.Log("walkAway"+walkAwayCount);
         //   if (walkAwayCount < 50)
         //   {
         //       
          //      walkAwayCount++;
       //         Debug.Log("IsMoving" + objVar.isMoving + " count" + walkAwayCount);
        //        if (objVar.isMoving==false && walkAwayCount>10) { Debug.Log("NEW DIRECTION"); SetNewDirection();walkAwayCount = 0; }
        //    }
        //   else
         //   {
         //       walkingTowards = true;
         //   }      
      // }
        inspectorItemInFront = ItemInfront();

    



        //set the OBJVAR diretion vector for animation purposes
      //  objVar.isMoving = true;
        objVar.directionVector = directionVector;
        //set the moveVecor and move the object.
        Vector3 moveVector = directionVector * spriteSpeed * Time.deltaTime;
        transform.Translate(moveVector);

      
    }


   public IEnumerator WalkAround()
        {
        walkingTowards = false;
        Debug.Log("FIRST TURN");
        TurnClockwise();
        yield return new WaitForSeconds(0.25f);
        while (objVar.isMoving==false)
        {
            Debug.Log("TURN");
            TurnClockwise();
            yield return new WaitForSeconds(0.25f);

        }     
       
        yield return new WaitForSeconds(0.5f);
        TurnAntiClockwise(); TurnAntiClockwise();
        yield return new WaitForSeconds(0.25f);
        if (ItemInfront())
        {
            Debug.Log("ITEM STILL IN FRONT");
           StartCoroutine("WalkAround");
        }
        else
        {
            Debug.Log("Item GONE");
            walkingTowards = true;
        }
     
        }


    public IEnumerator SetNewDirection()
    {
        walkingTowards = false;

        //  Vector3 newVector=new Vector3 (0,0,0);
        //   if (directionVector.x == -1&&directionVector.y==0) { newVector = new Vector3(-1, 1, 0); }
        //   if (directionVector.x == -1 && directionVector.y == 1) { newVector = new Vector3(0, 1, 0); }
        //   if (directionVector.x == 0&&directionVector.y==1) { newVector = new Vector3(1, 1, 0); }
        //   if (directionVector.x == 1&&directionVector.y==1) { newVector = new Vector3(1, 0, 0); }
        //   if (directionVector.x == 1&&directionVector.y==0) { newVector = new Vector3(1, -1, 0); }
        //   if (directionVector.x == 1&&directionVector.y==-1) { newVector = new Vector3(0, -1, 0); }
        //   if (directionVector.x == 0&&directionVector.y==-1) { newVector = new Vector3(-1, -1, 0); }
        //   if (directionVector.x == -1&&directionVector.y==-1) { newVector = new Vector3(-1, 0, 0); }    
        //   directionVector = newVector;
        //   objVar.directionVector = directionVector;
        while (objVar.isMoving == false)
        {
            TurnAntiClockwise();
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(2f);
        directionVector = new Vector3((int)Random.Range(-1, 2), (int)Random.Range(-1, 2), 0);
        yield return new WaitForSeconds(2f);

        walkingTowards = true;

    }


    public void TurnClockwise()
    {
        Vector3 newVector = new Vector3(0, 0, 0);
           if (directionVector.x == -1&&directionVector.y==0) { newVector = new Vector3(-1, 1, 0); }
           if (directionVector.x == -1 && directionVector.y == 1) { newVector = new Vector3(0, 1, 0); }
           if (directionVector.x == 0&&directionVector.y==1) { newVector = new Vector3(1, 1, 0); }
           if (directionVector.x == 1&&directionVector.y==1) { newVector = new Vector3(1, 0, 0); }
           if (directionVector.x == 1&&directionVector.y==0) { newVector = new Vector3(1, -1, 0); }
           if (directionVector.x == 1&&directionVector.y==-1) { newVector = new Vector3(0, -1, 0); }
           if (directionVector.x == 0&&directionVector.y==-1) { newVector = new Vector3(-1, -1, 0); }
           if (directionVector.x == -1&&directionVector.y==-1) { newVector = new Vector3(-1, 0, 0); }    
           directionVector = newVector;
           objVar.directionVector = directionVector;
    }

    public void TurnAntiClockwise()
    {
        Vector3 newVector = new Vector3(0, 0, 0);
        if (directionVector.x == -1 && directionVector.y == 0) { newVector = new Vector3(-1, -1, 0); }
        if (directionVector.x == -1 && directionVector.y == 1) { newVector = new Vector3(-1,0, 0); }
        if (directionVector.x == 0 && directionVector.y == 1) { newVector = new Vector3(-1, 1, 0); }
        if (directionVector.x == 1 && directionVector.y == 1) { newVector = new Vector3(0, 1, 0); }
        if (directionVector.x == 1 && directionVector.y == 0) { newVector = new Vector3(1, 1, 0); }
        if (directionVector.x == 1 && directionVector.y == -1) { newVector = new Vector3(1, 0, 0); }
        if (directionVector.x == 0 && directionVector.y == -1) { newVector = new Vector3(1, -1, 0); }
        if (directionVector.x == -1 && directionVector.y == -1) { newVector = new Vector3(0, -1, 0); }
        directionVector = newVector;
        objVar.directionVector = directionVector;
    }




    public bool ItemInfront()
    {       
        RaycastHit2D hit = Physics2D.Raycast(transform.position + objVar.directionVector, objVar.directionVector, 0.1f);
       
        if (hit)
        {
           
            if (hit.collider.gameObject.GetComponent<Rigidbody2D>() != null)
            {
                if (!hit.collider.gameObject.CompareTag("Player"))
                {
                    return true;
                }
                else { return false; }
            }
            else { return false; }
        }
        else
        { return false; }
    }

    public bool ItemAtLeft()
    {       
        RaycastHit2D hit = Physics2D.Raycast(transform.position + objVar.directionVector, objVar.directionVector, 0.1f);
       // RaycastHit2D hit = Physics2D.Raycast()
        if (hit)
        {
            return true;
        }
        else
        { return false; }
    }

    public IEnumerator CheckIfStuck()
    {
        while (true)
        {
            Vector3 oldPos = gameObject.transform.position;
            yield return new WaitForSeconds(0.25f);
            Vector3 newPos = gameObject.transform.position;
            float sizeInChange= Vector3.Magnitude(newPos - oldPos);
            if (sizeInChange < 0.2f) { objVar.isMoving = false; } else { objVar.isMoving = true; }

        }
    }
}
