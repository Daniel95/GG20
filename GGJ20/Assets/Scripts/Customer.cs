using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    [SerializeField, Tooltip("Time to fix customer weapon in seconds")]
    private float timeToFix = 30;
    private float timeLeft;
    [SerializeField, Tooltip("The time it takes for this customer's fix timer to initiate. To give the player a moment to read his request")]
    private float patience = 3f;

    [SerializeField]
    private Image clockImage = null;
    private Vector3 clockScale;
    private float quarterMark = 0f;
    float fl = 0.1f;

    private bool waiting;

    private Player player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        fl = 0.1f;
        quarterMark = timeToFix * 0.25f;    //get 25% of time
        clockScale = clockImage.transform.localScale;
        timeLeft = timeToFix;
        StartCoroutine(ReadTime());
    }

    private void Update()
    {
        if (!waiting)
            return; // fix timer has not started yet

        if(!player.isWorking)
            player.StartJob();

        //Debug.Log(timeLeft);
        //decrement fillrate of the clockimage
        clockImage.fillAmount = 1.0f / (timeToFix / timeLeft);

        //add lil scale effect on each 25% completed
        if(timeLeft % quarterMark >= -0.1f && timeLeft % quarterMark < 0.1f)
        {
            //Debug.Log("FEEDBACK YES");
            clockImage.transform.localScale *= 1.05f;
            fl = 0.1f;
        }
        else /*if (clockImage.transform.localScale.x <= clockScale.x - 0.01f && clockImage.transform.localScale.x >= clockScale.x + 0.01f)*/
        {
            //Debug.Log("Scale timer back down");
            clockImage.transform.localScale = Vector3.Lerp(clockImage.transform.localScale, clockScale, fl);
            fl += Time.deltaTime;
        }

        timeLeft -= Time.deltaTime;
        //when timer over, force player back to counter, display text result, customer leaves & takes weapon
        if (timeLeft <= 0)
        {
            waiting = false;
            timeLeft = timeToFix;
        }

    }

    IEnumerator ReadTime()
    {
        yield return new WaitForSeconds(patience);
        waiting = true;
    }
}
