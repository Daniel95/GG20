using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using NaughtyAttributes;

public class Player : MonoBehaviour
{
    [ReorderableList]
    public List<GameObject> cameraHooks;
    private GameObject mainCam = null;

    private int prevHookIndx = 0;
    private int currHookIndx = 0;

    float fp = 1;

    private void Start()
    {
        mainCam = GameObject.FindWithTag("MainCamera");

        //setup everything for camera switching
        currHookIndx = 0;
        prevHookIndx = cameraHooks.Count;
        if (cameraHooks.Count <= 0)
        {
            Debug.LogError("define camera hooks, you buffoon");
        }
        mainCam.transform.position = cameraHooks[0].transform.position;
        mainCam.transform.rotation = cameraHooks[0].transform.rotation;
    }

    private void Update()
    {
        //Slerp position & rotation if not within certain margin
        if (Vector3.Distance(mainCam.transform.position, cameraHooks[currHookIndx].transform.position) > 0.1f)
        {
            mainCam.transform.position = Vector3.Slerp(cameraHooks[prevHookIndx].transform.position, cameraHooks[currHookIndx].transform.position, fp);
            fp += Time.deltaTime;
        }

        if (Quaternion.Angle(mainCam.transform.rotation, cameraHooks[currHookIndx].transform.rotation) > 2)
        {
            mainCam.transform.rotation = Quaternion.Slerp(cameraHooks[prevHookIndx].transform.rotation, cameraHooks[currHookIndx].transform.rotation, fp);
            fp += Time.deltaTime;
        }
    }

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

    public void NextTaskLocation()
    {
        Debug.Log("Next Phase");
        if ((currHookIndx + 1) < cameraHooks.Count)
        {
            //safe
            GoToPhase(currHookIndx + 1);
        }
        else
        {
            //wrap to first phase
            GoToPhase(0);
        }
    }

    /// <summary>
    /// Public for any freaky boys who want to call this elsewhere
    /// </summary>
    /// <param name="phaseIndex"></param>
    public void GoToPhase(int phaseIndex)
    {
        prevHookIndx = currHookIndx;
        currHookIndx = phaseIndex;
        fp = 0.1f;
    }
}
