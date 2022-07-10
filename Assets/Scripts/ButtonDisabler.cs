using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonDisabler : MonoBehaviour
{
    private Button button;

    // Start is called before the first frame update
    void Start()
    {
        button = gameObject.GetComponent<Button>();
        GameManager.SetGamePausedStatus += SetButtonInteractable;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetButtonInteractable(bool GameStatus)
    {
        button.interactable = GameStatus;
    }
}
