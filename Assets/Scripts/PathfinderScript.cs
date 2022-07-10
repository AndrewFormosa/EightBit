using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfinderScript : MonoBehaviour
{
    public List<Node> openList;
    public List<Node> closedList;
    public List<Vector3> pathList;
    public Vector3 startPosition;
    public Vector3 endPosition;
    public int spriteSpeed;
    public int maxPathCount;
    public Vector3 currentRegionVector;

    private GameObject player;
    private ObjVar objVar;
 
    public Vector3 DebugLowestNodePos;
    public bool pathFound;
    public bool ableToMove;

    // Start is called before the first frame update
    void Start()
    {
        ableToMove = true;
        pathFound = false;
        objVar = gameObject.GetComponent<ObjVar>();
        objVar.directionVector = new Vector3(1, 0, 0);
        player = GameObject.FindGameObjectWithTag("Player");   
        
        objVar.isMoving = false;
        //SetRegion
        Vector3 region = SpaceManager.RegionVector(gameObject.transform.position);
        string key = SpaceManager.RegionToKey(region);
        if (SpaceManager.LoadedRegions.ContainsKey(key))
        {
            gameObject.transform.SetParent(SpaceManager.LoadedRegions[SpaceManager.RegionToKey(region)].transform);
        }
        else
        { //ableToMove = false;
            gameObject.transform.SetParent(GameObject.FindObjectOfType<PerminentRegionScript>().gameObject.transform);
        }


      //  gameObject.transform.SetParent(SpaceManager.LoadedRegions[SpaceManager.RegionToKey(region)].transform);
        //gameObject.transform.SetParent(GameObject.FindObjectOfType<PerminentRegionScript>().gameObject.transform);
    }

    // Update is called once per frame
    void Update()
    {

        //check the new region that the item is now in. if it has moved to a different region then if that region exists then move it there if not then stop the item from moving.  
        if (gameObject.GetComponentInParent<RegionScript>())
        {
            Vector3 currentRegionVector = gameObject.GetComponentInParent<RegionScript>().regionVector;
        }
        else
        {
            Vector3 currentRegionVector = new Vector3(float.MaxValue, float.MaxValue, 0);
        }
        Vector3 newRegion = SpaceManager.RegionVector(gameObject.transform.position);

        if (newRegion != currentRegionVector)
        {
            string key = SpaceManager.RegionToKey(newRegion);
            if (SpaceManager.LoadedRegions.ContainsKey(key))
            {
                gameObject.transform.SetParent(SpaceManager.LoadedRegions[SpaceManager.RegionToKey(newRegion)].transform);
            }
            else
            {
                gameObject.transform.SetParent(GameObject.FindObjectOfType<PerminentRegionScript>().gameObject.transform);
            }
        }                  
        
        startPosition = (PositionAsInteger(gameObject.transform.position));
        endPosition = (PositionAsInteger(player.transform.position));
        
        if (Vector3.Magnitude(startPosition - endPosition) > 1&&objVar.isMoving==false)
        {
            Debug.Log("Try and move enemy");
                InstantiatePathFinder();
                FindPath();
                StartCoroutine("MoveThroughPath");       
        }
        if (Vector3.Magnitude(startPosition - endPosition) < 2 &&objVar.isMoving==false)
        {           
 //CREATE THE IDEAL DIRECTION VECTOR FOR MOVING TOWARDS THE PLAYER
        //compare this position with the players position to create the actual direction vector.
        Vector3 myPosition = player.transform.position;
        Vector3 directionVector = (myPosition - gameObject.transform.position);
        //force the direction vector elements to take a -1,0 or 1 value.
        float v = 1 * Mathf.Sign(directionVector.y);
        float h = 1 * Mathf.Sign(directionVector.x);
        if (directionVector.x > -0.1 && directionVector.x < 0.1) { h = 0; gameObject.transform.position = new Vector3(player.transform.position.x, gameObject.transform.position.y, 0); }
        if (directionVector.y > -0.1 && directionVector.y < 0.1) { v = 0; gameObject.transform.position = new Vector3(gameObject.transform.position.x, player.transform.position.y, 0); }
        directionVector.y = v; directionVector.x = h;
            objVar.directionVector = directionVector;
            //set the moveVecor and move the object.
            Vector3 moveVector = directionVector * spriteSpeed * Time.deltaTime;
            transform.Translate(moveVector);

        }

        //currentRegionVector = SpaceManager.RegionVector(gameObject.transform.position);
       
    }



    public IEnumerator MoveThroughPath()
    {
      
         ableToMove = true;
        foreach (Vector3 pathPosition in pathList)
        {
            Vector3 currentPosition = PositionAsInteger(gameObject.transform.position);
            Vector3 desiredPosition = PositionAsInteger(pathPosition);
            Vector3 direction = desiredPosition - currentPosition;
         

            if (RididbodyAtPosition(desiredPosition))
            {
                ableToMove = false;
            }
      

            if (ableToMove)
            {
                int steps = (int)(1 / (spriteSpeed * Time.deltaTime));
                objVar.directionVector = direction;
                objVar.isMoving = true;
                for (int i = 0; i < steps; i++)
                {
                    // currentPosition += direction / steps;
                    gameObject.transform.Translate(direction / steps);
                    yield return new WaitForFixedUpdate();
                }

                gameObject.transform.position = PositionAsInteger(desiredPosition);
                startPosition = (PositionAsInteger(gameObject.transform.position));
                endPosition = (PositionAsInteger(player.transform.position));
            }
        }
        objVar.isMoving = false;   
        yield return null;
             
      }

    public bool RegionIsAvailableForPosition(Vector3 position)
    {
        string key = SpaceManager.RegionToKey(SpaceManager.RegionVector(position));
        if (SpaceManager.LoadedRegions.ContainsKey(key))
        {
            return true;

        }
        else
        {
            return false;
        }
    }

    public void CreatePathList()
    {
        Debug.Log("createList");
        pathList = new List<Vector3>();
        Vector3 lastPosition = closedList[(closedList.Count - 1)].nodePosition;
        Vector3 startPosition = closedList[0].nodePosition;
        int whileCount = 0;
        while (lastPosition != startPosition)
        {
            pathList.Add(lastPosition);
            Vector3 parentPosition = NodeFromClosedListAtPosition(lastPosition).nodeParent;
            lastPosition = parentPosition;
            whileCount++;
            if (whileCount > 100)
            {
                Debug.Log("Exit While Loop"); break;
            }
        }

        //reversePathList
        pathList.Reverse();    
        //clearLists;
        openList.Clear();
        closedList.Clear();
        

    }

    public Node NodeFromClosedListAtPosition(Vector3 nodePostition)
    {
        foreach(Node node in closedList)
        {
            if (node.nodePosition == nodePostition) { return node; }
        }
        return null;
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

    public void FindPath()
    {
        pathFound = true;
        int count = 0;
        Node nextNode = SelectBestNodeFromOpenList();
        if (nextNode != null)
        {
            while (nextNode.hValue > 0)
            {
                DebugLowestNodePos = nextNode.nodePosition;
                CreateSurroundingNodes(nextNode);
                nextNode = SelectBestNodeFromOpenList();
                if (nextNode == null) { Debug.Log("cant find path"); pathFound = false; break; }
                closedList.Add(nextNode); openList.Remove(nextNode);
                count++;
                if (count > maxPathCount) { Debug.Log("cant find path"); pathFound = false; break; }
            }
        }
        if (pathFound) { Debug.Log("PathFound"); }
      CreatePathList();          
    }

    public void InstantiatePathFinder()
    {
        openList.Clear();
        closedList.Clear();
        startPosition = (PositionAsInteger(gameObject.transform.position));
        endPosition = (PositionAsInteger(player.transform.position));
       
      if (startPosition != endPosition)
        {
            Node current = new Node();
            current.nodePosition = startPosition;
            current.nodeParent = startPosition;
            current.gValue = 0;
            current.hValue = (int)(10f * Vector3.Magnitude(startPosition - endPosition));
            current.fValue = current.gValue + current.hValue;
            closedList.Add(current);
            CreateSurroundingNodes(current);
        }
    }


    public void CreateSurroundingNodes(Node currentNode)
    {
        //openList.Remove(currentNode);
        //closedList.Add(currentNode);
        //Vector3[] SurroundingNodeOffSets = {new Vector3(1, 0, 0), new Vector3(1, -1, 0), new Vector3(0, -1, 0), new Vector3(-1, -1, 0), new Vector3(-1, 0, 0), new Vector3(-1, 1, 0), new Vector3(0, 1, 0), new Vector3(1, 1, 0) };
        Vector3[] SurroundingNodeOffSets = { new Vector3(1, 0, 0), new Vector3(0, -1, 0), new Vector3(-1, 0, 0), new Vector3(0, 1, 0) };
        foreach(Vector3 surroundingNodeOffset in SurroundingNodeOffSets)
        {
            Node newNode = new Node();
            newNode.nodePosition = PositionAsInteger(currentNode.nodePosition) + surroundingNodeOffset;
            newNode.gValue = currentNode.gValue+10;
            //newNode.gValue = gValue(newNode);
            newNode.hValue = hValue(newNode);
            newNode.fValue = newNode.gValue + newNode.hValue;
            newNode.nodeParent = currentNode.nodePosition;

            if (!RididbodyAtPosition(newNode.nodePosition) && !ClosedListContainsNodePosition(newNode.nodePosition))
            {
                if (OpenListContainsNodePosition(newNode.nodePosition))
                {
                    openList.Remove(NodeFromOpenList(newNode.nodePosition));
                }

                openList.Add(newNode);
            }
        }
      
    }

    public bool ClosedListContainsNodePosition(Vector3 nodePosition)
    {
        foreach(Node node in closedList)
        {
            if (node.nodePosition == nodePosition) { return true; }
        }
        return false;
    }

    public bool OpenListContainsNodePosition(Vector3 nodePosition)
    {
        foreach(Node node in closedList)
        {
            if (node.nodePosition == nodePosition) { return true; }
        }
        return false;
    }

    public Node NodeFromOpenList(Vector3 nodePosition)
    {
        foreach (Node node in closedList)
        {
            if (node.nodePosition == nodePosition) { return node; }
        }
        return null;
    }

    public Node SelectBestNodeFromOpenList()
    {
        int lowestFValue =int.MaxValue;
        int lowestHValue = int.MaxValue;
        Node lowestNode = null;
        foreach (Node node in openList)
        {
           //  if(node.fValue==lowestFValue&&node.hValue<lowestHValue)
           // {
           //     lowestNode = node; lowestFValue = node.fValue; lowestHValue = node.hValue;
           // }

            if (node.fValue < lowestFValue)
            {
                lowestNode = node;lowestFValue = node.fValue;lowestHValue = node.hValue;
            }
           
        }
        return lowestNode;

    }

    public int gValue(Node currentNode)
    {
        return (int)(10f*(Vector3.Magnitude(startPosition - currentNode.nodePosition)));
    }

    public int hValue(Node currentNode)
    {
        return (int)(10f*(Vector3.Magnitude(endPosition - currentNode.nodePosition)));
    }

    public bool RididbodyAtPosition(Vector3 nodePosition)
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector3(nodePosition.x-0.3f,nodePosition.y-0.3f,0), new Vector3(1, 1, 0), 0.6f);

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

}




[System.Serializable]
public class Node
{
   public Vector3 nodePosition;
   public Vector3 nodeParent;
   public int gValue;
    public int hValue;
    public int fValue;
}

