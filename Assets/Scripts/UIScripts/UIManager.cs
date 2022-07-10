using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public InteractionScript interactionScript;
    public static bool UIScreensActive;
    public Text scoreText;
    public Text healthText;
    public Text expText;
    public Text dialogTextInspector;
    public static Text dialogText;

    public Text_Typer_Script dialogTextTyperInspector;
    public static Text_Typer_Script dialogTextTyper;
    
    public GameObject playerStatusScreenPrefab;
    public MyStoreScript myStoreScript;
    public ObjVar playerObjVar;



    public static event Action UpdateUI;
    // Start is called before the first frame update
    void Start()
    {
        UpdateUI += UpdateBorderUI;
        dialogText = dialogTextInspector;
        dialogTextTyper = dialogTextTyperInspector;
        CallUpDateUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void CallUpDateUI()
    {
        UpdateUI();
    }

    public static void PrintToDialog(string dialogMessage)
    {
        //dialogText.text = dialogMessage;
        dialogTextTyper.TypeMessage(dialogMessage, false, null, 2);
    }

    public void UpdateBorderUI()
    {
        scoreText.text = ""+playerObjVar.intData[0];
        healthText.text = "" + playerObjVar.intData[1];
        expText.text = "" + playerObjVar.intData[2];

    }

    public void OpenPlayerStausScreen()
    {
        if (!UIScreensActive)
        {
            Instantiate(playerStatusScreenPrefab);
        }

    }

}
