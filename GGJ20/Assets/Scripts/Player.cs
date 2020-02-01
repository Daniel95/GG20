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

    private Transform currentCamTrans;
    private Transform currentWeaponTrans;

    //private int prevHookIndx = 0;
    //private int currHookIndx = 0;

    float fp = 1;

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
        GetLerpPoints();

        //stuff for camera slerping
        mainCam = GameObject.FindWithTag("MainCamera");

        //njeh
        if (cameraHooks.Count <= 0)
            Debug.LogError("define camera hooks, you buffoon");

        //I am now a lamda master
        currentCamTrans = cameraHooks.Find(x => x.GetComponent<CamPoint>().taskType == WorkManager.TaskType.None).transform;
        mainCam.transform.position = currentCamTrans.position;
        mainCam.transform.rotation = currentCamTrans.rotation;

        currentWeaponTrans = transform; //temp

        taskManagers = FindObjectsOfType<TaskManagerBase>().ToList();
    }

    void Update()
    {
        SlerpCamera();
        SlerpWeapon();
    }

    //public void NextTaskLocation()
    //{
    //    if ((currHookIndx + 1) < cameraHooks.Count)
    //    {
    //        //safe
    //        GoToPhase(currHookIndx + 1);
    //    }
    //    else
    //    {
    //        //wrap to first phase
    //        GoToPhase(0);
    //    }
    //}

    /// <summary>
    /// Public for any freaky boys who want to call this elsewhere (can be used to force camera to a specific phase)
    /// </summary>
    /// <param name="phaseIndex"></param>
    //public void GoToPhase(int phaseIndex)
    //{
    //    prevHookIndx = currHookIndx;
    //    currHookIndx = phaseIndex;
    //    fp = 0.1f;
    //}

    public WorkManager.Job StartJob()
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
        fp = 0.1f;  //reset slerp time

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

        WorkManager.TaskType currentType =  taskManagerBase.GetTaskType();

        currentCamTrans = GetCamPoint(currentType);
        currentWeaponTrans = GetSwordTeleportPoint(currentType);

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

    private void SlerpCamera()
    {
        if (Vector3.Distance(mainCam.transform.position, currentCamTrans.position) > 0.1f)
        {
            mainCam.transform.position = Vector3.Slerp(mainCam.transform.position, currentCamTrans.position, fp);
            fp += Time.deltaTime;
        }

        if (Quaternion.Angle(mainCam.transform.rotation, currentCamTrans.rotation) > 2)
        {
            mainCam.transform.rotation = Quaternion.Slerp(mainCam.transform.rotation, currentCamTrans.rotation, fp);
            fp += Time.deltaTime;
        }
    }

    private void SlerpWeapon()
    {
        if (Vector3.Distance(mainCam.transform.position, currentWeaponTrans.position) > 0.1f)
        {

        }

        if (Quaternion.Angle(mainCam.transform.rotation, currentWeaponTrans.rotation) > 2)
        {

        }
    }
}
