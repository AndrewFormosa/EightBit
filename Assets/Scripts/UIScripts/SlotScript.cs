using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotScript : MonoBehaviour
{
    public MultiItem_UI_Script multiItemUIScipt;
    public RectTransform GraphicTransform;
    public Image GraphicBorderImage;
    public RawImage GraphicSpriteImage;
    public Canvas SlotDiplayCanvas;
    public Text GraphicLabelText;
    public Text GraphicQtyText;

    public Color unSelectedHighlight;
    public Color singleClickedHighlight;
    public Color doubleClickedHighlight;



    public int slotNumber;//starts at 1.
    public string itemNameID;
    public int itemQty;
    public int displaySortingOrder;

    public void AddListeners()
    {

        multiItemUIScipt.UpdateAllSlotsGraphics += UpdateSlotGraphics;
    }

    public void ButtonClicked(PointerEventData clickData)
    {
        if (clickData.clickCount == 1) { multiItemUIScipt.SlotSingleClicked(slotNumber);}
        if (clickData.clickCount == 2) { multiItemUIScipt.SlotDoubleClicked(slotNumber);}
    }
  
    public void SetUpSlotGraphicPosition(Vector3 GraphicPosition)
    {
        GraphicTransform.localPosition = GraphicPosition;
    }

    public void UpdateSlotGraphics()
    {
        itemNameID = multiItemUIScipt.itemsNames[slotNumber - 1];
        itemQty = multiItemUIScipt.itemsValues[slotNumber - 1];


        //set deflault empty graphics
        string nameLabel = "";
        Texture imageTexture = null;
        Color borderColor = unSelectedHighlight;
        Color imageColor = Color.black;
        string volumeText = "";

        if (itemQty > 0)
        {  //set graphics if slot holds item
            nameLabel = itemNameID;
            imageTexture = ItemLoader.IndexedPrefabTexture(itemNameID);
            borderColor =unSelectedHighlight;
            imageColor = Color.white;
            if (itemQty > 1) { volumeText = "" + itemQty; } else { volumeText = "" + itemQty; ; }          
        }

        if (multiItemUIScipt.SingleClickedSlot == slotNumber)
        {
            borderColor = singleClickedHighlight;
        }

        if (multiItemUIScipt.DoubleClickedSlot == slotNumber)
        {
            borderColor =doubleClickedHighlight;
        }
        //set slot graphic attributes
        GraphicLabelText.text = nameLabel;
        GraphicSpriteImage.texture = imageTexture;
        GraphicSpriteImage.color = imageColor;
        GraphicQtyText.text = volumeText;
        GraphicBorderImage.color = borderColor;
             
    }

   

    // Start is called before the first frame update
    void Start()
    {
        SlotDiplayCanvas.sortingOrder = displaySortingOrder;
    }

    // Update is called once per frame
    void Update()
    {
        
    }



}
