using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeatingTaskManager : TaskManagerBase
{
    [SerializeField] private float shapingXScaleIncrement = 0.3f;

    [SerializeField] private Text resultText;

    private int targetHeat;
    private int currentHeat;

    public void SetTargetHeat(int _targetHeat)
    {
        targetHeat = _targetHeat;
        Debug.Log("I AM THIS HOT: " + _targetHeat + " DEGREES KELVIN");
    }

    public override void Deactivate()
    {
        base.Deactivate();
        targetHeat = 0;
        currentHeat = 0;
    }

    private void Update()
    {
        if(!active) { return; }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentHeat++;
            float newXScale = transform.localScale.x + shapingXScaleIncrement;
            transform.localScale = new Vector3(newXScale, transform.localScale.y, transform.localScale.z);
        }
    }

    public override int GetOffsetFromTarget()
    {
        return Mathf.Abs(targetHeat - currentHeat);
    }

    public override WorkManager.TaskType GetTaskType()
    {
        return WorkManager.TaskType.Heating;
    }
}


//Weapon

    //heating func(temperature)
    //Sharpning funk (sharpness)
    //