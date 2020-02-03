using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class CamPoint : MonoBehaviour
{
    public WorkManager.TaskType taskType;

    private Vector3 lookAt = new Vector3();

    [Button("Update")]
    private void OnValidate()
    {
        this.name = "CameraPoint " + taskType.ToString();

        SwordTeleportPoint[] swordTeleportPoints = FindObjectsOfType<SwordTeleportPoint>();
        foreach (SwordTeleportPoint stp in swordTeleportPoints)
        {
            if(stp.taskType == taskType)
            {
                lookAt = stp.GetPostion();
                this.transform.LookAt(lookAt);
                break;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(this.transform.position, lookAt);
    }
}
