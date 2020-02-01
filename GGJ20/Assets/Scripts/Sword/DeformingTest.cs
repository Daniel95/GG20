using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deform;

public class DeformingTest : MonoBehaviour
{

    private RadialCurveDeformer deformer;

    private float myFactor;

    [SerializeField, Tooltip("How much the deformity of the sword 'changes' every time.")]
    private float factorShift = 0.1F;

    [SerializeField, Tooltip("How fast the 'animation' is for bending the sword. Higher number = faster animation!")]
    private float bendingSpeed = 0.5F;

    private float prefferedFactor = 0;

    private float headedFactor;

    private float time = 1.1F;

    void Awake()
    {
        deformer = GetComponentInChildren<RadialCurveDeformer>();
        myFactor = deformer.Factor;
    }

    // Update is called once per frame
    void Update()
    {
        if (time < 1)
        {
            deformer.Factor = Mathf.Lerp(myFactor, headedFactor, time);
            time += bendingSpeed * Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.A)&&time>1)
        {
            time = 0;
            myFactor = deformer.Factor;
            headedFactor = myFactor - factorShift;

        }
        else if (Input.GetKeyDown(KeyCode.D) && time > 1)
        {
            time = 0;
            myFactor = deformer.Factor;
            headedFactor = myFactor + factorShift;

        }
    }
}
