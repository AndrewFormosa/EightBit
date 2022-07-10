using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestScript1 : MonoBehaviour
{

    public GameObject a;
    public GameObject b;
    public Dictionary<int, GameObject> space;


    // Start is called before the first frame update
    void Start()
    {
        space = new Dictionary<int, GameObject>();
        Debug.Log("here we go");
        StartCoroutine(Bob(() => comp() ));
        space.Add(1, a);
        space.Add(2, b);

        Debug.Log(space[1].transform.position.y);
        Debug.Log(space[2].transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void comp()
    {
        Debug.Log("Comp");
    }

    public IEnumerator Bob(Action onComplete = null)
    {
        Debug.Log("Co Started");
        yield return new WaitForSeconds(5f);
        if (onComplete != null)
        {
            onComplete();
        }
    }
}