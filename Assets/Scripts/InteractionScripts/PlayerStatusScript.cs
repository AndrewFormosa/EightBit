using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusScript : MonoBehaviour
{
    public static int playerScore;
    public static int playerHealth;
    public int inspectorPlayerScore;
    public int inspectorPlayerHealth;

    public float playerHealthRate;

    public ObjVar playerObjVar;
    public SaveableScipt saveableScript;
    public LoadableScript loadableScript;

    // Start is called before the first frame update
    void Start()
    {
        loadableScript.ConfigureItem(DataManager.LoadPlayerStatus());
        if (playerObjVar.stringData.Length == 0)
        {
            Debug.Log("Need to set up");
            InititalisePlayerStatusVariables();
        }
        else {Debug.Log("already set up"); }

        StartCoroutine("ReduceHealth");
       
    }

    public void InititalisePlayerStatusVariables()
    {
        playerObjVar.stringData = new string[3];
        playerObjVar.intData = new int[3];
        playerObjVar.stringData[0] = "SCORE";
        playerObjVar.intData[0] = 0;
        playerObjVar.stringData[1] = "ENERGY";
        playerObjVar.intData[1] = 500;
        playerObjVar.stringData[2] = "EXP";
        playerObjVar.intData[2] = 0;

    }

    


    void Update()
    {


        if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log("Add new status");
            AddNewStatusTitle("new One", 2);
        }
    }

    IEnumerator ReduceHealth()
    {
        while (true)
        {
            yield return new WaitForSeconds(10);
            ChangeHealth(-10);
            UIManager.CallUpDateUI();
        }
    }



    public void ChangeScore(int pointsChange)
    {
        playerObjVar.intData[0] += pointsChange;
    }

    public void ChangeHealth(int healthChange)
    {
        playerObjVar.intData[1] += healthChange;
    }

    public void ChangeExp (int expChange)
    {
        playerObjVar.intData[2] += expChange;
    }

    public void AddNewStatusTitle(string newStatusTitle, int newInitialValue)
    {
       
        int oldArrayLength = playerObjVar.stringData.Length;
        List<string> newStringData = new List<string>();
        List<int> newIntData = new List<int>();

        for(int i = 0; i < oldArrayLength; i++)
        {
            newStringData.Add(playerObjVar.stringData[i]);
            newIntData.Add(playerObjVar.intData[i]);
        }
        newStringData.Add(newStatusTitle);
        newIntData.Add(newInitialValue);

        playerObjVar.stringData = new string[oldArrayLength + 1];
        playerObjVar.intData = new int[oldArrayLength + 1];

        playerObjVar.stringData = newStringData.ToArray();
        playerObjVar.intData = newIntData.ToArray();
    } 

    public void ChangeValueOfChosenStatus(string statusTitle, bool overwrite, int changeValue, bool createIfNew)
    {
        bool notfound = true;
      for(int i = 0; i < playerObjVar.stringData.Length; i++)
        {
            if (playerObjVar.stringData[i] == statusTitle)
            {
                notfound = false;
                if (overwrite)
                {
                    playerObjVar.intData[i] = changeValue;
                }
                else
                {
                    playerObjVar.intData[i] += changeValue;
                }
            }
        }
        if (notfound && createIfNew)
        {
            AddNewStatusTitle(statusTitle, changeValue);
        }
    }


    public void AddOrChangePlayerStatusArrayValues(PlayerStatus[] statusArray)
    {
        for (int j = 0; j < statusArray.Length; j++)
        {
            int valueAquired;
            if (statusArray[j].statusValueOverwrite) { valueAquired = statusArray[j].statusValue; } else { valueAquired = statusArray[j].statusValue; }
            ChangeValueOfChosenStatus(statusArray[j].statusTitle.ToString(), statusArray[j].statusValueOverwrite, valueAquired, true);
        }
        UIManager.CallUpDateUI();
    }

    public bool StatusRequirementMet(PlayerStatus[] requiredStatusArray)
    {
       
        bool answer = true;
        for (int i = 0; i < requiredStatusArray.Length; i++)

        {
            if (PlayerStatusContains(requiredStatusArray[i].statusTitle.ToString()))
            {
                if (GetValueOfChosenStatus(requiredStatusArray[i].statusTitle.ToString()) < (requiredStatusArray[i].statusValue))
                {
                    answer = false;
                }
            }
            else { answer = false;}
        }
        return answer;

    }


    public void ChangeValueOfChosenStatus(PlayerStatus status, bool createIfNew)
    {
        string statusTitle = status.statusTitle.ToString();
        int changeValue = status.statusValue;
        bool overwrite = status.statusValueOverwrite;
        bool notfound = true;

      for(int i = 0; i < playerObjVar.stringData.Length; i++)
        {
            if (playerObjVar.stringData[i] == statusTitle)
            {
                notfound = false;
                if (overwrite)
                {
                    playerObjVar.intData[i] = changeValue;
                }
                else
                {
                    playerObjVar.intData[i] += changeValue;
                }
            }
        }
        if (notfound && createIfNew)
        {
            AddNewStatusTitle(statusTitle, changeValue);
        }
    }

    public bool PlayerStatusContains(string statusTitle)
    {
        foreach (string status in playerObjVar.stringData)
        {
            if (status == statusTitle) { return true; }
        }
        return false;
    }

    public int GetValueOfChosenStatus(string statusTitle)
    {
        for(int i=0; i< playerObjVar.stringData.Length; i++)
        {
            if (playerObjVar.stringData[i] == statusTitle)
            {
                return playerObjVar.intData[i];
            }                       
        }
        return 0;
    }

    public List<string> GetPlayerStatusTitles()
    {
        List<string> playerStatusTitleList = new List<string>();
        foreach (string status in playerObjVar.stringData)
        {
            playerStatusTitleList.Add(status);
        }
        return playerStatusTitleList;
    }


    void OnDestroy()
    {
        DataManager.SavePlayerStatus(saveableScript.GetSavableData());

    }

    // Update is called once per frame
 



}
