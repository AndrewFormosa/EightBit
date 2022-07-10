using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static bool gameRunning;
    public static Action<bool> SetGamePausedStatus; 

    // Start is called before the first frame update
    void Start()
    {
        gameRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void PauseGame()
    {
        gameRunning = false;
        Time.timeScale = 0;
        if (SetGamePausedStatus != null)
        {
            SetGamePausedStatus(gameRunning);
        }
    }

    public static void UnPauseGame()
    {
        gameRunning = true;
        Time.timeScale = 1;
        if (SetGamePausedStatus != null)
        {
            SetGamePausedStatus(gameRunning);
        }

        MultiItem_UI_Script[] allMIUI = FindObjectsOfType<MultiItem_UI_Script>();
        foreach(MultiItem_UI_Script MIUI in allMIUI)
        {
            Destroy(MIUI.gameObject);
        }
    }

}
