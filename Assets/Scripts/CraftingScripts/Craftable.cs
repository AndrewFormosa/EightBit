using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Craftable : MonoBehaviour
{
    public int QuantityProduced;
    public List<Ingrenient> IngredientsList;
    public PlayerStatus[] requiredStatus;
    public PlayerStatus[] gainedStatus;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
[System.Serializable]
public class Ingrenient
{
    public GameObject ingredientItem;
    public int ingredientQuantity;
    public bool ingredientDestroyed;
}

[System.Serializable]
public class PlayerStatus
{
    public StatusTitles.status statusTitle;
    public int statusValue;
    public bool statusValueOverwrite;
}
