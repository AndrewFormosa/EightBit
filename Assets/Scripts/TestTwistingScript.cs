using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTwistingScript : MonoBehaviour
{
    public TerrainObjects[] terrainObjects;
    private int x;
    private int y;
    public int size;
    public int totalTiles;
    private float totalChance;
    private int direction;
    private float steps;
    public int seed;
    public string[][] terrainIndex;
    private List<GameObject> prefabs;


    // Start is called before the first frame update
    void Start()
    {
        InstantiateVariables();
        SetAccChance();
        CreateTerrain();
        CreateNextTerrain();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

            foreach(GameObject prefab in prefabs)
            {
                Destroy(prefab);
            }
            CreateNextTerrain();
            seed++;
            InstantiateVariables();
            SetAccChance();
            CreateTerrain();

        }
    }

    public void InstantiateVariables()
    {
        prefabs = new List<GameObject>();
        terrainIndex = new string[size*3][];
        for (int i = 0; i < size; i++)
        {
            terrainIndex[i] = new string[size*3];
        }


        totalTiles = size * size;
        x = 0;
        y = 0;
        steps = 1;
        direction = 1;

        totalChance = 0f;
        Random.InitState(seed);
        SetAccChance();
        CreateTerrain();


    }

    public void CreateNextTerrain()
    {
        Debug.Log("NEXT ONE");
        for (int x = 24; x <75; x++)
           {
            for (int y = -25; y <25; y++)
            {

                Debug.Log("TRY"+x+","+y);
                float randomNumber = Random.Range(0f, totalChance);

                GameObject chosenTerrain = terrainObjects[0].terrainObjectPrefab;
                foreach (TerrainObjects terrainObject in terrainObjects)
                {
                    if (randomNumber > terrainObject.accumilativeChance)
                    {

                        chosenTerrain = terrainObject.terrainObjectPrefab;
                    }

                    if (Random.Range(0f, 1f) < nextToIndex(terrainObject, (int)x + (int)size / 2, (int)y + (int)size / 2)) { chosenTerrain = terrainObject.terrainObjectPrefab; randomNumber = 0f; }
                }

                GameObject newPrefab = Instantiate(chosenTerrain, new Vector3(x, y, 0), Quaternion.identity);
                prefabs.Add(newPrefab);
              //  terrainIndex[x + (int)size / 2][y + (int)size / 2] = chosenTerrain.name;

              
            }
        }

    }


    // Update is called once per frame
    public void CreateTerrain()
    {
        int totalSteps = 0;
        for (int i = 0; i < totalTiles; i++)
        {
          
            float randomNumber = Random.Range(0f, totalChance);
         
            GameObject chosenTerrain = terrainObjects[0].terrainObjectPrefab;
            foreach(TerrainObjects terrainObject in terrainObjects)
            {
                if (randomNumber> terrainObject.accumilativeChance)
                {
                  
                    chosenTerrain = terrainObject.terrainObjectPrefab;             
                }
                
                    if (Random.Range(0f, 1f) < nextToIndex(terrainObject, (int)x + (int)size / 2, (int)y + (int)size / 2)) { chosenTerrain = terrainObject.terrainObjectPrefab; randomNumber = 0f; } }
            

           


           GameObject newPrefab= Instantiate(chosenTerrain, new Vector3(x, y, 0), Quaternion.identity);
            prefabs.Add(newPrefab);
            terrainIndex[x+(int)size/2][y+(int)size/2] = chosenTerrain.name;

            totalSteps ++;
            if (totalSteps > (int)steps) { totalSteps = 1; ChangeDirection(); }
            if (direction == 1) { x++; }
            if (direction == 2) { y--; }
            if (direction == 3) { x--; }
            if (direction == 4) { y++; }
            
            
        }
     
    }

    public float nextToIndex(TerrainObjects newObject, int x, int y)
    {
        float answer = 0f;
        string name = newObject.terrainObjectPrefab.name;
        int[] xAdd = { 1, 1, 0, -1, -1, -1, 0, -1 };
        int[] yAdd = { 0, -1, -1, -1, 0, 1, 1, 1};

        for(int j = 0; j < 8;j++)
        {
            if (terrainIndex[x + xAdd[j]][y + yAdd[j]] == name)
            {
                answer+=newObject.degreeOfRepetition;
            }
        }

        return answer;
    }

    public void ChangeDirection()
    {
      
        direction++;
         
        if (direction > 4) { direction = 1; }
        steps += 0.5f;
       
    }

    public void SetAccChance()
    {
        totalChance = 0;
        foreach(TerrainObjects terrainObject in terrainObjects)
        {
           
            terrainObject.accumilativeChance = totalChance;    
             totalChance += terrainObject.chanceOfSpawn;
        }
    }

}

[System.Serializable]
public class TerrainObjects
{
    public GameObject terrainObjectPrefab;
    public float chanceOfSpawn;
    public float degreeOfRepetition;
    public float accumilativeChance;
}
