using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPoint : MonoBehaviour
{
    public WorkManager.TaskType taskType;

    private void OnValidate()
    {
        this.name = "CameraPoint " + taskType.ToString();
    }
}
