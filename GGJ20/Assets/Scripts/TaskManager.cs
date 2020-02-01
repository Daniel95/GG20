using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class TaskManager : MonoBehaviour
{
    private static TaskManager instance;

    public static TaskManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<TaskManager>();
            }

            return instance;
        }
    }

    [SerializeField] private JobLibraryScriptableObject jobLibrary;

    public Job ChooseJob()
    {
        int randomIndex = UnityEngine.Random.Range(0, jobLibrary.Jobs.Count);
        Job job = jobLibrary.Jobs[randomIndex];

        Debug.Log(job.Description);
        Debug.Log(job.Time);

        return job;
    }

    [Serializable]
    public struct Job
    {
        public int Time;
        public string Description;
        public List<ScriptableObject> Tasks;
    }

    /*
    public static void CreateTask(out ShapingTask shapingTask)
    {
        shapingTask = new ShapingTask();
        shapingTask.TargetWidth = Random.Range(Instance.MinTargetWidth, Instance.MaxTargetWidth);
    }

    public static void CreateTask(out SharpeningTask sharpeningTask)
    {
        sharpeningTask = new SharpeningTask();
        sharpeningTask.TargetSharpness = Random.Range(Instance.MinTargetSharpness, Instance.MaxTargetSharpness);
    }

    public class Task { }

    public class ShapingTask : Task
    {
        public int TargetWidth;
    }

    public class SharpeningTask
    {
        public int TargetSharpness;
    }
     */
}
