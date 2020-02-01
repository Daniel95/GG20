using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class AudioController : MonoBehaviour
{

    public float timer = 78;
    public float halfMeasureLength = 8;
    public int numberOfLayers = 4;
    public float defaultVolume = 0.75f;

    public List<AudioList> audioClipsNormal = new List<AudioList>();
    public List<AudioList> audioClipsFinal = new List<AudioList>();

    private int currentStage = 0;
    private float startTime;
    private int currentLoop;
    private bool initialized;
    private List<AudioSource> audioSources = new List<AudioSource>();
    
    

    void Start()
    {
        Player.StartJobEvent += StartMusic;
        Player.StartTaskEvent += NextStage;
        initialized = false;

        for (int i = 0; i < numberOfLayers; i++)
        {
            audioSources.Add(gameObject.AddComponent<AudioSource>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!initialized)
        {
            return;
        }

        float currentTime = Time.realtimeSinceStartup - startTime;

        if (currentTime >= timer)
        {
            initialized = false;
            //Debug.Log("Loop " + (currentLoop + 1) + ": finished (" + Mathf.FloorToInt(currentTime) + "s)");
            return;
        }

        if (Input.GetKeyDown("e"))
        {
            //NextStage();
        }
        else if (Input.GetKeyDown("q"))
        {
            PreviousStage();
        }
        else if (Mathf.FloorToInt(currentTime / halfMeasureLength) > currentLoop)
        {
            currentLoop++;
            InitializeAudioSources(ChooseClipsToPlay(false), false);
        }

    }

    /// <summary>
    /// Start the dynamic music based on the task timer ... the timer length needs to be a multiple of 8
    /// </summary>
    /// <param name="timerLength"> The length of time for which the music should play based on the task timer</param>
    public void StartMusic(WorkManager.Job job)
    {
        float timerLength = job.Time;
        if (timerLength % halfMeasureLength != 0)
        {
            Debug.LogWarning("The timer length should be a multiple of " + halfMeasureLength + "!");
        }
        timer = timerLength - (timerLength % halfMeasureLength);

        startTime = Time.realtimeSinceStartup;
        currentLoop = -1;
        initialized = true;
        
    }

    /// <summary>
    /// Move the music to the next stage which results in more instruments being added
    /// </summary>
    public void NextStage(WorkManager.TaskType type)
    {
        //type;
        float currentTime = Time.realtimeSinceStartup - startTime;
        if (Mathf.FloorToInt(currentTime / halfMeasureLength) > currentLoop)
        {
            currentLoop++;
        }

        currentStage++;

        InitializeAudioSources(ChooseClipsToPlay(true), true);
    }

    /// <summary>
    /// Move the music back to a previous stage, which results in less instruments playing
    /// </summary>
    public void PreviousStage()
    {
        float currentTime = Time.realtimeSinceStartup - startTime;
        if (Mathf.FloorToInt(currentTime / halfMeasureLength) > currentLoop)
        {
            currentLoop++;
        }

        currentStage = Mathf.Max(currentStage - 1, 0);

        InitializeAudioSources(ChooseClipsToPlay(true), true);
    }

    private List<AudioList> ChooseClipsToPlay(bool startFromADifferentPosition)
    {
        float currentTime = Time.realtimeSinceStartup - startTime;

        if (currentTime > timer - halfMeasureLength * 2)
        {
            if (currentTime < timer - halfMeasureLength || startFromADifferentPosition)
            {
                //Debug.Log("Loop " + currentLoop.ToString() + ": play final (" + Mathf.FloorToInt(currentTime) + "s)");
                return audioClipsFinal;
            }
        }
        else if (currentLoop % 2 == 0 || startFromADifferentPosition)
        {
            //Debug.Log("Loop " + currentLoop.ToString() + ": play normal (" + Mathf.FloorToInt(currentTime) + "s)");
            return audioClipsNormal;
        }

        return null;
    }

    private void InitializeAudioSources(List<AudioList> audioList, bool startFromADifferentPosition)
    {
        if (audioList == null)
        {
            return;
        }

        int tempCurrentStage = Mathf.Min(currentStage, audioList.Count - 1);

        List<AudioClip> audioClips = audioList[tempCurrentStage].audioClips;

        foreach (AudioSource source in audioSources)
        {
            source.Stop();
        }

        for (int i = 0; i < audioClips.Count; i++)
        {
            audioSources[i].clip = audioClips[i];

            if (startFromADifferentPosition)
            {
                audioSources[i].time = Time.realtimeSinceStartup - startTime - (currentLoop - currentLoop % 2) * halfMeasureLength;
            }
            else
            {
                audioSources[i].time = 0;
            }

            audioSources[i].volume = defaultVolume;
            audioSources[i].Play();
        }
    }
}


[System.Serializable]
public class AudioList
{
    public List<AudioClip> audioClips = new List<AudioClip>();
}