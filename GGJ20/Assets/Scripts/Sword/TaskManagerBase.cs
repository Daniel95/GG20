using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class TaskManagerBase : MonoBehaviour
{
    [HideInInspector]
    public GameObject sword;
    [HideInInspector]
    public Sword swordDetails = null;
    protected bool isActivated;
    public abstract float GetOffsetPercentage();
    public abstract WorkManager.TaskType GetTaskType();
    public abstract void SetTaskObject(TaskScriptableObject a_taskScriptableObject);

    protected virtual void Awake()
    {
        sword = GameObject.FindGameObjectWithTag("Sword");
        swordDetails = sword.GetComponent<Sword>();
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
