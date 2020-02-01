using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using NaughtyAttributes;

public class Player : MonoBehaviour
{
    private List<TaskManagerBase> taskManagers;

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

    private void Awake()
    {
        taskManagers = FindObjectsOfType<TaskManagerBase>().ToList();
    }

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
        TaskScriptableObject taskData = job.Tasks[taskIndex];
        WorkManager.TaskType taskType = taskData.GetTaskType();

        if(StartTaskEvent != null)
            StartTaskEvent(taskType);

        if (!taskManagers.Exists(x => x.GetTaskType() == taskType))
        {
            Debug.LogError("VERY BAD");
            return;
        }

        TaskManagerBase taskManagerBase = taskManagers.Find(x => x.GetTaskType() == taskType);
        taskManagerBase.Activate();
        taskManagerBase.SetTaskObject(taskData);

        //switch (taskType)
        //{
        //    case WorkManager.TaskType.Shaping:
        //        //GetComponent shaprening logic etc

        //        break;
        //    case WorkManager.TaskType.Sharpening:

        //        break;
        //    case WorkManager.TaskType.Heating:
        //        HeatingTaskManager heatingTaskMan = (HeatingTaskManager)taskManagerBase;
        //        HeatingTaskScriptableObject heatingTaskScriptableObject = (HeatingTaskScriptableObject)taskScriptableObject;
        //        heatingTaskMan.SetTargetHeat(heatingTaskScriptableObject.TargetHeat);
        //        break;
        //    case WorkManager.TaskType.UnBumping:
        //        UnBumpTaskManager unBumpTaskManager = (UnBumpTaskManager)taskManagerBase;
                
        //        break;
        //    default:
        //        throw new ArgumentOutOfRangeException();
        //}
    }
}
