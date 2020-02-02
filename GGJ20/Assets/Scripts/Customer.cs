using System;
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


    public static Action<string> ResultTextMadeEvent;

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
        player.RemoveSword();

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
        int i = 0;
        float f = 0;
        Debug.Log("Job compete!");
        foreach(KeyValuePair<WorkManager.TaskType, float> kv in collection)
        {
            f += kv.Value;
            //Debug.Log("Task: " + kv.Key + " offset: " + kv.Value);
            i++;
        }

        f = f / i;
        f = 1.0f / (i / f);

        Debug.Log("AAAAAAAA " + f);

        FindEnding(f);


        StartCoroutine(EvaluationTime());
    }

    IEnumerator EvaluationTime()
    {
        yield return new WaitForSeconds(1f);

        customerModel.transform.DOMove(new Vector3(customerModel.transform.position.x + 10, customerModel.transform.position.y, customerModel.transform.position.z), 2f).onComplete += SpawnCustomerModel;
    }

    private void FindEnding(float score)
    {
        string s = "wtf no score text?";
        if(score >= 0.75f && score <= 1f)
        {
            //very good
            s = currentJob.VeryGoodEnding;
        }
        else if(score >= 0.51f && score <= 0.74f)
        {
            //good
            s = currentJob.GoodEnding;
        }
        else if(score >= 0.25f && score <= 0.50f)
        {
            //bad
            s = currentJob.BadEnding;
        }
        else if(score >= 0.0f && score <= 0.25f)
        {
            //very bad
            s = currentJob.VeryBadEnding;
        }

        ResultTextMadeEvent?.Invoke(s);
    }
}
