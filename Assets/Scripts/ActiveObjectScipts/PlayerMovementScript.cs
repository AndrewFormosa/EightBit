using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
   
    public float spriteSpeed;
    private Vector3 directionVector;
    public bool faceFrontToRest;
    public bool restOnLeftAndRightOnly;

    private ObjVar objVar;

    // Start is called before the first frame update
    void Start()
    {
        objVar = this.GetComponent<ObjVar>();
        directionVector = new Vector3 (1,1,0);
    }


   
    // Update is called once per frame
    void Update()
    {
        //check for change in input
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");
      
            if ((directionVector.x != v || directionVector.y != h)&&faceFrontToRest)
            {       
                directionVector.y = v;
                directionVector.x = h;
                objVar.isMoving = true;
                objVar.directionVector = directionVector;
            }
      
            else if(!faceFrontToRest && (v != 0 || h != 0))
        {
            directionVector.y = v;
            directionVector.x = h;
            objVar.isMoving = true;
            objVar.directionVector = directionVector;
        }
        else { objVar.isMoving = false; if (restOnLeftAndRightOnly) { TurnLefOrRight(); } }






        //move player if required
        // if (v != 0 || h != 0 && !RididbodyInDirection(objVar.directionVector))
        if (v != 0 || h != 0)
        {
              Vector3 moveVector = directionVector * spriteSpeed * Time.fixedUnscaledDeltaTime;
              transform.Translate(moveVector);       
        }

    }

   public void TurnLefOrRight()
    {
        //if direction is diagonal then force direction to Left or Right
        if (Vector3.Magnitude(directionVector) > 1)
        {
            directionVector = new Vector3(directionVector.x, 0, 0);
            objVar.directionVector = directionVector;
        }
    }


    public bool RididbodyInDirection(Vector3 direction)
    {
        Vector3 targetPosition = new Vector3();
        targetPosition = gameObject.transform.position + direction;

        RaycastHit2D hit = Physics2D.Raycast(new Vector3(targetPosition.x - 0.3f, targetPosition.y - 0.3f, 0), direction, 0.6f);

        if (hit)
        {

            if (hit.collider.gameObject.GetComponent<Rigidbody2D>() != null)
            {

                return true;
            }
            else { return false; }
         
        }
        else
        { return false; }
    }


}
