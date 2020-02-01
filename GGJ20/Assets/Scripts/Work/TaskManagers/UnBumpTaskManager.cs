using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnBumpTaskManager : TaskManagerBase
{
    private UnBumpTaskScriptableObject curTask = null;

    public override int GetOffsetFromTarget()
    {
        return 0;
    }

    public override WorkManager.TaskType GetTaskType()
    {
        return WorkManager.TaskType.UnBumping;
    }

    public override void Activate()
    {
        base.Activate();
    }

    public override void Deactivate()
    {
        base.Deactivate();
    }

    private void Update()
    {
        if(!isActivated)
        {
            return;
        }

        //Hammer func.
        //sword
        Debug.Log("Test " + curTask.hits.ToString());
    }

    public override void SetTaskObject(TaskScriptableObject a_taskScriptableObject)
    {
        curTask = Instantiate(a_taskScriptableObject) as UnBumpTaskScriptableObject;
    }
}
