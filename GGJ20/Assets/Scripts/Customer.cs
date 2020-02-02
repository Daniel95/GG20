using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Customer : MonoBehaviour
{
    private Player player;
    private WorkManager.Job currentJob;
    private GameObject customerModel = null;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        
        Player.EndJobEvent += EvaluateWeapon;
        SpawnCustomerModel();
    }

    private void GiveJob()
    {
        Debug.Log("Start the job, lazybum");
        player.ReceiveJob(currentJob);    //give player job
    }

    private void SpawnCustomerModel()
    {
        //a customer needs a job :)
        currentJob = WorkManager.Instance.ChooseJob();

        if (customerModel)
        {
            Destroy(customerModel);
            customerModel = null;
        }

        customerModel = Instantiate(currentJob.CustomerModel);
        customerModel.transform.position = new Vector3(transform.position.x, transform.position.y, 13f);
        customerModel.transform.DOMove(transform.position, 1f).onComplete += CustomerArrived;
    }

    /// <summary>
    /// This is almost completely redundant and I know I'm better than this
    /// </summary>
    private void CustomerArrived()
    {
        GiveJob();
        player.SpawnWeapon(currentJob);
    }

    private void EvaluateWeapon(Dictionary<WorkManager.TaskType, float> collection)
    {
        Debug.Log("Job compete!");
        foreach(KeyValuePair<WorkManager.TaskType, float> kv in collection)
        {
            Debug.Log("Task: " + kv.Key + " offset: " + kv.Value);
        }

        StartCoroutine(EvaluationTime());
        player.RemoveSword();
    }

    IEnumerator EvaluationTime()
    {
        yield return new WaitForSeconds(1f);

        customerModel.transform.DOMove(new Vector3(customerModel.transform.position.x + 10, customerModel.transform.position.y, customerModel.transform.position.z), 2f).onComplete += SpawnCustomerModel;
    }
}
