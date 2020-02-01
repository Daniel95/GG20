using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class TaskManagerBase : MonoBehaviour
{
    protected GameObject sword;
    protected bool active;
    public abstract int GetOffsetFromTarget();
    public abstract WorkManager.TaskType GetTaskType();

    private void Awake()
    {
        sword = GameObject.FindGameObjectWithTag("Sword");
    }

    public Transform GetSwordTeleportPoint()
    {
        List<SwordTeleportPoint> teleportPoints = FindObjectsOfType<SwordTeleportPoint>().ToList();
        SwordTeleportPoint swordTeleportPoint = teleportPoints.Find(x => x.taskType == GetTaskType());

        if (swordTeleportPoint == null)
        {
            Debug.LogError("Teleport point " + GetTaskType() + " does not exist!");
        }

        return swordTeleportPoint.transform;
    }

    public virtual void Activate()
    {
        Debug.Log("TELEPORT SWORD");
        active = true;
        StartCoroutine(LerpWeapon());
        //Transform swordTeleportPoint = GetSwordTeleportPoint();

        //sword.transform.position = swordTeleportPoint.position;
        //sword.transform.rotation = swordTeleportPoint.rotation;
    }

    public virtual void Deactivate()
    {
        active = false;
    }


    IEnumerator LerpWeapon()
    {
        float fp = 1;
        Transform swordTeleportPoint = GetSwordTeleportPoint();

        while (Quaternion.Angle(sword.transform.rotation, swordTeleportPoint.rotation) < 2)
        {
            Debug.Log("Lerping sword");
            sword.transform.position = Vector3.Slerp(sword.transform.position, swordTeleportPoint.transform.position, fp);
            sword.transform.rotation = Quaternion.Slerp(sword.transform.rotation, swordTeleportPoint.transform.rotation, fp);
            fp += Time.deltaTime;
        }

        yield return null;
    }
}
