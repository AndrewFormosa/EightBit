using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class TESTER_SCRIPT : MonoBehaviour
{
    GameObject MID;
    Text_Typer_Script typer;
    public string[] messages;
    private int messageCount = 0;

    public void Start()
    {
     
       
    }



    public GameObject itemDisplayPrefab;  
    public void Update()
    {


        //DEBUG
        if (Input.GetKeyDown(KeyCode.N))
        {
                    
                Debug.Log("TESTER");
                MID = Instantiate(itemDisplayPrefab);
                typer = MID.GetComponentInChildren<Text_Typer_Script>();
               // typer.TypeMessage("ERROR...ERROR...FILE INTERUPTION>>>>  UNEXPECTED EXCEPTION RECIEVED>...\n ..PLEASE WAIT> .....\n..UNABLE TO CONSOLIDATE ERROR>>\n..POTENTIAL VIRUS DETECTED...SOURCE>>..UNKNOWN>\n..RETURN>>>", true, () => nextMess(), 1);
                nextMess();         
        }
    }



    public void nextMess()
    {
      
        if (messages.Length > messageCount)
        {
            typer.TypeMessage(messages[messageCount], true, () => nextMess(), 2);
        }
        else
        {
            //Destroy(MID);
        }
        messageCount++;
    }

    



}

