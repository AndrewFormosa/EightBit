using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TestTileScript : MonoBehaviour
{
    public Tilemap objects;
    public Tile[] tiles;

    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {

            Debug.Log("position:" + objects.WorldToCell(this.transform.position));
           // Debug.Log("object:" + objects.GetTile(objects.WorldToCell(this.transform.position)).name);
            objects.SetTile(objects.WorldToCell(this.transform.position), tiles[0]);
         
        }


    }
}
