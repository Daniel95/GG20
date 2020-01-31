using System;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "JobLibrary", menuName = "ScriptableObjects/JobLibrary", order = 1)]
public class JobLibraryScriptableObject : ScriptableObject
{
    public List<TaskManager.Job> Jobs;
}