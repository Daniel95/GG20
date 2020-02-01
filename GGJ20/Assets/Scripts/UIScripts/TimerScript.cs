using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    private Text text;
    private void Start()
    {
        
    }

    private void OnEnable()
    {
        Player.StartJobEvent += OnStartJob;
    }

    private void OnStartJob(WorkManager.Job job)
    {
    }

    private void OnDisable()
    {
        //text.text = "";
        Player.StartJobEvent -= OnStartJob;
    }
}
