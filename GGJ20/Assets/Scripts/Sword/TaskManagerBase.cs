using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class TaskManagerBase : MonoBehaviour
{
    protected GameObject sword;
    protected bool active;
    public abstract int GetOffsetFromTarget();
    public abstract WorkManager.TaskType GetTaskType();

    private void Awake()
    {
        sword = GameObject.FindGameObjectWithTag("Sword");
    }

    public Transform GetSwordTeleportPoint()
    {
        List<SwordTeleportPoint> teleportPoints = FindObjectsOfType<SwordTeleportPoint>().ToList();
        SwordTeleportPoint swordTeleportPoint = teleportPoints.Find(x => x.taskType == GetTaskType());

        if (swordTeleportPoint == null)
        {
            Debug.LogError("Teleport point " + GetTaskType() + " does not exist!");
        }

        return swordTeleportPoint.transform;
    }

    public virtual void Activate()
    {
        active = true;

        Transform swordTeleportPoint = GetSwordTeleportPoint();

        sword.transform.position = swordTeleportPoint.position;
        sword.transform.rotation = swordTeleportPoint.rotation;
    }

    public virtual void Deactivate()
    {
        active = false;
    }
}
