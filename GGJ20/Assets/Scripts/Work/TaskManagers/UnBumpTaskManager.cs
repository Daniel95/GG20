using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deform;

public class UnBumpTaskManager : TaskManagerBase
{
    [SerializeField]
    private LayerMask layerMask = new LayerMask();
    [SerializeField]
    private float deformHitAmount = 0.1f;
    [SerializeField]
    private Vector2 ripplenessMinMax = new Vector2(-0.5f, 0.5f);

    private UnBumpTaskScriptableObject curTask = null;

    public override float GetOffsetPercentage()
    {
        return 0;
    }

    public override WorkManager.TaskType GetTaskType()
    {
        return WorkManager.TaskType.UnBumping;
    }

    public override void Activate()
    {
        base.Activate();
    }

    public override void Deactivate()
    {
        base.Deactivate();

        RippleDeformer[] rippleDeformers = sword.GetComponentsInChildren<RippleDeformer>();
        foreach (RippleDeformer rd in rippleDeformers)
        {
            if (rd.TryGetComponent<Collider>(out Collider c))
            {
                c.enabled = false;
            }
        }
    }

    private void Update()
    {
        if(!isActivated)
        {
            return;
        }

        //TODO: Hammer swing functionality.
        
        if(Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 10.0f, layerMask))
            {
                if(hit.transform.TryGetComponent<RippleDeformer>(out RippleDeformer deformer))
                {
                    //Hit ripple.
                    deformer.Amplitude -= deformHitAmount;
                    deformer.Amplitude = Mathf.Clamp(deformer.Amplitude, ripplenessMinMax.x, ripplenessMinMax.y);
                }
            }
        }
    }

    public override void SetTaskObject(TaskScriptableObject a_taskScriptableObject)
    {
        curTask = Instantiate(a_taskScriptableObject) as UnBumpTaskScriptableObject;

        RippleDeformer[] rippleDeformers = sword.GetComponentsInChildren<RippleDeformer>();
        foreach (RippleDeformer rd in rippleDeformers)
        {
            if(rd.TryGetComponent<Collider>(out Collider c))
            {
                c.enabled = true;
            }
        }
    }
}
