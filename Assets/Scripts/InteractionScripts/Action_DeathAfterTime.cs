using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_DeathAfterTime : MonoBehaviour
{
    public float lifeTime;
    private float deathTime;
    public GameObject ItemDropedOnDeath;
    public int qtyDroped;
    
    // Start is called before the first frame update
    void Start()
    {
        deathTime = Time.time + lifeTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > deathTime) {
            if (ItemDropedOnDeath != null)
            {

                for (int i = 0; i < qtyDroped; i++)
                {
                    string key = SpaceManager.RegionToKey(SpaceManager.RegionVector(this.transform.position));
                    GameObject dropedObject = Instantiate(ItemDropedOnDeath, new Vector3(this.transform.position.x + (0.2f * i), this.transform.position.y + (0.2f * i), this.transform.position.z), Quaternion.identity);
                    dropedObject.transform.SetParent(SpaceManager.LoadedRegions[key].transform);
                }
            }


            Destroy(gameObject); }
    }



}
