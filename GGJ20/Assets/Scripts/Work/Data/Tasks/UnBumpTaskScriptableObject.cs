using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnBumpingTask", menuName = "ScriptableObjects/UnBumpingTask", order = 1)]
public class UnBumpTaskScriptableObject : TaskScriptableObject
{
    public int hits = 5;

    public override WorkManager.TaskType GetTaskType()
    {
        return WorkManager.TaskType.UnBumping;
    }
}
