using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public ParticleSystem shinyParticles = null;
    public Transform blade = null;
    public Material material = null;
    public float cooldownSpeed = 0.075f;
    public float currentHeat = 0.0f;
    public float maxHeat = 3.0f;

    private void Awake()
    {
        material = blade.GetComponentInChildren<MeshRenderer>().material;
    }

    private void Update()
    {
        float heatProgress = currentHeat / maxHeat;

        //Cool down.
        if(currentHeat > 0.0f)
        {
            currentHeat = Mathf.Max(currentHeat - (cooldownSpeed * Time.deltaTime), 0);
        }

        //Update shader.
        material.SetFloat("_HeatAmount", heatProgress);
        material.SetFloat("_HeatEmission", heatProgress * 10);
    }
}
