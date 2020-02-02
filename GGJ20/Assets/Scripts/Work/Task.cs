using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task : MonoBehaviour
{

    /// <summary>
    /// Mark this task as in progress, start animation etc.
    /// </summary>
    public void StartTask()
    {
        Debug.Log("Starting task: " + gameObject.name);
    }

    public void EndTask()
    {
        Debug.Log("Ending task: " + gameObject.name);
    }

}
