using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseWatcher : MonoBehaviour
{
    [ReorderableList]
    public List<GameObject> cameraHooks;
    [SerializeField]
    private GameObject mainCam = null;

    private int prevHookIndx = 0;
    private int currHookIndx = 0;

    float fp = 1;

    private void Start()
    {
        currHookIndx = 0;
        prevHookIndx = cameraHooks.Count;
        if (cameraHooks.Count <= 0)
        {
            Debug.LogError("define camera hooks, you buffoon");
        }
        mainCam.transform.position = cameraHooks[0].transform.position;
        mainCam.transform.rotation = cameraHooks[0].transform.rotation;
    }

    void Update()
    {
        if(Vector3.Distance(mainCam.transform.position, cameraHooks[currHookIndx].transform.position) > 0.1f)
        {
            mainCam.transform.position = Vector3.Slerp(cameraHooks[prevHookIndx].transform.position, cameraHooks[currHookIndx].transform.position, fp);
            fp += Time.deltaTime;
        }

        if(Quaternion.Angle(mainCam.transform.rotation, cameraHooks[currHookIndx].transform.rotation) > 2)
        {
            mainCam.transform.rotation = Quaternion.Slerp(cameraHooks[prevHookIndx].transform.rotation, cameraHooks[currHookIndx].transform.rotation, fp);
            fp += Time.deltaTime;
        }
    }

    public void NextPhase()
    {
        Debug.Log("Next Phase");
        if((currHookIndx + 1) < cameraHooks.Count)
        {
            //safe
            GoToPhase(currHookIndx+1);
            fp = 0.1f;
        }
        else
        {
            //wrap to first phase
            GoToPhase(0);
            fp = 0.1f;
        }
    }

    public void GoToPhase(int phaseIndex)
    {
        prevHookIndx = currHookIndx;
        currHookIndx = phaseIndex;

        cameraHooks[prevHookIndx].GetComponentInParent<Task>().EndTask();
        cameraHooks[currHookIndx].GetComponentInParent<Task>().StartTask();
    }
}
