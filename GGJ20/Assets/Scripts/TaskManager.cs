using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Random = UnityEngine.Random;

public class TaskManager : MonoBehaviour
{
    private static TaskManager instance;

    public static TaskManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<TaskManager>();
            }

            return instance;
        }
    }

    [Category("Shaping")]
    [SerializeField] [Range(1, 10)] private int MinTargetWidth;
    [SerializeField] [Range(1, 10)] private int MaxTargetWidth;

    [Category("Sharpening")]
    [SerializeField] [Range(1, 10)] private int MinTargetSharpness;
    [SerializeField] [Range(1, 10)] private int MaxTargetSharpness;

    public static void CreateTask(out ShapingTask shapingTask)
    {
        shapingTask = new ShapingTask();
        shapingTask.TargetWidth = Random.Range(Instance.MinTargetWidth, Instance.MaxTargetWidth);
    }

    public static void CreateTask(out SharpeningTask sharpeningTask)
    {
        sharpeningTask = new SharpeningTask();
        sharpeningTask.TargetSharpness = Random.Range(Instance.MinTargetSharpness, Instance.MaxTargetSharpness);
    }

    public struct ShapingTask
    {
        public int TargetWidth;
    }

    public struct SharpeningTask
    {
        public int TargetSharpness;
    }
}
