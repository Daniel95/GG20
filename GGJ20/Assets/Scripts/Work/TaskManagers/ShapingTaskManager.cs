using Deform;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapingTaskManager : TaskManagerBase
{
    [SerializeField]
    private LayerMask layerMask = new LayerMask();
    [SerializeField]
    private float deformHitAmount = 0.1f;
    [SerializeField]
    private Vector2 bendinessMinMax = new Vector2(-0.5f, 0.5f);

    private ShapingTaskScriptableObject curTask = null;
    private Vector3 initialMouseLocation;
    private int deformRotation = 1;
    private float swordInitialX;
    public GameObject teleportObject;


    public override WorkManager.TaskType GetTaskType()
    {
        return WorkManager.TaskType.Shaping;
    }

    public override void Activate()
    {
        base.Activate();
        deformRotation = 1;
        swordInitialX = teleportObject.transform.position.x;
    }

    public override void Deactivate()
    {
        base.Deactivate();

        /*CurveDisplaceDeformer[] curveDisplaceDeformers = sword.GetComponentsInChildren<CurveDisplaceDeformer>();
        foreach (CurveDisplaceDeformer cdd in curveDisplaceDeformers)
        {
            if (cdd.TryGetComponent<Collider>(out Collider c))
            {
                c.enabled = false;
            }
        }*/
    }

    private void Update()
    {
        if (!isActivated)
        {
            return;
        }

        //TODO: Hammer swing functionality.

        /*if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 10.0f, layerMask))
            {
                if (hit.transform.TryGetComponent<CurveDisplaceDeformer>(out CurveDisplaceDeformer deformer))
                {
                    deformer.Factor -= deformHitAmount;
                }
            }
        }*/

        if (Input.GetKeyDown("space"))
        {
            float closestDistance = 1000000;
            CurveDisplaceDeformer closestDeformer = null;
            CurveDisplaceDeformer[] deformers = sword.GetComponentsInChildren<CurveDisplaceDeformer>();
            foreach (CurveDisplaceDeformer deformer in deformers)
            {
                float distance = Mathf.Abs(deformer.gameObject.transform.position.x - swordInitialX);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestDeformer = deformer;
                }
            }

            if (closestDeformer != null)
            {
                closestDeformer.Factor -= deformHitAmount * deformRotation;
            }
        }

        if (Input.GetKeyDown("x"))
        {
            sword.transform.Rotate(new Vector3(180,0,0));
            deformRotation *= -1;
        }


        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out RaycastHit hit, 10.0f, layerMask))
            {

                if (Input.GetMouseButtonDown(0))
                {
                    initialMouseLocation = hit.point;
                }

                hit.transform.gameObject.transform.position += new Vector3(hit.point.x - initialMouseLocation.x, 0, 0);
                initialMouseLocation = hit.point;
            }
        }
    }

    public override void SetTaskObject(TaskScriptableObject a_taskScriptableObject)
    {
        curTask = Instantiate(a_taskScriptableObject) as ShapingTaskScriptableObject;

        /*CurveDisplaceDeformer[] curveDisplaceDeformers = sword.GetComponentsInChildren<CurveDisplaceDeformer>();
        foreach (CurveDisplaceDeformer cdd in curveDisplaceDeformers)
        {
            if (cdd.TryGetComponent<Collider>(out Collider c))
            {
                c.enabled = true;
            }
        }*/
    }

    public override float GetOffsetPercentage()
    {
        return 0;
    }
}
