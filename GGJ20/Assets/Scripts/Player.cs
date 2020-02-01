﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using NaughtyAttributes;

public class Player : MonoBehaviour
{
    [Serializable]
    public struct TaskPair
    {
        public WorkManager.TaskType TaskType;
        public TaskManager TaskObject;
    }

    [SerializeField] [ReorderableList] private List<TaskPair> taskManagerPairs;

    /// <summary>
    /// Params: Description, Time
    /// </summary>
    public static Action<string, int>StartJobEvent;
    /// <summary>
    /// Params: WorkManager.TaskType
    /// </summary>
    public static Action<WorkManager.TaskType> StartTaskEvent;

    private WorkManager.Job job;
    private int taskIndex = 0;

    [HideInInspector]
    public bool isWorking;

    public  WorkManager.Job StartJob()
    {
        isWorking = true;
        job = WorkManager.Instance.ChooseJob();

        if (StartJobEvent != null)
            StartJobEvent(job.Description, job.Time);

        print(job.Description);
        print(job.Time);

        taskIndex = 0;

        StartTask(0);

        return job;
    }

    public void NextTask()
    {
        if(taskIndex >= job.Tasks.Count)
        {
            isWorking = false;
            return;
        }

        taskIndex++;
        StartTask(taskIndex);

    }

    private void StartTask(int taskIndex)
    {
        TaskScriptableObject taskScriptableObject = job.Tasks[taskIndex];
        WorkManager.TaskType taskType = taskScriptableObject.GetTaskType();

        if(StartTaskEvent != null)
            StartTaskEvent(taskType);

        if (!taskManagerPairs.Exists(x => x.TaskType == taskType))
        {
            Debug.LogError("VERY BAD");
            return;
        }

        TaskPair taskTypeTaskObjectPair = taskManagerPairs.Find(x => x.TaskType == taskType);
        TaskManager taskObject = taskTypeTaskObjectPair.TaskObject;
        taskObject.Activate();

        switch (taskType)
        {
            case WorkManager.TaskType.Shaping:
                //GetComponent shaprening logic etc

                break;
            case WorkManager.TaskType.Sharpening:

                break;
            case WorkManager.TaskType.Heating:
                HeatingTaskManager heatingTaskMan = (HeatingTaskManager) taskObject;
                HeatingTaskScriptableObject heatingTaskScriptableObject = (HeatingTaskScriptableObject)taskScriptableObject;
                heatingTaskMan.SetTargetHeat(heatingTaskScriptableObject.TargetHeat);

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
