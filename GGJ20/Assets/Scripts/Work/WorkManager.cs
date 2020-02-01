using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;
using NaughtyAttributes;

public class WorkManager : MonoBehaviour
{
    public enum TaskType
    {
        None = -1,
        Shaping,
        Sharpening,
        Heating,
        UnBumping,
        Cleaning
    }

    [Serializable]
    public struct Job
    {
        public int Time;
        public string CustomerName;
        public GameObject CustomerModel;
        public GameObject Weapon;
        public string Description;
        [ReorderableList]
        public List<TaskScriptableObject> Tasks;
    }

    private static WorkManager instance;

    public static WorkManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<WorkManager>();
            }

            return instance;
        }
    }

    [SerializeField] private JobLibraryScriptableObject jobLibrary;

    public Job ChooseJob()
    {
        int randomIndex = UnityEngine.Random.Range(0, jobLibrary.Jobs.Count);
        Job job = jobLibrary.Jobs[randomIndex];

        //Debug.Log(job.Description);
        //Debug.Log(job.Time);

        return job;
    }
}
