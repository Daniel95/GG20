using UnityEngine;

[CreateAssetMenu(fileName = "CleaningTask", menuName = "ScriptableObjects/CleaningTask", order = 1)]
public class CleaningTaskScriptableObject : TaskScriptableObject
{
    [Range(0.0f, 1.0f)] 
    public float TargetCleanliness;

    public override WorkManager.TaskType GetTaskType()
    {
        return WorkManager.TaskType.Cleaning;
    }
}