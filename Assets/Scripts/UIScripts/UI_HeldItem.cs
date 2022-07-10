using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HeldItem : MonoBehaviour
{
    public RawImage heldImage;
    // Start is called before the first frame update
    void Start()
    {
        UIManager.UpdateUI += UpdateImage;    
    }


   void UpdateImage()
    {
       heldImage.texture = InteractionScript.heldItem.GetComponent<SpriteRenderer>().sprite.texture;
    }


}
