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

    private void EvaluateWeapon(WorkManager.Job job, Dictionary<WorkManager.TaskType, float> results)
    {

        float totalResults = results.Count;

        foreach (WorkManager.TaskType taskType in results.Keys)
        {
            print(taskType);
        }

        foreach (float offset in results.Values)
        {
            print(offset);
            totalResults -= offset;
        }

        float score = totalResults / results.Count;

        float grade = score * 10;

        string message = (int)grade + "/10";

        string ending = GetEnding(score) + " " + message;

        ResultTextMadeEvent?.Invoke(ending);
        StartCoroutine(EvaluationTime());
    }

    IEnumerator EvaluationTime()
    {
        yield return new WaitForSeconds(10f);

        customerModel.transform.DOMove(new Vector3(customerModel.transform.position.x + 10, customerModel.transform.position.y, customerModel.transform.position.z), 2f).onComplete += SpawnCustomerModel;
    }

    private string GetEnding(float score)
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

        return s;
    }
}
