using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_EffectsPlayerWhenTouched : MonoBehaviour
{
    public PlayerStatusScript playerStatusScript;
    public PlayerStatus[] ReqiredStatusForEffect;
    public PlayerStatus[] ReqiredStatusToStopEffect;
    public PlayerStatus[] AquiredStatus;
    public float effectOverTimeOnly;
    private bool notPaused;


    // Start is called before the first frame update
    void Start()
    {
        notPaused = true;
        playerStatusScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatusScript>();

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag=="Player")
        {

            if (effectOverTimeOnly== 0)
            {
                Touched();
                
            }
            else
            {
                if (notPaused)
                {
                    StartCoroutine("TouchedOverTime");
                }
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag== "Player")
        {

            if (effectOverTimeOnly == 0)
            {
                Touched();
            }
            else
            {
                if (notPaused)
                {
                    StartCoroutine("TouchedOverTime");
                }
            }
        }
    }


    public IEnumerator TouchedOverTime()
    {
        Debug.Log("TOUCHED Over Time");
        notPaused = false;
        Touched();
        yield return new WaitForSeconds(effectOverTimeOnly);
        notPaused = true;
    }

    public void Touched()
    {
        if (playerStatusScript.StatusRequirementMet(ReqiredStatusForEffect)&&(EffectNotStopped(ReqiredStatusToStopEffect)))
        {
            playerStatusScript.AddOrChangePlayerStatusArrayValues(AquiredStatus);
        }
    }

    public bool EffectNotStopped( PlayerStatus[] requiredToStop)
    {
        bool answer = true;
        if (playerStatusScript.StatusRequirementMet(requiredToStop)&&requiredToStop.Length>0)
        {
            answer = false;
        }

        return answer;
    }

}
