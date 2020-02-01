using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DescriptionTextScript : MonoBehaviour
{
    private Text text;

    private void Start()
    {
        text = GetComponent<Text>();
    }

    private void OnEnable()
    {
        Player.StartJobEvent += OnStartJob;
    }

    private void OnStartJob(string desc, int time)
    {
        text.text = desc;
    }

    private void OnDisable()
    {
        text.text = "";
        Player.StartJobEvent -= OnStartJob;
    }

}
