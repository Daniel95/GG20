using System;
using UnityEngine;
using System.Collections.Generic;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "JobLibrary", menuName = "ScriptableObjects/JobLibrary", order = 1)]
public class JobLibraryScriptableObject : ScriptableObject
{
    [ReorderableList]
    public List<WorkManager.Job> Jobs;
}