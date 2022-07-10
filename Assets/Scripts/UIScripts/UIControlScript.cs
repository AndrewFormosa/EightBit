using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControlScript : MonoBehaviour
{
    public GameObject playerStatusScreenPrefab;
    public Text scoreText;
    public Text healthText;




    // Start is called before the first frame update
    void Start()
    {
        UIManager.UpdateUI += UpdateAllUI;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateAllUI()
    {

    }

   

    public void OpenPlayerStatusScreen()
    {
        Instantiate(playerStatusScreenPrefab);
    }
}
