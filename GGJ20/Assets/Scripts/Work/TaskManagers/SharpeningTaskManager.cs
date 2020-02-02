using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deform;
using DG.Tweening;
public class SharpeningTaskManager : TaskManagerBase
{
    private SharpeningTaskScriptableObject curTask = null;
    private PerlinNoiseDeformer deformer = null;
    [SerializeField]
    private float deformSharpingAmount = 0.25f;

    private Transform startTransform = null;
    [SerializeField]
    private Vector3 localOffset = new Vector3(0, 0.5f, 0);

    private AudioSource audioSource;

    [SerializeField]
    private AudioClip audioClip;


    public override void Activate()
    {
        base.Activate();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClip;
    }

    public override void Deactivate()
    {
        base.Deactivate();
        audioSource.Stop();
        audioSource.loop = false;
        audioSource.clip = null;
    }

    protected void Awake()
    {
        SwordTeleportPoint[] swordTeleportPoints = GameObject.FindObjectsOfType<SwordTeleportPoint>();
        foreach (SwordTeleportPoint p in swordTeleportPoints)
        {
            if(p.taskType == this.GetTaskType())
            {
                startTransform = p.transform;
                break;
            }
        }
    }

    public override float GetOffsetPercentage()
    {
        float magnitude = deformer.MagnitudeScalar * 4;
        float invScale = 1 - swordDetails.blade.transform.localScale.z;

        float offset = magnitude - invScale;

        float offsetToTarget = 1 - Mathf.Max(Mathf.Abs(offset - curTask.targetSharpness));

        return offsetToTarget;
    }

    public override WorkManager.TaskType GetTaskType()
    {
        return WorkManager.TaskType.Sharpening;
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

        if (Input.GetMouseButtonDown(0))
        {
            //Go down.
            sword.transform.DOLocalMove(startTransform.position - localOffset, 0.25f);
            audioSource.Play();
            audioSource.loop = true;
        }

        if(Input.GetMouseButtonUp(0))
        {
            //Go up.
            sword.transform.DOLocalMove(startTransform.position, 0.25f);
            audioSource.Stop();
            audioSource.loop = false;
        }

        if (Input.GetMouseButton(0))
        {
            //Deform logic.
            deformer.MagnitudeScalar -= deformSharpingAmount * Time.deltaTime;
            deformer.MagnitudeScalar = Mathf.Max(deformer.MagnitudeScalar, 0.0f);

            if(deformer.MagnitudeScalar <= 0.0f && swordDetails.blade.localScale.z >= 0.0f)
            {
                swordDetails.blade.localScale -= new UnityEngine.Vector3(0, 0, deformSharpingAmount) * Time.deltaTime;
            }
        }
    }
}
