using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    private void Awake()
    {
        Player.EndJobEvent += EvaluateWeapon;
    }

    private void EvaluateWeapon(Dictionary<WorkManager.TaskType, float> collection)
    {
        Debug.Log("Job compete!");
        foreach(KeyValuePair<WorkManager.TaskType, float> kv in collection)
        {
            Debug.Log("Task: " + kv.Key + " offset: " + kv.Value);
        }
    }
}
