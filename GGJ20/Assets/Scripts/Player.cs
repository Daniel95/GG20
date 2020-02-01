using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Serializable]
    public struct TaskTypeTaskObjectPair
    {
        public WorkManager.TaskType TaskType;
        public TaskObject TaskObject;
    }

    [SerializeField] private List<TaskTypeTaskObjectPair> taskTypePrefabs;

    /// <summary>
    /// Params: Description, Time
    /// </summary>
    public static Action<string, int> OnStartJobEvent;
    /// <summary>
    /// Params: WorkManager.TaskType
    /// </summary>
    public static Action<WorkManager.TaskType> OnStartTaskEvent;
    public static Action<HeatingTaskScriptableObject> Test;

    private WorkManager.Job job;
    private int taskIndex = 0;

    public  WorkManager.Job StartJob()
    {
        WorkManager.Job job = WorkManager.Instance.ChooseJob();

        OnStartJobEvent(job.Description, job.Time);

        print(job.Description);
        print(job.Time);

        taskIndex = 0;

        StartTask(0);

        return job;
    }

    public void NextTask()
    {
        taskIndex++;
        StartTask(taskIndex);
    }

    private void StartTask(int taskIndex)
    {
        TaskScriptableObject taskScriptableObject = job.Tasks[taskIndex];
        WorkManager.TaskType taskType = taskScriptableObject.GetTaskType();

        OnStartTaskEvent(taskType);

        TaskTypeTaskObjectPair taskTypeTaskObjectPair = taskTypePrefabs.Find(x => x.TaskType == taskType);
        TaskObject taskObject = taskTypeTaskObjectPair.TaskObject;
        taskObject.Activate();

        switch (taskType)
        {
            case WorkManager.TaskType.Shaping:
                //GetComponent shaprening logic etc

                break;
            case WorkManager.TaskType.Sharpening:

                break;
            case WorkManager.TaskType.Heating:
                HeatingSword heatingSword = (HeatingSword) taskObject;
                HeatingTaskScriptableObject heatingTaskScriptableObject = (HeatingTaskScriptableObject)taskScriptableObject;
                heatingSword.SetTargetHeat(heatingTaskScriptableObject.TargetHeat);

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
