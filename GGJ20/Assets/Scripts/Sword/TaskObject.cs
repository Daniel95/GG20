using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TaskObject : MonoBehaviour
{
    protected bool active;
    public abstract int GetOffsetFromTarget();

    public virtual void Activate()
    {
        active = true;
    }

    public virtual void Deactivate()
    {
        active = false;
    }
}
