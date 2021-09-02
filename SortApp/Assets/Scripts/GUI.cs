using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
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
    [SerializeField]
    private float animDuration = 0.1f;
    [SerializeField]
    private AudioMixer am;

    private int numOfSteps;
    private float timeElapsed;

    private bool isTimerRunning;
    private bool isUIActive;

    private bool isMuted;
    private float currentVolume;

    private Button[] buttons;

    void OnEnable()
    {
        CubesHandler.OnSortToggle += StateToggle;
        UFO.OnSwapIsOver += CountSteps;
    }

    void Awake()
    {
        numOfSteps = 0;
        timeElapsed = 0f;

        isTimerRunning = false;
        isUIActive = true;
        
        isMuted = false;
        am.GetFloat("MasterVol", out currentVolume);

        buttons = FindObjectsOfType<Button>();
        AssignSoundToButtons(buttons);
    }

    void AssignSoundToButtons(Button[] btns)
    {
        foreach (var btn in btns)
        {
            btn.onClick.AddListener(() => AudioManager.PlaySound(AudioManager.Sound.ClickSound));
        }
    }

    void StateToggle()
    {
        isUIActive = !isUIActive;

        foreach(var btn in buttonlist)
        {
            btn.interactable = isUIActive;
        }
    }

    public void QuitPrompt()
    {
        Time.timeScale = 0;
        modalPanel.transform.DOScaleX(0.3f, animDuration).SetUpdate(true);
        ToggleMute();
    }

    public void CloseQuitPrompt()
    {
        Time.timeScale = 1;
        modalPanel.transform.DOScaleX(0f, animDuration).SetUpdate(true); 
        ToggleMute(); 
    } 

    public void ToggleMute()
    {
        float volume = 0f;

        isMuted = !isMuted;

        if(isMuted)
        {
            volume = -80.0f;
        } 
        else
        {
            volume = currentVolume;
        }

        am.SetFloat("MasterVol", volume);
    }

    public void Quit()
    {
        Application.Quit();
    }

    void Update()
    {
        isTimerRunning = !isUIActive;

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
        stepCounter.text = string.Format("Swaps done: {0}", steps);
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
