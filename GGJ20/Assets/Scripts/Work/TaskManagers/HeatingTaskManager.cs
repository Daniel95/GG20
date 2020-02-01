using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HeatingTaskManager : TaskManagerBase
{
    [SerializeField] private float shapingXScaleIncrement = 0.3f;
    [SerializeField] private Transform heatPoint;

    private Transform nonHeatPoint;
    private int targetHeat;
    private int currentHeat;

    private Coroutine slerpCoroutine = null;

    private bool down;

    private void Awake()
    {
        List<SwordTeleportPoint> swordTeleportPoints = GameObject.FindObjectsOfType<SwordTeleportPoint>().ToList();
        nonHeatPoint = swordTeleportPoints.Find(x => x.taskType == WorkManager.TaskType.Heating).transform;
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


        if (Input.GetMouseButton(0) && !down)
        {
            down = true;

            if (slerpCoroutine != null)
            {
                StopCoroutine(slerpCoroutine);
            }

            slerpCoroutine = StartCoroutine(SlerpTransform(sword.transform, heatPoint));
        }
        else if(down)
        {
            down = true;

            slerpCoroutine = StartCoroutine(SlerpTransform(sword.transform, nonHeatPoint));
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentHeat++;
            float newXScale = transform.localScale.x + shapingXScaleIncrement;
            transform.localScale = new Vector3(newXScale, transform.localScale.y, transform.localScale.z);
        }
    }

    public override float GetOffsetFromTarget()
    {
        return Mathf.Abs(targetHeat - currentHeat);
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

        if (OnCompleted != null)
        {
            OnCompleted();
        }
    }
}
