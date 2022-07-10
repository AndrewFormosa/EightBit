using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class MultiItem_UI_Script : MonoBehaviour
{//StoreVariables

    public int SingleClickedSlot;
    public int DoubleClickedSlot;

    public int dispaySortingOrder;
    public float displayPosX;
    public float displayPosY;
    public float displayWidth;
    public float displayTitleHeight;
    public float displayItemsPanelHeight;
    public float displayInfoHeight;
    public float displayBorderWidth;

    public bool PlayerInventoryStore;
    public int NumberOfSlots;

    //DisplayVariables
    public float HeaderHeight;

    public float StoreDisplayHeight;

    public float paddingX;
    public float paddingY;
    public float slotWidth;
    public float slotHeight;




  //  public GameObject Display;
    public GameObject titelPanel;
  
    public GameObject slotsPanel;
    public GameObject infoPanel;
    public GameObject ContainerSlotPrefab;
    public Canvas display;///use to set order
    //public ItemLoader itemLoader;

    public Text mainTitle;
    public Text navigationText;
    public Text nextText;
    public Text backText;
    public bool overideONOFFSwitch;
    


    //  public GameObject StoreSlot;

    private float panelx;
    private float panely;
    private int noOfCol;
    private int noOfRow;
    private float totalX;
    private float totalY;
    private float firstX;
    private float firstY;
    private int maxSlots;
    private int page;
    private int maxPage;

    private List<GameObject> slots = new List<GameObject>();

    public string[] itemsNames;
    public int[] itemsValues;


    public bool StorageContainerActivated;

    public event Action UpdateAllSlotsGraphics;

    public event Action<int,MultiItem_UI_Script> SlotSingClicked;
    public event Action<int,MultiItem_UI_Script> SlotDoubClicked;
    public event Action<MultiItem_UI_Script> CloseClicked;


    // Start is called before the first frame update
    void Start()
    {
        page = 0;
        if (PlayerInventoryStore)
        {
            SingleClickedSlot = InteractionScript.openSlot;
        }
        else
        {
            SingleClickedSlot = 0;
        }
        GameManager.PauseGame();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetUpDisplay(string UITitle, string[]names, int[]values)
    {

        display.sortingOrder = dispaySortingOrder;
        mainTitle.text = UITitle;
        itemsNames = names;
        itemsValues = values;
    //   Display.SetActive(true);
        NumberOfSlots = names.Length;
        SetUpMultiItemUIDisplay();
        CreateListOfSlots();
        PositionAndActivateSlots();
       
        if (UpdateAllSlotsGraphics != null)
        {
            UpdateAllSlotsGraphics();
        }  
    }

    public void SetDisplayOrder(int displayOrder)
    {
        display.sortingOrder = displayOrder;
    }

    public void CallUpdateAllSlotGraphics()
    {
        UpdateAllSlotsGraphics();
    }

      public void SetHeadings(string navigation, bool next, bool back)
      {
         navigationText.text = navigation;
       if (next) { nextText.text = "NEXT"; } else { nextText.text = ""; }
       if (back) { backText.text = "BACK"; } else { backText.text = ""; }
      }



    public void SlotSingleClicked(int slotNumber)
    {
        if (SlotSingClicked != null)
        {
            SlotSingClicked(slotNumber,this);
        }
    
    }

    public void SlotDoubleClicked(int slotNumber)
    {
        if (SlotDoubClicked != null)
        {
            SlotDoubClicked(slotNumber, this);
        }
    }


    public void Close()
    {
        if (CloseClicked != null)
        {
            CloseClicked(this);
        }
        UIManager.CallUpDateUI();
        InteractionScript.playerInputAllowed = true;
        GameManager.UnPauseGame();
        Destroy(gameObject);
    }


   
    public void SetUpBorderedBox(GameObject BackgroundPanelObject, float width, float height, float x, float y, float borderWidth)
    {
        RectTransform titlePanelRect = BackgroundPanelObject.GetComponent<RectTransform>();
        RectTransform titleForegroundRect = BackgroundPanelObject.transform.GetChild(0).GetComponent<RectTransform>();
        titlePanelRect.sizeDelta = new Vector2(width, height);
        titleForegroundRect.sizeDelta = new Vector2((width - (borderWidth * 2f)), (height - (borderWidth * 2f)));
        titlePanelRect.localPosition = new Vector3(x, y, 0);
    }

    //Set up the container and display
    public void SetUpMultiItemUIDisplay()
    {

        //setUpTitlePanel;
        float PosX = displayPosX;
        float PosY = displayPosY + ((displayTitleHeight + displayItemsPanelHeight) / 2f);
        SetUpBorderedBox(titelPanel, displayWidth, displayTitleHeight, PosX, PosY, displayBorderWidth);

        //setUpSlotPanel;
        PosY = displayPosY;
        SetUpBorderedBox(slotsPanel, displayWidth, displayItemsPanelHeight, PosX, PosY, displayBorderWidth);
        //setUpInfoPanel
        PosY = displayPosY - ((displayInfoHeight + displayItemsPanelHeight) / 2f);
        SetUpBorderedBox(infoPanel, displayWidth, displayInfoHeight, PosX, PosY, displayBorderWidth);
    }

    public void CreateListOfSlots()
    {
        for (int i = 0; i < NumberOfSlots; i++)
        {
            Debug.Log("createLIST");
            GameObject item = Instantiate(ContainerSlotPrefab, gameObject.transform);
            SlotScript slotScript = item.GetComponent<SlotScript>();
            slotScript.AddListeners();
            slotScript.slotNumber = i + 1; // starts at 1.
            slotScript.itemNameID = itemsNames[i];
            slotScript.itemQty = itemsValues[i];
            slotScript.displaySortingOrder = dispaySortingOrder + 1;
            slots.Add(item);
        }
        Destroy(ContainerSlotPrefab);//*************DESTROY ORIGIONAL SLOT AFTER SET UP....    
    }

    public void PositionAndActivateSlots()
    {
        panelx = displayWidth;
        panely = StoreDisplayHeight;
        int maxCols = (int)(panelx / (slotWidth + paddingX));
        int maxRows = (int)((panely - HeaderHeight) / (slotHeight + paddingY+paddingY));
        Debug.Log("MaxRows:" + maxRows);
        int maxSlots = maxCols * maxRows;
        maxPage = ((int)((float)NumberOfSlots / (float)(maxSlots) + 0.999f)) - 1;
        noOfCol = Mathf.Min(maxCols, NumberOfSlots);
        noOfRow = (int)(((float)NumberOfSlots / (float)noOfCol) + 0.999f);
        if (noOfRow > maxRows) { noOfRow = maxRows; }
        totalX = ((float)noOfCol * slotWidth) + (paddingX * (noOfCol - 1f));
        totalY = ((float)noOfRow * slotHeight) + (paddingY * (noOfRow - 1f) + HeaderHeight);
        firstX = ((-totalX / 2f) + (slotWidth / 2f)) + displayPosX;
        firstY = (((totalY / 2f) - (slotHeight / 2f)) + displayPosY);

        float x = firstX;
        float y = firstY - HeaderHeight;
        int row = 1;
        int col = 1;

        int firstSlotDisplayed = (maxSlots * page);
        int lastSlotDisplayed = firstSlotDisplayed + maxSlots-1;
        if (lastSlotDisplayed > NumberOfSlots-1) { lastSlotDisplayed = NumberOfSlots - 1; }
        Debug.Log("firstSlot:" + firstSlotDisplayed);
        Debug.Log("LastSlot:" + lastSlotDisplayed);

        for (int i = 0; i < firstSlotDisplayed; i++)
        {
            slots[i].transform.GetChild(0).gameObject.SetActive(false);

        }

        for (int i = firstSlotDisplayed; i < lastSlotDisplayed + 1; i++)
        {
            slots[i].transform.GetChild(0).gameObject.SetActive(true);
            slots[i].GetComponent<SlotScript>().SetUpSlotGraphicPosition(new Vector3(x, y, 0));
            x += (slotWidth + paddingX);
            col++;
            if (col > noOfCol)
            {
                row++; y -= (slotHeight + paddingY); col = 1; x = firstX;
            }
        }

        for (int i = lastSlotDisplayed+1; i < NumberOfSlots; i++)
        {
            slots[i].transform.GetChild(0).gameObject.SetActive(false);
        }

        if (page == 0) { backText.text = ""; } else { backText.text = "BACK"; }
        if (page == maxPage) { nextText.text = ""; } else { nextText.text = "NEXT"; }
        navigationText.text = "PAGE " + page;
    }

    public void Next()
    {
        Debug.Log("next");
        if (page < maxPage)
        {
            page++; PositionAndActivateSlots();

        }
    }

    public void Back()
    {
        if (page > 0)
        {
            page--; PositionAndActivateSlots();
        }
    }


}