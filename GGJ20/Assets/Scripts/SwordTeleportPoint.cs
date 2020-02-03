using UnityEngine;

public class SwordTeleportPoint : MonoBehaviour
{
    public WorkManager.TaskType taskType;
    [SerializeField]
    private float offset = 0.0f;

    public Vector3 GetPostion()
    {
        return this.transform.localPosition + (offset * this.transform.right);
    }

    private void OnValidate()
    {
        this.name = "TeleportPoint " + taskType.ToString();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(this.transform.localPosition, GetPostion());
    }
}
