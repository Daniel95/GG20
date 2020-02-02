using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DescriptionTextScript : MonoBehaviour
{
    private Text text;

    private void Start()
    {
        text = GetComponent<Text>();
    }

    private void OnEnable()
    {
        Player.GetJobEvent += OnDisplayJob;
        Player.EndJobEvent += OnEndJob;
        Player.StartJobEvent += OnStartJob;
        Player.StartTaskEvent += OnNextTask;
    }

    private void OnDisplayJob(WorkManager.Job job)
    {
        text.text = job.Description;
    }

    private void OnEndJob(Dictionary<WorkManager.TaskType, float> results)
    {
        text.text = "";
    }

    private void OnStartJob(WorkManager.Job job)
    {
        //not sure if these are needed but its convenient
    }

    private void OnNextTask(WorkManager.TaskType task)
    {
        //same story
    }

    private void OnDisable()
    {
        text.text = "";
        Player.GetJobEvent -= OnDisplayJob;
        Player.EndJobEvent -= OnEndJob;
        Player.StartJobEvent -= OnStartJob;
        Player.StartTaskEvent -= OnNextTask;
    }
}
