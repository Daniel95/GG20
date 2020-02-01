using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    private Image clockImage;
    private float timeLimit;
    private float timeLeft;
    private float fl = 0.1f;
    private float quarterMark = 0f;
    private Vector3 clockScale;

    private void Start()
    {
        Player.StartJobEvent += OnStartJob;
        clockImage = GetComponent<Image>();
        clockScale = transform.localScale;
    }

    private void Update()
    {
        if(timeLeft > 0)
        {
            clockImage.fillAmount = 1.0f / (timeLimit / timeLeft);
        }
        else
        {
            return;
        }

        if (timeLeft % quarterMark >= -0.1f && timeLeft % quarterMark < 0.1f)
        {
            //Debug.Log("FEEDBACK YES");
            clockImage.transform.localScale *= 1.05f;
            fl = 0.1f;
        }
        else
        {
            //Debug.Log("Scale timer back down");
            clockImage.transform.localScale = Vector3.Lerp(clockImage.transform.localScale, clockScale, fl);
            fl += Time.deltaTime;
        }
    }

    private void OnStartJob(WorkManager.Job job)
    {
        timeLimit = time;
        timeLeft = timeLimit;
        Debug.Log("Start the job");
    }

    private void OnEnable()
    {
        Player.StartJobEvent += OnStartJob;
    }

    private void OnDisable()
    {
        //text.text = "";
        Player.StartJobEvent -= OnStartJob;
    }
}
