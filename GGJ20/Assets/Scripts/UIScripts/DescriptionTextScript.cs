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
        text.text = "";
    }

    private void OnEnable()
    {
        Player.GetJobEvent += OnDisplayJob;
        Player.EndJobEvent += OnEndJob;
        Player.StartJobEvent += OnStartJob;
        Player.StartTaskEvent += OnNextTask;
        Customer.ResultTextMadeEvent += PrintResults;
    }

    private void OnDisplayJob(WorkManager.Job job)
    {
        Debug.Log("Display job desc");
        text.text = job.Description;
        Player.GetJobEvent -= OnDisplayJob;
    }

    private void PrintResults(string s)
    {
        Debug.Log("Trying to print some results : " + s);
        //text.enabled = true;
        text.text = s;
    }

    private void OnEndJob(Dictionary<WorkManager.TaskType, float> results)
    {
        //text.text = "";
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
        Customer.ResultTextMadeEvent -= PrintResults;
    }
}
