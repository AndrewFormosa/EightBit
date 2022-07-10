using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_ButtonClick : MonoBehaviour, IPointerClickHandler
{
    public GameObject targetObject;
    public string RoutineName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData pointerEvent)
    {
        
        if (targetObject != null)
        {
            targetObject.BroadcastMessage(RoutineName,pointerEvent, SendMessageOptions.DontRequireReceiver);
        }
    }

  
}
