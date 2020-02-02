using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HeatingTaskManager : TaskManagerBase
{
    [SerializeField] private Transform heatPoint;
    [SerializeField] private float timeForMaxHeat = 7;
    [SerializeField] private float moveTime = 1;
    [SerializeField] private float meltSpeed = 0.15f;
    [SerializeField, Range(0, 1)] private float meltMax = 0.75f;

    private Transform nonHeatPoint;
    private float targetHeat;
    private float currentMelt = 0.0f;
    private bool heating;

    private AudioSource audioSource;

    [SerializeField]
    private AudioClip audioClip;

    public void SetTargetHeat(int _targetHeat)
    {
        targetHeat = _targetHeat;
    }

    public override void Activate()
    {
        base.Activate();
        List<SwordTeleportPoint> swordTeleportPoints = GameObject.FindObjectsOfType<SwordTeleportPoint>().ToList();
        nonHeatPoint = swordTeleportPoints.Find(x => x.taskType == WorkManager.TaskType.Heating).transform;
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClip;
    }

    public override void Deactivate()
    {
        base.Deactivate();
        audioSource.Stop();
        audioSource.loop = false;
        audioSource.clip = null;
        targetHeat = 0;
        currentMelt = 0.0f;
    }

    private void Update()
    {
        if(!isActivated) { return; }

        if (Input.GetMouseButtonDown(0))
        {
            sword.transform.DOMove(heatPoint.position, moveTime).OnComplete(() => heating = true);
            audioSource.Play();
            audioSource.loop = true;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            heating = false;
            sword.transform.DOMove(nonHeatPoint.position, moveTime);
            audioSource.Stop();
            audioSource.loop = false;

        }

        if (heating)
        {
            swordDetails.currentHeat += Mathf.Min((Time.deltaTime * swordDetails.maxHeat) / timeForMaxHeat, swordDetails.maxHeat);

            if(swordDetails.currentHeat >= swordDetails.maxHeat)
            {
                currentMelt += meltSpeed * Time.deltaTime;
                currentMelt = Mathf.Clamp(currentMelt, 0.0f, meltMax);
                swordDetails.material.SetFloat("_Cutoff", currentMelt);
            }
        }
    }

    public override float GetOffsetPercentage()
    {
        float progress = swordDetails.currentHeat / swordDetails.maxHeat;
        float offset = Mathf.Min(Mathf.Abs(targetHeat - progress), 1);
        return offset;
    }

    public override WorkManager.TaskType GetTaskType()
    {
        return WorkManager.TaskType.Heating;
    }

    public override void SetTaskObject(TaskScriptableObject a_taskScriptableObject)
    {
        var task = a_taskScriptableObject as HeatingTaskScriptableObject;
        targetHeat = task.targetHeatPercentage;
    }
}
