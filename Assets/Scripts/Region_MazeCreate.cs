using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Region_MazeCreate : MonoBehaviour
{
   // public enum Directions { up,down,left,right};

       //private List<Vector2> positionStack = new List<Vector2>();
       private  List<Vector2> positionVisited = new List<Vector2>();
    private int width;
    private int height;
  
  
    public GameObject mazeWallPrefab;
    public GameObject mazeDoorPrefab;
    [Header("Element0: wall, Element1:Door, Element2: corridor, Element3: Single placed Item")]
    public GameObject[] mazeObjects;

    public int[][] tileMatrix;

   


    public void CreateRegion(int regionWidth, int regionHeight, GameObject region)
    {
        width = regionWidth;
        height = regionHeight;
        tileMatrix = new int [regionWidth][];
        for (int i = 0; i < regionWidth;i++)
        {
            tileMatrix[i] = new int[regionHeight];
           
        }
        for(int i = 0; i < regionHeight;i++)
        {
            tileMatrix[0][i] = 2;
        }
        for (int i = 0; i < regionWidth;i++)
        {
            tileMatrix[i][0] = 2;
        }

        //tileMatrix[0][0]= 1;

        //selectRandomStartPosition
        int startX=0;
        int startY=0;
        int startSide = Random.Range(0, 4);
        if (startSide == 0) { startX = Random.Range(2, regionWidth-1);startY = 1; }
        if (startSide == 1) { startY = Random.Range(2, regionHeight-1); startX = regionWidth-1; }
        if (startSide == 2) { startX = Random.Range(2, regionWidth-1); startY = regionHeight-1; }
        if (startSide == 3) { startY = Random.Range(2, regionHeight-1); startX = 1; }
        
        tileMatrix[startX][startY] = 1; // set start position with object prefab 1.

        //createMaze

        //identify potential directions.
        List<Vector2> positionStack = new List<Vector2>();
        bool placedNo3 = false;
        Vector2 tilePosition = new Vector2(startX, startY);
        positionStack.Add(tilePosition);
        while (positionStack.Count > 0)
        { 
        List<Vector2> potentailNextDirection = DirectionsAvailable(tilePosition);
       
            if (potentailNextDirection.Count > 0)//if it is possible to move in any direction then
            {
                Vector2 nextDirection = potentailNextDirection[Random.Range(0, (int)potentailNextDirection.Count)];
                //setWallTileTo2 and set next space to 2
                tilePosition += nextDirection;
                Debug.Log("tile position:" + tilePosition + " nextDirection:" + nextDirection);
                tileMatrix[(int)(tilePosition.x)][(int)tilePosition.y] = 2;
                tilePosition += nextDirection;
                tileMatrix[(int)(tilePosition.x)][(int)tilePosition.y] = 2;
                //Add this position to the positionStack
                positionStack.Add(tilePosition);
            }
            else //if it is not possible to move in any direction then move to previous position on the stack and try again.
            {
                if (positionStack.Count > 0)
                {
                    if (!placedNo3)
                    {
                        tileMatrix[(int)tilePosition.x][(int)tilePosition.y] = 3;placedNo3 = true;
                    }
                    tilePosition = positionStack[positionStack.Count - 1];
                    positionStack.Remove(tilePosition);
                }
            }
        }

        //Instantiate objects at positions within region
        for (int x = 0; x < regionWidth; x++)
        {
            for (int y = 0; y < regionHeight; y++)
            {
                float posX = x - (regionWidth / 2f);
                float posY = y - (regionHeight / 2f);

                GameObject nextItem = mazeObjects[tileMatrix[x][y]];
                if (nextItem != null)
                {
                    GameObject thing = Instantiate(nextItem, region.transform, false);
                    thing.transform.localPosition = new Vector3(posX, posY, 0);

                }
            }
        }

    
    }

    public List<Vector2> DirectionsAvailable (Vector2 position)
    {

        List<Vector2> directions = new List<Vector2>();
        Vector2[] dir= { new Vector2(1, 0), new Vector2(0, -1), new Vector2(-1, 0), new Vector2(0, 1) };
        Vector2[] offSets = { new Vector2(2, 0), new Vector2(0, -2), new Vector2(-2, 0), new Vector2(0, 2) };
       for(int i=0;i<4;i++)
        {
            Vector2 TestPosition = position + offSets[i];
            if (TestPosition.x > 1 && TestPosition.x < (width - 1) && TestPosition.y > 1 && TestPosition.y < (height - 1))
            {
                if (tileMatrix[(int)TestPosition.x][(int)TestPosition.y] == 0) { directions.Add(dir[i]); }
            }
        }

        return directions;
    }



}
