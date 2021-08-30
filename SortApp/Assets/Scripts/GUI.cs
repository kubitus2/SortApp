using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GUI : MonoBehaviour
{

    [SerializeField]
    Canvas canvas;

    [SerializeField]
    private List<Button> buttonlist = new List<Button>();

    [SerializeField]
    private GameObject modalPanel;

    [SerializeField]
    private Text timer;

    [SerializeField]
    private Text stepCounter;

    int numOfSteps;
    float timeElapsed;
    bool isTimerRunning;


    private bool UIActive;


    void OnEnable()
    {
        CubesHandler.OnSortToggle += StateToggle;
        UFO.OnSwapIsOver += CountSteps;
    }

    void Awake()
    {
        UIActive = true;
        numOfSteps = 0;
        timeElapsed = 0f;
        isTimerRunning = false;
    }

    void StateToggle()
    {
        UIActive = !UIActive;

        foreach(var btn in buttonlist)
        {
            btn.interactable = UIActive;
        }
    }

    public void QuitPrompt()
    {
        modalPanel.SetActive(true);
        modalPanel.transform.DOScaleX(0.3f, 0.1f);
    }

    public void ClosePrompt()
    {
        modalPanel.transform.DOScaleX(0f, 0.2f);
        modalPanel.SetActive(false);
    } 

    public void Quit()
    {
        Application.Quit();
    }

    void Update()
    {
        isTimerRunning = !UIActive;

        if(isTimerRunning)
        {
            timeElapsed += Time.deltaTime;
            DisplayTime(timeElapsed);
            DisplaySteps(numOfSteps);
        }
        else
        {
            timeElapsed = 0f;
            numOfSteps = 0;
            //DisplayTime(timeElapsed);
        }
    }

    void DisplayTime(float time)
    {
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);
        float miliSeconds = (time % 1) * 1000;

        timer.text = string.Format("Time elapsed: {0:00}:{1:00}.{2:000}", minutes, seconds,miliSeconds);
    }

    void DisplaySteps(int steps)
    {
        stepCounter.text = string.Format("Steps done: {0}", steps);
    }

    void CountSteps()
    {
        numOfSteps++;
    }

    void OnDisable()
    {
        CubesHandler.OnSortToggle -= StateToggle;
        UFO.OnSwapIsOver -= CountSteps;
    }

}
