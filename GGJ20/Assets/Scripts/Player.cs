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

    private bool startedJob;

    public bool IsWorking() { return currentTaskType != WorkManager.TaskType.None; }
    public bool IsAtCounter() { return currentTaskType == WorkManager.TaskType.None; }

    private void Awake()
    {
        GetLerpPoints();

        //stuff for camera slerping
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
        if (startedJob)
        {
            bool startedTask = NextTask();

            if (!startedTask)
            {
                startedJob = false;
                GoToCounter();
            }
        }
        else 
        {
            StartJob();
            startedJob = true;
        }
    }

    private void GoToCounter()
    {
        currentTaskType = WorkManager.TaskType.None;

        Transform counterCameraTransform = cameraHooks.Find(x => x.taskType == WorkManager.TaskType.None).transform;
        Transform counterSwordTransform = cameraHooks.Find(x => x.taskType == WorkManager.TaskType.None).transform;

        SlerpCameraAndSword(counterCameraTransform, counterSwordTransform);
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
    }

    public bool NextTask()
    {

        if(taskIndex >= job.Tasks.Count)
        {
            return false;
        }


        StartTask(taskIndex);
        taskIndex++;
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

        Debug.Log(currentTaskType.ToString() + " " + taskManagerBase.ToString());

        Transform currentCamTrans = GetCamPoint(currentTaskType);
        Transform currentWeaponTrans = GetSwordTeleportPoint(currentTaskType);

        SlerpCameraAndSword(currentCamTrans, currentWeaponTrans);

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

    private void SlerpCameraAndSword(Transform cameraTarget, Transform swordTarget)
    {
        if (cameraSlerpCoroutine != null || swordSlerpCoroutine != null)
        {
            return;
        }

        cameraSlerpCoroutine = StartCoroutine(SlerpTransform(mainCam.transform, cameraTarget, () => cameraSlerpCoroutine = null));
        swordSlerpCoroutine = StartCoroutine(SlerpTransform(sword.transform, swordTarget, () => swordSlerpCoroutine = null));
    }

    private IEnumerator SlerpTransform(Transform transformToMove, 
        Transform targetTransform, 
        Action OnCompleted = null,
        float minDistanceOffset = 0.2f, 
        float minRotationOffset = 5.0f)
    {
        while (true)
        {
            float positionOffset = Vector3.Distance(transformToMove.transform.position, targetTransform.position);
            float angleOffset = Quaternion.Angle(transformToMove.transform.rotation, targetTransform.rotation);

            bool reachedPosition = positionOffset <= minDistanceOffset;
            bool reachedRotation = angleOffset <= minRotationOffset;

            if (reachedPosition && reachedRotation)
            {
                break;
            }
 
            transformToMove.transform.position = Vector3.Slerp(transformToMove.transform.position, targetTransform.position, fp);
            transformToMove.transform.rotation = Quaternion.Slerp(transformToMove.transform.rotation, targetTransform.rotation, fp);
            fp += Time.deltaTime;

            yield return null;
        }

        if (OnCompleted != null)
        {
            OnCompleted();
        }
    }
}
