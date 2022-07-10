using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus_UI : MonoBehaviour
{
    public int linesPerPage;
    public Text statusText;
    public int pageNo;
    public ObjVar playerStatus;
    public int lastPageNo;
    public GameObject nextButton;
    public GameObject lastButton;
    public GameObject closeButton;
    public int numberOfLines;


    // Start is called before the first frame update
    void Start()
    {
        UIManager.UIScreensActive = true;
        GameManager.PauseGame();
        pageNo = 0;
        playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<ObjVar>();
        numberOfLines = playerStatus.stringData.Length;
        lastPageNo = (numberOfLines/ linesPerPage);
        SetUpText();
       
    }

    public void NextButton()
    {
        pageNo++;
        SetUpText();
    }

    public void BackButton()
    {
        pageNo--;
        SetUpText();
    }



    public void SetUpText()
    {
        int firstLine = pageNo * linesPerPage;
        int lastLine = firstLine + linesPerPage;
        if (pageNo > 0) { lastButton.SetActive(true); } else { lastButton.SetActive(false); }
        if (lastLine > numberOfLines-1) { nextButton.SetActive(false); lastLine = numberOfLines; } else { nextButton.SetActive(true); }

        string totalText = "ME:\n\n";
        for (int i = firstLine; i < lastLine; i++)
        {
            totalText += playerStatus.stringData[i];
            if (playerStatus.intData[i] != 0)
            {
                totalText += " (" + playerStatus.intData[i] + ")";
            }
            totalText += "\n";
        }
        statusText.text = totalText;

    }

    public void OnClose()
    {
        UIManager.UIScreensActive = false;
        GameManager.UnPauseGame();
        Destroy(gameObject);
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
