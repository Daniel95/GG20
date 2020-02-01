using UnityEngine;

[CreateAssetMenu(fileName = "ShapingTask", menuName = "ScriptableObjects/ShapingTask", order = 1)]
public class ShapingTaskScriptableObject : TaskScriptableObject
{
    public int TargetWidth;

    public override WorkManager.TaskType GetTaskType()
    {
        return WorkManager.TaskType.Sharpening;
    }
}