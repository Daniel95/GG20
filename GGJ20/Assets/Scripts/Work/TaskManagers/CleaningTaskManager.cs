﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleaningTaskManager : TaskManagerBase
{
    private float targetCleanliness;

    private float currentCleanliness;

    private float maxCleanliness = 5f;

    private Vector3 previousMousePosition = Vector3.negativeInfinity;


    private RandomPitchPlayer pitchPlayer;

    [SerializeField]
    private AudioClip cleaningClip;


    [Header("Particle settings")]

    private ParticleSystem shineParticleSystem;

    private float currentSpawningAnmount;

    [SerializeField, Tooltip("The maximum anmount of particles that can appear. Should be about equal to the 'max particles' variable in the particle system.")]
    private float maxSpawnAnmount = 30;

    [SerializeField, Tooltip("The anmount of particles that get added by cleanliness. It will never exceed the maximum anmount.")]
    private float cleanlinessToParticleRate = 0.5F;

    [Space]

    [Header("Rubbing input settings")]

    [SerializeField, Tooltip("The least required anmount of distance between the position for the game for it to consider the player to be rubbing.")]
    private float rubbingTreshold = 5F;

    private float mousePositionDelta;

    public override float GetOffsetPercentage()
    {
        float currentPercentage = 1.0f / (maxCleanliness / currentCleanliness);
        float rawOffset = Mathf.Abs(targetCleanliness - currentPercentage); //target cleanliness is already between 0-1
        Debug.Log("Target cleanliness Clean = " + targetCleanliness + ". Current cleanliness = " + currentPercentage + ". Offset = " + rawOffset);
        return rawOffset;
    }

    public override WorkManager.TaskType GetTaskType()
    {
        return WorkManager.TaskType.Cleaning;
    }

    public override void SetTaskObject(TaskScriptableObject a_taskScriptableObject)
    {
        var task = a_taskScriptableObject as CleaningTaskScriptableObject;
        targetCleanliness = task.TargetCleanliness;
    }

    public override void Deactivate()
    {
        base.Deactivate();
        targetCleanliness = 0;
        currentCleanliness = 0;
        //shineParticleSystem.emissionRate = 0;
        previousMousePosition = Vector3.negativeInfinity;
    }

    private void RubDetected()
    {
        if (shineParticleSystem.emissionRate < maxSpawnAnmount)
        {
            shineParticleSystem.emissionRate += cleanlinessToParticleRate * Time.deltaTime;
            if(Random.Range(0,100)>97)
            { 
                pitchPlayer.PlaySFX(cleaningClip, 0.7F, 0.9F);
            }
            currentCleanliness += Time.deltaTime;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActivated)
        {
            return;
        }

        if (Input.GetMouseButton(0))
        {
            if(previousMousePosition!=Vector3.negativeInfinity)
            {
                mousePositionDelta = Vector2.Distance(Input.mousePosition, previousMousePosition);
                if(mousePositionDelta>rubbingTreshold)
                {
                    RubDetected();
                }
            }
            previousMousePosition = Input.mousePosition;
        }
    }

    public override void Activate()
    {
        base.Activate();
        shineParticleSystem = swordDetails.shinyParticles;
        shineParticleSystem.Play();
        shineParticleSystem.emissionRate = 0;
        pitchPlayer = GetComponent<RandomPitchPlayer>();
    }
}
