using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Serializable]
    public struct TaskTypePrefab
    {
        public WorkManager.TaskType TaskType;
        public GameObject Prefab;
    }

    [SerializeField] private List<TaskTypePrefab> taskTypePrefabs;

    /// <summary>
    /// Params: Description, Time
    /// </summary>
    public static Action<string, int> OnStartJobEvent;
    /// <summary>
    /// Params: WorkManager.TaskType
    /// </summary>
    public static Action<WorkManager.TaskType> OnStartTaskEvent;

    private WorkManager.Job job;
    private int taskIndex = 0;

    private WorkManager.TaskType taskType;
    private TaskScriptableObject taskScriptableObject;

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
        taskScriptableObject = job.Tasks[taskIndex];
        taskType = taskScriptableObject.GetTaskType();

        OnStartTaskEvent(taskType);

        TaskTypePrefab taskTypePrefab = taskTypePrefabs.Find(x => x.TaskType == taskType);
        Instantiate(taskTypePrefab.Prefab);

        switch (taskType)
        {
            case WorkManager.TaskType.Shaping:
                //GetComponent shaprening logic etc

                break;
            case WorkManager.TaskType.Sharpening:

                break;
            case WorkManager.TaskType.Heating:

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
