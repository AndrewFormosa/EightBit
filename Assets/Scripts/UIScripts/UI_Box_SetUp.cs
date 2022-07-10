using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Box_SetUp : MonoBehaviour
{

    public int NumberOfSlots;
    //DisplayVariables
    public float StoreDisplayHeight;
    public float StoreDisplayWidth;
    public float StoreDisplayPosX;
    public float StoreDisplayPosY;
    public float paddingX;
    public float paddingY;
    public float slotWidth;
    public float slotHeight;

    public GameObject slotPrefab;
    public GameObject StoreDisplay;
    public GameObject selectedSlot;

    //for transfer outside of this container.
    public GameObject OtherSelectedContainer;

    // public bool splitMode;
    // public bool storeActive;

    public GameObject StoreSlot;
    public GameObject background;
    private float panelx;
    private float panely;
    private int noOfCol;
    private int noOfRow;
    private float totalX;
    private float totalY;
    private float firstX;
    private float firstY;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
