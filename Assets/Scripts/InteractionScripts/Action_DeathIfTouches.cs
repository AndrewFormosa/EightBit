using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_DeathIfTouches : MonoBehaviour
{
    public GameObject[] killerObjectPrefabs;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        string collName = collision.gameObject.GetComponent<ObjVar>().Name;
        foreach (GameObject killerPrefab in killerObjectPrefabs)
      {
            string killerName = killerPrefab.GetComponent<ObjVar>().Name;
            if (killerName == collName)
            {
                Destroy(gameObject);
            }
        }

       
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        string collName = collision.gameObject.GetComponent<ObjVar>().Name;
        foreach (GameObject killerPrefab in killerObjectPrefabs)
      {
            string killerName = killerPrefab.GetComponent<ObjVar>().Name;
            if (killerName == collName)
            {
                Destroy(gameObject);
            }
        }

       
    }

}
