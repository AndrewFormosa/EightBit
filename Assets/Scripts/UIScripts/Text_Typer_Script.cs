using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Text_Typer_Script : MonoBehaviour
{
    public Text targetText;
    public float typingSpeed;
    public string textContent;
    public Action CallBack;
    public bool currentlyTyping;
    public float endWaitTime;
    public float cursorFlashTime;

    // Start is called before the first frame update
    void Start()
    {

    }
    
    IEnumerator TypeText(string message)
    {
        char[] messageCharacters = message.ToCharArray();
        string printedMessage = "";

        for (int i = 0; i < 5; i++)
        {
            targetText.text = ">";
            yield return new WaitForSeconds(cursorFlashTime);
            targetText.text = "";
            yield return new WaitForSeconds(cursorFlashTime);
        }

        for (int i = 0; i< messageCharacters.Length; i++)
        {
            float waitingTime = (UnityEngine.Random.Range(0.2f,1f )) * typingSpeed;
            targetText.text = printedMessage;
            
            printedMessage += messageCharacters[i];
            yield return new WaitForSeconds(waitingTime);
        }      



        for (int i = 0; i< 5; i++)
        {
            targetText.text = printedMessage + ">";
            yield return new WaitForSeconds(cursorFlashTime);
            targetText.text = printedMessage;
            yield return new WaitForSeconds(cursorFlashTime);
        }
        yield return new WaitForSeconds(endWaitTime);
        targetText.text = "";
        OnComplete();
    }

   

public void TypeMessage(string messageString, bool CallBackWhenComplete, Action CallBackAction, float WaitTimeAfterMessageSeconds )
    {
        if (!currentlyTyping)
        {
            endWaitTime = WaitTimeAfterMessageSeconds;
            currentlyTyping = true;
            StartCoroutine("TypeText", messageString);
            CallBack = null;
            if (CallBackWhenComplete)
            {
                CallBack += CallBackAction;
            }
        }
    }

    public void TypeMessage(string messageString, bool CallBackWhenComplete, Action CallBackAction, float WaitTimeAfterMessageSeconds, bool takePriority )
    {
        if (takePriority)
        {
            StopAllCoroutines();
            currentlyTyping = false;
        }

        if (!currentlyTyping)
        {
            endWaitTime = WaitTimeAfterMessageSeconds;
            currentlyTyping = true;
            StartCoroutine("TypeText", messageString);
            CallBack = null;
            if (CallBackWhenComplete)
            {
                CallBack += CallBackAction;
            }
        }
    }

public void OnComplete()
    {
        currentlyTyping = false;
        if (CallBack != null)
        {
            CallBack();
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            TypeMessage(textContent, false, null,3);
        }
    }
}
