using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TaskInfoText : MonoBehaviour
{ 
    private Text text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    private void OnEnable()
    {
        Player.StartTaskEvent += OnStartTask;
        Player.EndJobEvent += Reset;
    }

    private void OnDisable()
    {
        Player.StartTaskEvent -= OnStartTask;
        Player.EndJobEvent -= Reset;
    }

    private void Reset(WorkManager.Job job, Dictionary<WorkManager.TaskType, float> info)
    {
        text.text = "";
    }

    private void OnStartTask(WorkManager.TaskType taskType)
    {
        switch (taskType)
        {
            case WorkManager.TaskType.None:
                break;
            case WorkManager.TaskType.Shaping:
                text.text = "Align and hit";
                //move down to turn over
                break;
            case WorkManager.TaskType.Sharpening:
                text.text = "Hold to sharpen";


                break;
            case WorkManager.TaskType.Heating:
                text.text = "Move it in the Forge";


                break;
            case WorkManager.TaskType.UnBumping:
                text.text = "Press Sword to unbump";


                break;
            case WorkManager.TaskType.Cleaning:
                text.text = "Rub to clean";

                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(taskType), taskType, null);
        }
    }
}
