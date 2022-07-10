using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Region_Maze2Create : MonoBehaviour
{
    //private List<Vector2> positionStack = new List<Vector2>();
    private List<Vector2> positionVisited = new List<Vector2>();
    private int width;
    private int height;


    public GameObject mazeWallPrefab;
    public GameObject mazeDoorPrefab;
    [Header("Element0: wall, Element1:Door, Element2: corridor, Element3: Single placed Item")]
    public GameObject[] mazeObjects;
    public int enemyQty;
    public int[][] tileMatrix;




    public void CreateRegion(int regionWidth, int regionHeight, GameObject region)
    {

        width = regionWidth;
        height = regionHeight;
        //CreateMatrix
        tileMatrix = new int[regionWidth][];     
        for (int i = 0; i < regionWidth; i++)
        {
            tileMatrix[i] = new int[regionHeight];
        }

        //create outside spaces
        for (int i = 0; i < regionHeight; i++)
        {
            tileMatrix[0][i] = 2;
        }
        for (int i = 0; i < regionWidth; i++)
        {
            tileMatrix[i][0] = 2;
        }


        //selectRandomStartPosition
        int startX = 0;
        int startY = 0;
        int startSide = Random.Range(0, 4);
        if (startSide == 0) { startX = Random.Range(2, regionWidth - 1); startY = 1; }
        if (startSide == 1) { startY = Random.Range(2, regionHeight - 1); startX = regionWidth - 1; }
        if (startSide == 2) { startX = Random.Range(2, regionWidth - 1); startY = regionHeight - 1; }
        if (startSide == 3) { startY = Random.Range(2, regionHeight - 1); startX = 1; }

        tileMatrix[startX][startY] = 1; // set start position with object prefab 1 ie Door.

        //createMaze
        
        for(int i = 2; i < width - 1; i++)
        {
            int count = 0;
            for(int j = 2; j < height - 1; j++)
            {
                int RandNo = Random.Range(0, 10);
                if (RandNo < 8&&count==0)
                {
                    tileMatrix[i][j] = 2;
                   
                }
                if (RandNo < 6&&count==1)
                {
                    tileMatrix[i][j] = 2;
                }
                if (count == 2)
                {
                    tileMatrix[i][j] = 2;
                }
                count++;
                {
                    if (count > 2) { count = 0; }
                }
                if (i == 2||i==width-2||j==2||j==height-2) { tileMatrix[i][j] = 2; }
                
            }      
        }

        for (int i = 0; i < enemyQty; i++)
        {
            tileMatrix[2][i + 2] = 3;
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

   
}
