using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deform;

public class SharpeningTaskManager : TaskManagerBase
{
    private SharpeningTaskScriptableObject curTask = null;
    private PerlinNoiseDeformer deformer = null;
    [SerializeField]
    private float deformSharpingAmount = 0.01f;

    public override float GetOffsetFromTarget()
    {
        return 0;
    }

    public override WorkManager.TaskType GetTaskType()
    {
        return WorkManager.TaskType.Sharpening;
    }

    public override void Activate()
    {
        base.Activate();
    }

    public override void Deactivate()
    {
        base.Deactivate();
    }

    public override void SetTaskObject(TaskScriptableObject a_taskScriptableObject)
    {
        curTask = Instantiate(a_taskScriptableObject) as SharpeningTaskScriptableObject;

        deformer = sword.GetComponentInChildren<PerlinNoiseDeformer>();
    }

    private void Update()
    {
        if (!isActivated)
        {
            return;
        }

        if (Input.GetMouseButton(0))
        {
            deformer.MagnitudeScalar -= deformSharpingAmount * Time.deltaTime;
            deformer.MagnitudeScalar = Mathf.Max(deformer.MagnitudeScalar, 0.0f);

            if(deformer.MagnitudeScalar <= 0.0f && sword.transform.localScale.z >= 0.0f)
            {   
                //TODO: get blade.
                sword.transform.localScale -= new UnityEngine.Vector3(0, 0, deformSharpingAmount) * Time.deltaTime;
            }
        }
    }
}
