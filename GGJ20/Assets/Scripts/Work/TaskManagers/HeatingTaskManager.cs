using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HeatingTaskManager : TaskManagerBase
{
    public static float HeatMultiplier;

    [SerializeField] private float shapingXScaleIncrement = 0.3f;
    [SerializeField] private Transform heatPoint;
    [SerializeField] private Color maxHeatColor;
    [SerializeField] private float maxHeatMultiplier = 3;
    [SerializeField] private float timeForMaxHeat = 7;

    private Transform nonHeatPoint;
    private Material swordMaterial;
    private int targetHeat;
    private int currentHeat;

    private Coroutine slerpCoroutine = null;

    private bool heating;
    private bool heatMatters;

    private void Start()
    {
        List<SwordTeleportPoint> swordTeleportPoints = GameObject.FindObjectsOfType<SwordTeleportPoint>().ToList();
        nonHeatPoint = swordTeleportPoints.Find(x => x.taskType == WorkManager.TaskType.Heating).transform;

        swordMaterial = sword.GetComponent<Material>();
    }

    public void SetTargetHeat(int _targetHeat)
    {
        targetHeat = _targetHeat;
    }

    public override void Deactivate()
    {
        base.Deactivate();
        targetHeat = 0;
        currentHeat = 0;
    }

    private void Update()
    {
        if(!isActivated) { return; }

        /*
        if (Input.GetMouseButtonDown(0))
        {
            if (slerpCoroutine != null)
            {
                StopCoroutine(slerpCoroutine);
            }

            slerpCoroutine = StartCoroutine(SlerpTransform(sword.transform, heatPoint, () => heating = true));
        }
        else if(Input.GetMouseButtonUp(0))
        {
            if (slerpCoroutine != null)
            {
                StopCoroutine(slerpCoroutine);
            }

            slerpCoroutine = StartCoroutine(SlerpTransform(sword.transform, nonHeatPoint, () => heating = false));
        }

        if (heating)
        {
            currentHeat++;

            swordMaterial.color = Color.Lerp(swordMaterial.color, maxHeatColor, 1);
        }
         */
    }

    public override float GetOffsetFromTarget()
    {
        if (heatMatters)
        {
            return Mathf.Abs(targetHeat - currentHeat);
        }
        else
        {
            return 0;
        }
    }

    public override WorkManager.TaskType GetTaskType()
    {
        return WorkManager.TaskType.Heating;
    }

    public override void SetTaskObject(TaskScriptableObject a_taskScriptableObject)
    {
        var task = a_taskScriptableObject as HeatingTaskScriptableObject;
        targetHeat = task.TargetHeat;
    }
    private IEnumerator SlerpTransform(Transform transformToMove,
        Transform targetTransform,
        Action OnCompleted = null,
        float minDistanceOffset = 0.2f,
        float minRotationOffset = 5.0f)
    {
        float fp = 0;

        while (true)
        {
            float positionOffset = Vector3.Distance(transformToMove.transform.position, targetTransform.position);
            float angleOffset = Quaternion.Angle(transformToMove.transform.rotation, targetTransform.rotation);

            bool reachedPosition = positionOffset <= minDistanceOffset;
            bool reachedRotation = angleOffset <= minRotationOffset;

            if (reachedPosition && reachedRotation)
            {
                break;
            }

            transformToMove.transform.position = Vector3.Slerp(transformToMove.transform.position, targetTransform.position, fp);
            transformToMove.transform.rotation = Quaternion.Slerp(transformToMove.transform.rotation, targetTransform.rotation, fp);
            fp += Time.deltaTime;

            yield return null;
        }

        print("Done");

        if (OnCompleted != null)
        {
            OnCompleted();
        }
    }
}
