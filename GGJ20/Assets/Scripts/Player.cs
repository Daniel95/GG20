using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using NaughtyAttributes;

public class Player : MonoBehaviour
{
    private List<CamPoint> cameraHooks;
    private List<SwordTeleportPoint> swordTeleportPoints;
    private GameObject mainCam = null;
    private GameObject sword = null;

    float fp = 1;

    private List<TaskManagerBase> taskManagers;

    /// <summary>
    /// Params: Description, Time
    /// </summary>
    public static Action<WorkManager.Job> StartJobEvent;
    /// <summary>
    /// Params: WorkManager.TaskType
    /// </summary>
    public static Action<WorkManager.TaskType> StartTaskEvent;

    private WorkManager.Job job;
    private int taskIndex = 0;
    private WorkManager.TaskType currentTaskType = WorkManager.TaskType.None;

    private Coroutine cameraSlerpCoroutine;
    private Coroutine swordSlerpCoroutine;

    public bool IsWorking() { return currentTaskType != WorkManager.TaskType.None; }
    public bool IsAtCounter() { return currentTaskType == WorkManager.TaskType.None; }

    private void Awake()
    {
        GetLerpPoints();

        //stuff for camera slerping
        mainCam = GameObject.FindWithTag("MainCamera");
        mainCam = GameObject.FindWithTag("MainCamera");

        //njeh
        if (cameraHooks.Count <= 0)
            Debug.LogError("define camera hooks, you buffoon");

        //I am now a lamda master
        Transform startCameraTransform = cameraHooks.Find(x => x.GetComponent<CamPoint>().taskType == WorkManager.TaskType.None).transform;
        mainCam.transform.position = startCameraTransform.position;
        mainCam.transform.rotation = startCameraTransform.rotation;

        taskManagers = FindObjectsOfType<TaskManagerBase>().ToList();
    }

    public void OnNextButton()
    {
        if (IsWorking())
        {
            bool startedTask = NextTask();

            if (!startedTask)
            {
                GoToCounter();
            }
        }
        else 
        {
            StartJob();
        }
    }

    private void GoToCounter()
    {
        currentTaskType = WorkManager.TaskType.None;

        Transform counterCameraTransform = cameraHooks.Find(x => x.taskType == WorkManager.TaskType.None).transform;
        Transform counterSwordTransform = cameraHooks.Find(x => x.taskType == WorkManager.TaskType.None).transform;

        if (cameraSlerpCoroutine == null)
        {
            cameraSlerpCoroutine = StartCoroutine(SlerpTransform(mainCam.transform, counterCameraTransform,() => cameraSlerpCoroutine = null));
        }

        if (cameraSlerpCoroutine == null)
        {
            swordSlerpCoroutine = StartCoroutine(SlerpTransform(sword.transform, counterSwordTransform, () => swordSlerpCoroutine = null));
        }
    }

    public void StartJob()
    {
        Debug.Log("Start the job, lazybum");
        job = WorkManager.Instance.ChooseJob();

        if (StartJobEvent != null)
        {
            StartJobEvent(job);
        }

        print(job.Description);
        print(job.Time);

        sword = GameObject.FindGameObjectWithTag("Sword");

        taskIndex = 0;

        StartTask(0);
    }

    public bool NextTask()
    {
        taskIndex++;

        if(taskIndex >= job.Tasks.Count)
        {
            return false;
        }

        StartTask(taskIndex);
        return true;
    }

    private void StartTask(int taskIndex)
    {
        fp = 0.1f;  //reset slerp time

        TaskScriptableObject taskData = job.Tasks[taskIndex];
        currentTaskType = taskData.GetTaskType();

        if(StartTaskEvent != null)
            StartTaskEvent(currentTaskType);

        if (!taskManagers.Exists(x => x.GetTaskType() == currentTaskType))
        {
            Debug.LogError("VERY BAD");
            return;
        }

        TaskManagerBase taskManagerBase = taskManagers.Find(x => x.GetTaskType() == currentTaskType);
        taskManagerBase.Activate();

        WorkManager.TaskType currentType =  taskManagerBase.GetTaskType();

        Transform currentCamTrans = GetCamPoint(currentType);
        Transform currentWeaponTrans = GetSwordTeleportPoint(currentType);

        if (cameraSlerpCoroutine == null)
        {
            cameraSlerpCoroutine = StartCoroutine(SlerpTransform(mainCam.transform, currentCamTrans, () => cameraSlerpCoroutine = null));
        }

        if (cameraSlerpCoroutine == null)
        {
            swordSlerpCoroutine = StartCoroutine(SlerpTransform(sword.transform, currentWeaponTrans, () => swordSlerpCoroutine = null));
        }

        StartCoroutine(SlerpTransform(mainCam.transform, currentCamTrans));
        StartCoroutine(SlerpTransform(taskManagerBase.sword.transform, currentWeaponTrans));

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

    private void GetLerpPoints()
    {
        swordTeleportPoints = FindObjectsOfType<SwordTeleportPoint>().ToList();
        cameraHooks = FindObjectsOfType<CamPoint>().ToList();
    }

    public Transform GetCamPoint(WorkManager.TaskType taskType)
    {
        CamPoint currentCamPoint = cameraHooks.Find(x => x.taskType == taskType);

        if (currentCamPoint == null)
        {
            Debug.LogError("Teleport point " + taskType + " does not exist!");
        }

        return currentCamPoint.transform;
    }

    public Transform GetSwordTeleportPoint(WorkManager.TaskType taskType)
    {
        SwordTeleportPoint swordTeleportPoint = swordTeleportPoints.Find(x => x.taskType == taskType);

        if (swordTeleportPoint == null)
        {
            Debug.LogError("Teleport point " + taskType + " does not exist!");
        }

        return swordTeleportPoint.transform;
    }

    private IEnumerator SlerpTransform(Transform transformToMove, 
        Transform targetTransform, 
        Action OnCompleted = null,
        float minDistanceOffset = 0.1f, 
        float minRotationOffset = 2.0f)
    {
        bool reachedPosition = Vector3.Distance(transformToMove.transform.position, targetTransform.position) <= minDistanceOffset;
        bool reachedRotation = Quaternion.Angle(transformToMove.transform.rotation, targetTransform.rotation) <= minRotationOffset;

        while (!reachedPosition && !reachedRotation)
        {
            transformToMove.transform.position = Vector3.Slerp(transformToMove.transform.position, targetTransform.position, fp);
            transformToMove.transform.rotation = Quaternion.Slerp(transformToMove.transform.rotation, targetTransform.rotation, fp);
            fp += Time.deltaTime;

            yield return null;
        }

        print("DONE");

        if (OnCompleted != null)
        {
            OnCompleted();
        }
    }
}
