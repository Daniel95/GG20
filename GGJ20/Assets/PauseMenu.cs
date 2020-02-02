using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    public static bool gameIsPaused = false;

    [SerializeField,Tooltip("Displays the pause menu")]
    public GameObject pauseMenuUI;

    public  Button pauseMenuButton;

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0)) //change this to button input
        {
             if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }
}
