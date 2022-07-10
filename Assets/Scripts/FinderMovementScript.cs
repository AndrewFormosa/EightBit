using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinderMovementScript : MonoBehaviour
{

    public float spriteSpeed;
 public Vector3 objectsPosition;
 public Vector3 playerPosition;
    public Vector3 directionVector;
    public bool faceFrontToRest;
    private GameObject player;
   
   
    private Vector3 raycastDirection;
    public bool walkingTowards;
    public bool inspectorItemInFront;
    public bool inspectorItemToLeft;
    private ObjVar objVar;
    public Vector3[] directions = {new Vector3(1, 0, 0),new Vector3(1, -1, 0),new Vector3(0, -1, 0), new Vector3(-1, -1, 0), new Vector3(-1, 0, 0), new Vector3(-1, 1, 0), new Vector3(0, 1, 0), new Vector3(1, 1, 0) };
    public int directionInt;
    public bool InspectorPlayerInSight;

    public int objectX;
    public int objectY;
 
    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        objVar = gameObject.GetComponent<ObjVar>();
        walkingTowards = true;     
        objVar.isMoving = true;
        directionInt = 0;
        directionVector = directions[directionInt];  
    }

    public void Update()
    {

            objectsPosition =PositionAsInteger(gameObject.transform.position);          
            playerPosition = PositionAsInteger(player.transform.position);
        // directionVector = directionToPlayer();
        InspectorPlayerInSight = playerInSight(objectsPosition, directionToPlayer());
        if (objVar.isMoving==false&&InspectorPlayerInSight==false)
        {
           // StartCoroutine("MoveOneSpaceInDirection", directionVector);
           // directionVector = TurnDirection(directionVector, true, true);
            if (ObjectAtLeft(objectsPosition,directionVector))
            {
                Debug.Log("AtLeft");
                if (!ObjectAtFront(objectsPosition, directionVector))
                {
                    Debug.Log("NotAtFront");
                    StartCoroutine("MoveOneSpaceInDirection", directionVector);
                }
                else
                {
                   directionVector= TurnDirection(directionVector, true, false);
                    StartCoroutine("MoveOneSpaceInDirection", directionVector);
                }
            }
            else
          {
                directionVector=TurnDirection(directionVector, true, true);
                StartCoroutine("MoveOneSpaceInDirection", directionVector);
            }

        }else if(objVar.isMoving == false && InspectorPlayerInSight == true)
        {
            directionVector = directionToPlayer();
            StartCoroutine("MoveOneSpaceInDirection", directionVector);
        }

     
        


    }


    public bool ObjectAtLeft(Vector3 objectPos, Vector3 currentDirection)
    {
        return !AvailabeDirections(objectPos, TurnDirection(currentDirection, true, true));
    }

    public bool ObjectAtFront(Vector3 objectPos, Vector3 currentDirection)
    {
        return !AvailabeDirections(objectPos, currentDirection);
    }

    public bool ObjectAtRight(Vector3 objectPos, Vector3 currentDirection)
    {
            return !AvailabeDirections(objectPos, TurnDirection(currentDirection, true, false));
    }

    public Vector3 TurnDirection(Vector3 startingDirection, bool UpDownLeftOnly, bool antiClockwise)
    {
        int addition = 1;
        if (antiClockwise)
        {
            addition = -1;
        }

        int newDirectionInteger=0;
        for(int i = 0; i < 8; i++)
        {
            if (directions[i] == startingDirection)
            {
                newDirectionInteger = i+addition;
                if (newDirectionInteger > 7)
                {
                    newDirectionInteger = 0;
                }
                if (newDirectionInteger < 0)
                {
                    newDirectionInteger = 7;
                }

            }
        }
        if (UpDownLeftOnly)
                {
                    if (newDirectionInteger == 1 || newDirectionInteger == 3 || newDirectionInteger == 5 || newDirectionInteger == 7)
                    {
                        newDirectionInteger+=addition;
                    }                   
                }
        if (newDirectionInteger > 7)
        {
            newDirectionInteger = 0;
        }
        if (newDirectionInteger < 0)
        {
            newDirectionInteger = 7;
        }


        return directions[newDirectionInteger];

    }

    public IEnumerator MoveOneSpaceInDirection(Vector3 Direction)
    {
      
        Vector3 currentPosition = PositionAsInteger(objectsPosition);
        Vector3 desiredPosotion = PositionAsInteger(objectsPosition + Direction);
        if (AvailabeDirections(currentPosition, Direction))
        {
            int steps = (int)(1 / (spriteSpeed * Time.deltaTime * spriteSpeed));
            objVar.directionVector = Direction;
            objVar.isMoving = true;

            for (int i = 0; i < steps; i++)
            {
                currentPosition += Direction / steps;
                gameObject.transform.Translate(Direction / steps);
                yield return new WaitForFixedUpdate();
            }
            gameObject.transform.position = PositionAsInteger(desiredPosotion);
            objVar.isMoving = false;
        }
        yield return null;
    }

    public bool playerInSight(Vector3 myPosition, Vector3 playerDirection)
    {
       
        RaycastHit2D[] hits = Physics2D.RaycastAll(myPosition + playerDirection, playerDirection, 10f);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit)
            {

                if (hit.collider.gameObject.GetComponent<Rigidbody2D>() != null)
                {
                    if (hit.collider.gameObject.CompareTag("Player"))
                    {                       
                        return true;
                    }
                    else { return false; }
                }
               
            }
        }
        return false; 
    }


    public bool AvailabeDirections(Vector3 myPosition, Vector3 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(myPosition +direction, direction, 0.2f);

        if (hit)
        {

            if (hit.collider.gameObject.GetComponent<Rigidbody2D>() != null)
            {
                if (!hit.collider.gameObject.CompareTag("Player"))
                {
                    return false;
                }
                else { return true; }
            }
            else { return true; }
        }
        else
        { return true; }
    }


    public Vector3 directionToPlayer()
    {
        Vector3 dirVector = (PositionAsInteger(playerPosition) - PositionAsInteger(objectsPosition));
        //force the direction vector elements to take a -1,0 or 1 value.
        float v = 1 * Mathf.Sign(dirVector.y);
        float h = 1 * Mathf.Sign(dirVector.x);
        if (dirVector.x ==0) { h = 0;}
        if (dirVector.y ==0) { v = 0;}
        dirVector.y =(int) v; dirVector.x =(int) h;
        return dirVector;
    }
    




    public Vector3 PositionAsInteger(Vector3 position)
    {
        Vector3 fPosition = new Vector3(0, 0, 0);
        Vector3 offSet = new Vector3(0.4f, 0.4f, 0);
        if (position.x < 0) { offSet.x = -0.4f; }
        if (position.y < 0) { offSet.y = -0.4f; }
        fPosition.x = (int)((position.x + offSet.x));
        fPosition.y = (int)((position.y + offSet.y));
        return fPosition;
    }

}
