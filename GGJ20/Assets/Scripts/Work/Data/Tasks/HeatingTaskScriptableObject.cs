using UnityEngine;

[CreateAssetMenu(fileName = "HeatingTask", menuName = "ScriptableObjects/HeatingTask", order = 1)]
public class HeatingTaskScriptableObject : TaskScriptableObject
{
    public int TargetHeat;
    public bool Any;

    public override WorkManager.TaskType GetTaskType()
    {
        return WorkManager.TaskType.Heating;
    }
}