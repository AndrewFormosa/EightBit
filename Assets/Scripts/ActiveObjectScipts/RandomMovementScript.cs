using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class RandomMovementScript : MonoBehaviour
{
 

    public float minimumSpeed;
    public float maximumSpeed;
    public int minDistance;
    public int maxDisitance;
    public bool keepObjectWithinABoundry;
    public Vector3 boundryCenter;
    public bool useSpaceManagerScriptForBoundryHeightAndWidth;
    public float boundryRegionHeight;
    public float boundryRegionWidth;
    public bool moveTowardsPlayer;
    public int agressionOutOf10;
    private bool undoMove;

    private ObjVar objVar;

    private Vector3 randomDirection;
    private Vector3 randomMovement;
    private int rndCount;
    private int count;
    private float speed;
    private bool hitObject;

    private Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        undoMove = false;
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        objVar = this.GetComponent<ObjVar>();

        ResetMove();
        if (useSpaceManagerScriptForBoundryHeightAndWidth)
        {
            SetUpBoundriesUsingSpaceManager();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentRegionVector = SpaceManager.RegionVector(gameObject.transform.position);

        objVar.directionVector = randomDirection;
        if (count < rndCount)
        {
            if (keepObjectWithinABoundry)
            {
                if (SpaceManager.insideBoundry(boundryCenter, (this.transform.localPosition + randomMovement)))
                {
                    DoMove();
                }
            }
            else
            {
                DoMove();
            }
            count++;
        }
        else { ResetMove(); }

        if (hitObject)
        {
            //UnDoMove();
            undoMove = true;
            hitObject = false;
        }


        //check if item has moved into a new region - If this region is loaded then set that region as items parent if not loaded then undo the move back into the old region.
        Vector3 newRegion = SpaceManager.RegionVector(gameObject.transform.position);

        if (newRegion != currentRegionVector)
        {
            string key = SpaceManager.RegionToKey(newRegion);
            if (SpaceManager.LoadedRegions.ContainsKey(key)){
               // keepObjectWithinABoundry = false;
                gameObject.transform.SetParent(SpaceManager.LoadedRegions[SpaceManager.RegionToKey(newRegion)].transform);
            }
            else
            {
                //UnDoMove();
                undoMove = true;
            }
        }

        if (undoMove) { UnDoMove(); undoMove = false; }

    }
  


    public void UnDoMove()
    {
        this.transform.Translate(-randomMovement);
        ResetMove();

    }



    public void DoMove()
    {
        this.transform.Translate(randomMovement);
    }
  
    public void ResetMove()
    {
        Vector3 playerDirection = new Vector3(playerTransform.position.x - transform.position.x, playerTransform.position.y - transform.position.y, 0);
        playerDirection.Normalize();
        count = 0;
        randomDirection = new Vector3(UnityEngine.Random.Range(-1, 2), UnityEngine.Random.Range(-1, 2), 0);
        if (UnityEngine.Random.Range(0, 10) < agressionOutOf10 && moveTowardsPlayer) { randomDirection = playerDirection; }
        speed = UnityEngine.Random.Range(minimumSpeed, maximumSpeed);
        rndCount = UnityEngine.Random.Range(minDistance, maxDisitance);
        randomMovement = randomDirection * Time.deltaTime * speed;
    }

    public bool insideBoundry(Vector3 regionCenter, Vector3 objectPosition)
    {
        float pX = objectPosition.x;
        float pY = objectPosition.y;
        float tb = regionCenter.y + (boundryRegionHeight / 2f);
        float bb = regionCenter.y - (boundryRegionHeight/2f);
        float rb = regionCenter.x + (boundryRegionWidth / 2f);
        float lb = regionCenter.x - (boundryRegionWidth / 2f);
        if (pX > lb && pX < rb && pY < tb && pY > bb) { return true; } else { return false; }
    }

    public void SetUpBoundriesUsingSpaceManager()
    {
        boundryRegionHeight = SpaceManager.regionH;
        boundryRegionWidth = SpaceManager.regionW;
    }

    public void OnCollisionEnter2D(Collision2D collision)

    {
        if (collision.gameObject.GetComponent<ObjVar>().Name == "GREY WALL")
        {
            hitObject = true;
        }
    }

}
