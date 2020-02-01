using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnBumpingTask", menuName = "ScriptableObjects/UnBumpingTask", order = 1)]
public class UnBumpTaskScriptableObject : TaskScriptableObject
{
    [Tooltip("0 is perfect everything else is bumpy! It's from all the ripple effects combined. (Amplitude)")]
    [Range(-10.0f, 10.0f)]
    public float totalTargetRippleness = 0.0f;

    public override WorkManager.TaskType GetTaskType()
    {
        return WorkManager.TaskType.UnBumping;
    }
}
