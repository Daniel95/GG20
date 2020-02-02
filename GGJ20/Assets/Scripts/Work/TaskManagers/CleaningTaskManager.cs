using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleaningTaskManager : TaskManagerBase
{
    private float targetCleanliness;
    public override float GetOffsetPercentage()
    {
        return 0;   
    }

    public override WorkManager.TaskType GetTaskType()
    {
        return WorkManager.TaskType.Cleaning;
    }

    public override void SetTaskObject(TaskScriptableObject a_taskScriptableObject)
    {
        var task = a_taskScriptableObject as CleaningTaskScriptableObject;
        targetCleanliness = task.TargetCleanliness;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActivated)
        {
            return;
        }
    }
}
