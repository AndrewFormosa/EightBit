using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmSpawner : MonoBehaviour
{
    public GameObject swarmPrefab;
   public int spawnQty;
  public  float spawnDelay;
    

    private SpaceManager spaceManager;
    

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<SpaceManager>();
        SpaceManager.NewRegionCreated += OnNewRegionCreation;


       // StartCoroutine("SpawnSwarm");
    }

    // Update is called once per frame
    void Update()
    {

        //debug test

        if (Input.GetKeyDown(KeyCode.H))
        {
            StartCoroutine("SpawnSwarm");
        }

    }


   public void OnNewRegionCreation(Vector3 regionVector)
        {

         
            int distanceFromOrigin = (int)(Vector3.Magnitude(regionVector)/10);
                Debug.Log("RegionCreated:" + regionVector+" dist"+distanceFromOrigin);

            for(int i = 0; i < distanceFromOrigin;i++)
             {
            //    StartCoroutine("SpawnSwarm");
            }
         
        }


    public IEnumerator SpawnSwarm()
    {
        for (int i = 0; i < spawnQty; i++)
        {
            //   Instantiate(swarmPrefab,new Vector3(-20,20,0),Quaternion.identity); 
            Instantiate(swarmPrefab,this.transform.position,Quaternion.identity);
         
         yield return new WaitForSeconds(spawnDelay);

        }

       
    }



}
