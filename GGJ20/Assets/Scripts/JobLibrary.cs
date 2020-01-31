using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobLibrary : MonoBehaviour
{
    [SerializeField] private List<Job> Jobs;

    [Serializable]
    public struct Job
    {
        public int Time;
        public string Description;
        public List<ScriptableObject> tasks;
    }
}
