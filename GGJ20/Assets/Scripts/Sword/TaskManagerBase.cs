using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class TaskManagerBase : MonoBehaviour
{
    public GameObject sword;
    public Sword swordDetails = null;
    protected bool isActivated;
    public abstract float GetOffsetFromTarget();
    public abstract WorkManager.TaskType GetTaskType();
    public abstract void SetTaskObject(TaskScriptableObject a_taskScriptableObject);

    private void Awake()
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
