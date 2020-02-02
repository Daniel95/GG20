using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HeatingTaskManager : TaskManagerBase
{
    public static float HeatMultiplier;

    [SerializeField] private float shapingXScaleIncrement = 0.3f;
    [SerializeField] private Transform heatPoint;
    [SerializeField] private float maxHeat = 3;
    [SerializeField] private float timeForMaxHeat = 7;
    [SerializeField] private float moveTime = 1;

    private Transform nonHeatPoint;
    private Material swordMaterial;
    private float targetHeat;
    private float currentHeat;

    private bool heating;
    private bool heatMatters;

    public void SetTargetHeat(int _targetHeat)
    {
        targetHeat = _targetHeat;
    }

    public override void Activate()
    {
        base.Activate();
        List<SwordTeleportPoint> swordTeleportPoints = GameObject.FindObjectsOfType<SwordTeleportPoint>().ToList();
        nonHeatPoint = swordTeleportPoints.Find(x => x.taskType == WorkManager.TaskType.Heating).transform;

        swordMaterial = swordDetails.blade.GetComponentInChildren<MeshRenderer>().material;
    }

    public override void Deactivate()
    {
        base.Deactivate();
        targetHeat = 0;
        currentHeat = 0;
    }

    private void Update()
    {
        if(!isActivated) { return; }

        if (Input.GetMouseButtonDown(0))
        {
            sword.transform.DOMove(heatPoint.position, moveTime).OnComplete(() => heating = true);
        }
        else if(Input.GetMouseButtonUp(0))
        {
            heating = false;
            sword.transform.DOMove(nonHeatPoint.position, moveTime);


        }

        if (heating)
        {
            currentHeat += Mathf.Min((Time.deltaTime * maxHeat) / timeForMaxHeat, maxHeat);

            float heatProgress = currentHeat / maxHeat;

            swordMaterial.SetFloat("_HeatAmount", heatProgress);
            swordMaterial.SetFloat("_HeatEmission", heatProgress * 10);
        }
    }

    public override float GetOffsetPercentage()
    {
        if (heatMatters)
        {
            float maxOffset = targetHeat;

            if (maxHeat - targetHeat > maxOffset)
            {
                maxOffset = maxHeat - targetHeat;
            }

            float offset = currentHeat - targetHeat;
            float offsetPercentage = Mathf.Min(offset / maxOffset, 1);

            return offsetPercentage;
        }
        else
        {
            return 0;
        }
    }

    public override WorkManager.TaskType GetTaskType()
    {
        return WorkManager.TaskType.Heating;
    }

    public override void SetTaskObject(TaskScriptableObject a_taskScriptableObject)
    {
        var task = a_taskScriptableObject as HeatingTaskScriptableObject;
        targetHeat = task.targetHeatPercentage * maxHeat;
    }
}
