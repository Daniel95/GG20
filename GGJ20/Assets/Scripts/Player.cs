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
    private Customer customer;

    private List<TaskManagerBase> taskManagers;
    private Dictionary<WorkManager.TaskType, float> weaponResultOffsets = new Dictionary<WorkManager.TaskType, float>();

    /// <summary>
    /// Params: Job
    /// </summary>
    public static Action<WorkManager.Job> StartJobEvent;
    public static Action<WorkManager.Job> GetJobEvent;
    public static Action NextCustomerEvent;
    /// <summary>
    /// Params: Job
    /// </summary>
    public static Action<WorkManager.Job, Dictionary<WorkManager.TaskType, float>> EndJobEvent;
    /// <summary>
    /// Params: WorkManager.TaskType
    /// </summary>
    public static Action<WorkManager.TaskType> StartTaskEvent;

    private WorkManager.Job job;
    private int taskIndex = 0;
    private WorkManager.TaskType currentTaskType = WorkManager.TaskType.None;
    private TaskManagerBase taskManagerBase;

    private Coroutine cameraSlerpCoroutine;
    private Coroutine swordSlerpCoroutine;

    private bool startedJob;
    private bool gotJob;    //got job you pauper???

    public bool IsWorking() { return currentTaskType != WorkManager.TaskType.None; }
    public bool IsAtCounter() { return currentTaskType == WorkManager.TaskType.None; }

    private void Awake()
    {
        //TimerScript.TimeExpiredEvent += GoToCounter;      //Doesnt quite work

        GetLerpPoints();

        //stuff for camera slerping
        mainCam = GameObject.FindWithTag("MainCamera");
        customer = GameObject.FindWithTag("Customer").GetComponent<Customer>();

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
        if(!gotJob)
        {
            Debug.Log("You dont have a job yet you eager little boy");
            return;
        }

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

        gotJob = false;
        //Destroy(sword, 5f);
        EndJobEvent?.Invoke(job, weaponResultOffsets);   //wow
        //sword = null;
    }

    public void StartJob()
    {
        //Debug.Log("Start the job, lazybum");
        //job = WorkManager.Instance.ChooseJob();

        if (StartJobEvent != null)
        {
            StartJobEvent(job);
        }
        
        weaponResultOffsets.Clear();

        taskIndex = 0;
        bool startedTask = NextTask();
    }

    public bool NextTask()
    {
        if (taskManagerBase != null)
        {
            //store offset
            StoreTaskOffset(taskManagerBase, weaponResultOffsets);
            taskManagerBase.Deactivate();
        }

        if (taskIndex >= job.Tasks.Count)
        {
            return false;
        }


        StartTask(taskIndex);
        taskIndex++;
        return true;
    }

    private void StartTask(int taskIndex)
    {
        TaskScriptableObject taskData = job.Tasks[taskIndex];
        currentTaskType = taskData.GetTaskType();

        if(StartTaskEvent != null)
            StartTaskEvent(currentTaskType);

        if (!taskManagers.Exists(x => x.GetTaskType() == currentTaskType))
        {
            Debug.LogError("VERY BAD");
            return;
        }

        taskManagerBase = taskManagers.Find(x => x.GetTaskType() == currentTaskType);

        if (!taskManagerBase.sword)
        {
            taskManagerBase.sword = sword;
            taskManagerBase.swordDetails= sword.GetComponent<Sword>();
        }

        taskManagerBase.Activate();

        Transform currentCamTrans = GetCamPoint(currentTaskType);
        Transform currentWeaponTrans = GetSwordTeleportPoint(currentTaskType);

        SlerpCameraAndSword(currentCamTrans, currentWeaponTrans);

        taskManagerBase.SetTaskObject(taskData);
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
        float fp = 0;

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

    private void StoreTaskOffset(TaskManagerBase task, Dictionary<WorkManager.TaskType, float> container)
    {
        if (!container.ContainsKey(task.GetTaskType()))
        {
            //key was not found before, just add
            container.Add(task.GetTaskType(), task.GetOffsetPercentage());
        }
        else
        {
            //the same task has already occurred in this job. So instead add the 2 results & clamp
            float newOffset = task.GetOffsetPercentage() + container[task.GetTaskType()];
            newOffset = Mathf.Clamp(newOffset, -1, 1);
            container[task.GetTaskType()] = newOffset;
        }
    }

    public void ReceiveJob(WorkManager.Job j)
    {
        job = j;
    }

    public void SpawnWeapon(WorkManager.Job j)
    {
        sword = Instantiate(j.Weapon);
        sword.transform.position = GetSwordTeleportPoint(WorkManager.TaskType.None).position;
        sword.transform.rotation = GetSwordTeleportPoint(WorkManager.TaskType.None).rotation;
        GetJobEvent?.Invoke(j);
        gotJob = true;
    }

    public void RemoveSword()
    {
        NextCustomerEvent?.Invoke();
        Destroy(sword);
    }
}
