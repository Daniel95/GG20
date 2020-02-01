using UnityEngine;

[CreateAssetMenu(fileName = "SharpeningTask", menuName = "ScriptableObjects/SharpeningTask", order = 1)]
public class SharpeningTaskScriptableObject : TaskScriptableObject
{
    public int TargetSharpness;

    public override WorkManager.TaskType GetTaskType()
    {
        return WorkManager.TaskType.Sharpening;
    }
}