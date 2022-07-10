using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemAction_UI_Script : MonoBehaviour
{
    public Canvas displayCanvas;
    public RawImage itemRawImage;
    public Text qtyToCraftText;
    public int displaySortOrder;
    public int qtyToCraft;
    public Texture itemTexture;
    public int maxCraftQty;
    public Craft_Script craftScript;
    public MultiItem_UI_Script craftMIUI;
    
    // Start is called before the first frame update
    void Start()
    {
        qtyToCraft = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUpParameters(Texture image, int maxQty)
    {
        itemRawImage.texture = image;
        maxCraftQty = maxQty;
        UpDateDisplay();
    }

    public void UpDateDisplay()
    {
        qtyToCraftText.text = ""+qtyToCraft;
    }

    public void OnClose()
    {
        craftScript.doubleSelectedSlot = 0;
        craftScript.singleSelectedSlot = 0;
        craftMIUI.SingleClickedSlot = 0;
        craftMIUI.CallUpdateAllSlotGraphics();      
        Destroy(gameObject);
        
    }

    public void OnCraftItem()
    {
        craftScript.OnCraftItem(qtyToCraft);
        Destroy(gameObject);
    }
    public void OnMakeMore()
    {
        if (qtyToCraft < maxCraftQty)
        {
            qtyToCraft++;
            UpDateDisplay();
        }
    }
    public void OnMakeLess()
    {
        if (qtyToCraft > 0)
        {
            qtyToCraft--;
            UpDateDisplay();
        }
    }

    public void SetUpItemActionUI()
    {
        displayCanvas.sortingOrder = displaySortOrder;


    }
}
