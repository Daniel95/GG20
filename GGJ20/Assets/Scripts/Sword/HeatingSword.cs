using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeatingSword : TaskObject
{
    [SerializeField] private float shapingXScaleIncrement = 0.3f;

    [SerializeField] private Text resultText;

    private int targetWidth;
    private int currentWidth;

    private void Update()
    {
        if(!active) { return; }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentWidth++;
            float newXScale = transform.localScale.x + shapingXScaleIncrement;
            transform.localScale = new Vector3(newXScale, transform.localScale.y, transform.localScale.z);
        }
    }

    public override int GetOffsetFromTarget()
    {
        return Mathf.Abs(targetWidth - currentWidth);
    }
}