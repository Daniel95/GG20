using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeatingSword : TaskObject
{
    [SerializeField] private float shapingXScaleIncrement = 0.3f;

    [SerializeField] private Text resultText;

    private int targetHeat;
    private int currentHeat;

    public void SetTargetHeat(int _targetHead)
    {
        targetHeat = _targetHead;
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
}