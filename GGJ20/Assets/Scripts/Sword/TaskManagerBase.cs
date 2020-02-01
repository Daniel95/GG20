using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class TaskManagerBase : MonoBehaviour
{
    [HideInInspector]
    public GameObject sword;
    protected bool isActivated;
    public abstract int GetOffsetFromTarget();
    public abstract WorkManager.TaskType GetTaskType();
    public abstract void SetTaskObject(TaskScriptableObject a_taskScriptableObject);

    private void Awake()
    {
        sword = GameObject.FindGameObjectWithTag("Sword");
    }

    public virtual void Activate()
    {
        isActivated = true;
    }

    public virtual void Deactivate()
    {
        isActivated = false;
    }
}
