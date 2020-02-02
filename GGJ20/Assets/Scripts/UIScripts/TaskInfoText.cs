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
                text.text = "SOMETHING SOMETHING";

                break;
            case WorkManager.TaskType.Sharpening:
                text.text = "SOMETHING Sharpening";


                break;
            case WorkManager.TaskType.Heating:
                text.text = "SOMETHING Heating";


                break;
            case WorkManager.TaskType.UnBumping:
                text.text = "SOMETHING UnBumping";


                break;
            case WorkManager.TaskType.Cleaning:
                text.text = "SOMETHING Cleaning";

                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(taskType), taskType, null);
        }
    }
}
