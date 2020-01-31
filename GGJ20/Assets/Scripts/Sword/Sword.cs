using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private float shapingXScaleIncrement = 0.3f;

    private TaskManager.ShapingTask shapingTask;

    private int width;

    // Start is called before the first frame update
    void Start()
    {
        TaskManager.CreateTask(out shapingTask);

        if (shapingTask.TargetWidth > 5)
        {
            Debug.Log("I want a broad sword!");
        }
        else
        {
            Debug.Log("I want a thin sword!");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("Width offset: " + Mathf.Abs(shapingTask.TargetWidth - width));
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            width++;
            float newXScale = transform.localScale.x + shapingXScaleIncrement;
            transform.localScale = new Vector3(newXScale, transform.localScale.y, transform.localScale.z);
        }
    }
}