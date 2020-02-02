using UnityEngine;

[CreateAssetMenu(fileName = "ShapingTask", menuName = "ScriptableObjects/ShapingTask", order = 1)]
public class ShapingTaskScriptableObject : TaskScriptableObject
{
    [Range(0.0f, 1.0f)]
    public int TargetBendiness;

    public override WorkManager.TaskType GetTaskType()
    {
        return WorkManager.TaskType.Shaping;
    }
}