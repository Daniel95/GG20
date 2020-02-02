using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    private Player player;
    private void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        Player.EndJobEvent += EvaluateWeapon;
    }



    private void GiveJob()
    {
        Debug.Log("Start the job, lazybum");
        player.ReceiveJob(WorkManager.Instance.ChooseJob());    //give player random job
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
