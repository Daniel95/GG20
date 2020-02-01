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

    private float headedFactor;

    private float time = 1.1F;

    [SerializeField, Tooltip("To play the hitting sound at random pitches.")]
    private RandomPitchPlayer pitchPlayer;

    [SerializeField]
    private AudioClip audioClip;

    void Awake()
    {
        deformer = GetComponentInChildren<RadialCurveDeformer>();
        myFactor = deformer.Factor;
    }

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
            pitchPlayer.PlaySFX(audioClip, 0.75F, 0.9F);

        }
        else if (Input.GetKeyDown(KeyCode.D) && time > 1)
        {
            time = 0;
            myFactor = deformer.Factor;
            headedFactor = myFactor + factorShift;
            pitchPlayer.PlaySFX(audioClip, 0.95F, 1.25F);
        }
    }
}
