using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UI_CraftingScreenScript : MonoBehaviour
{


    public bool PlayerInventoryStore;
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

    // public GameObject StoreSlotPrefab;
    //public GameObject StoreDisplay;
    //public GameObject selectedSlot;

    //for transfer outside of this container.
    public GameObject OtherSelectedContainer;

    // public bool splitMode;
    // public bool storeActive;

    public GameObject slot;
    public GameObject background;
    private float panelx;
    private float panely;
    private int noOfCol;
    private int noOfRow;
    private float totalX;
    private float totalY;
    private float firstX;
    private float firstY;

    public List<GameObject> craftableSlots;

    public bool StorageContainerActivated;


    // public event Action<bool> ActivateStoreUI;
    // public event Action UpdateAllSlotsGraphics;
    //  public event Action<GameObject> ObjectSelectedFromSlot;


    // Start is called before the first frame update
    void Start()
    {
        SetUpCraftingDisplay();
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateCraftableSlots(List<GameObject> craftableItems)
    {
        for (int i = 0; i < craftableItems.Count; i++)
        {
            Debug.Log("i:" + i);
            Debug.Log("craftable" + craftableItems.Count);
            Debug.Log(craftableSlots[i]);
          craftableSlots[i].transform.GetChild(1).GetComponent<RawImage>().texture = craftableItems[i].GetComponent<SpriteRenderer>().sprite.texture;
        }
    }

    public void SetUpCraftingDisplay()
    {
        panelx = StoreDisplayWidth;
        panely = StoreDisplayHeight;
        background.GetComponent<RectTransform>().sizeDelta = new Vector2(panelx, panely);
        background.GetComponent<RectTransform>().localPosition = new Vector3(StoreDisplayPosX, StoreDisplayPosY, 0);
        // panely = background.GetComponent<RectTransform>().sizeDelta.y;
        noOfCol = (int)((Mathf.Sqrt((float)NumberOfSlots)) + 0.9999f);
        int maxCols = (int)(panelx / (slotWidth + paddingX));
        noOfCol = Mathf.Min(maxCols, NumberOfSlots);
        noOfRow = (int)(((float)NumberOfSlots / (float)noOfCol) + 0.999f);
        totalX = ((float)noOfCol * slotWidth) + (paddingX * (noOfCol - 1f));
        totalY = ((float)noOfRow * slotHeight) + (paddingY * (noOfRow - 1f));
        firstX = ((-totalX / 2f) + (slotWidth / 2f)) + StoreDisplayPosX;
        firstY = ((totalY / 2f) - (slotHeight / 2f)) + StoreDisplayPosY;

        float x = firstX;
        float y = firstY;
        int row = 1;
        int col = 1;

        for (int i = 0; i < NumberOfSlots; i++)
        {
            GameObject item = Instantiate(slot, this.transform);
            item.GetComponent<Transform>().transform.localPosition= new Vector3(x, y, 0);
            craftableSlots.Add(item);
            //  item.GetComponent<>().AddListeners();

            x += (slotWidth + paddingX);
            col++;
            if (col > noOfCol)
            {
                row++; y -= (slotHeight + paddingY); col = 1; x = firstX;
            }
        }
        Destroy(slot);//*************DESTROY ORIGIONAL SLOT AFTER SET UP....    
       // gameObject.SetActive(false);
    }

}



