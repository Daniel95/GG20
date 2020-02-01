using UnityEngine;

public class SwordTeleportPoint : MonoBehaviour
{
    public WorkManager.TaskType taskType;

    private void OnValidate()
    {
        this.name = "TeleportPoint " + taskType.ToString();
    }
}
