using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "SharpeningTask", menuName = "ScriptableObjects/SharpeningTask", order = 1)]
public class SharpeningTaskScriptableObject : TaskScriptableObject
{
    [InfoBox("negative is thinner, 0 is without noise, positive is noise.")]
    [Range(-1.0f, 1.0f)] 
    public float targetSharpness = 0.0f;

    public override WorkManager.TaskType GetTaskType()
    {
        return WorkManager.TaskType.Sharpening;
    }
}