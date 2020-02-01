using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;
using NaughtyAttributes;

public class WorkManager : MonoBehaviour
{
    public enum TaskType
    {
        Shaping,
        Sharpening,
        Heating
    }

    [Serializable]
    public struct Job
    {
        public int Time;
        public string Description;
        [ReorderableList]
        public List<TaskScriptableObject> Tasks;
    }

    private static WorkManager instance;

    public static WorkManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<WorkManager>();
            }

            return instance;
        }
    }

    [SerializeField] private JobLibraryScriptableObject jobLibrary;

    public Job ChooseJob()
    {
        int randomIndex = UnityEngine.Random.Range(0, jobLibrary.Jobs.Count);
        Job job = jobLibrary.Jobs[randomIndex];

        //Debug.Log(job.Description);
        //Debug.Log(job.Time);

        return job;
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
