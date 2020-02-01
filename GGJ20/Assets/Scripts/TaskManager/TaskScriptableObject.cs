using UnityEngine;

[CreateAssetMenu(fileName = "ShapingTask", menuName = "ScriptableObjects/ShapingTask", order = 1)]
public abstract class TaskScriptableObject : ScriptableObject
{
    public abstract WorkManager.TaskType GetTaskType();
}