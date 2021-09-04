using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using DG.Tweening;

public class GUI : MonoBehaviour
{ 

    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private GameObject modalPanel; //exit prompt
    [SerializeField]
    private Canvas controlPanel; //simulation controls
    [SerializeField]
    private Canvas UIPanel; //all UI except modal panel

    [SerializeField]
    private Text timer;
    [SerializeField]
    private Text stepCounter;

    [SerializeField]
    private Text muteText;
    [SerializeField]
    private Button muteButton;
    [SerializeField]
    private AudioMixer audioMixer;
    
    [SerializeField]
    [Range(0,1)]
    private float animDuration = 0.1f; //modal panel fade animation duration (in seconds)

    private int numOfSteps;
    private float timeElapsed;

    private bool isTimerRunning;
    private bool areSimControlsActive;
    private bool isUIActive;

    private bool isMuted;
    private float currentVolume;

    private Button[] buttons;

    void OnEnable()
    {
        CubesHandler.OnSortToggle += ToggleControls;
        UFO.OnSwapIsOver += CountSteps;
    }

    void Awake()
    {
        numOfSteps = 0;
        timeElapsed = 0f;

        isTimerRunning = false;
        areSimControlsActive = true;
        isUIActive = true;
        
        isMuted = false;
        audioMixer.GetFloat("MasterVol", out currentVolume);

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

    void ToggleControls()
    {
        areSimControlsActive = !areSimControlsActive;

        controlPanel.GetComponent<CanvasGroup>().interactable = areSimControlsActive;
    }

    void UIToggle()
    {
        isUIActive = !isUIActive;
        
        ToggleControls();
        UIPanel.GetComponent<CanvasGroup>().interactable = isUIActive;
    }

    public void QuitPrompt()
    {
        Time.timeScale = 0;
        modalPanel.transform.DOScaleX(0.3f, animDuration).SetUpdate(true);
        ToggleMute();
        UIToggle();
    }

    public void CloseQuitPrompt()
    {
        Time.timeScale = 1;
        modalPanel.transform.DOScaleX(0f, animDuration).SetUpdate(true); 
        ToggleMute(); 
        UIToggle();
    } 

    public void ToggleMute()
    {
        float volume = 0f;

        isMuted = !isMuted;

        if(isMuted)
        {
            volume = -80.0f;
            muteText.text = "Unmute";
        } 
        else
        {
            volume = currentVolume;
            muteText.text = "Mute";
        }

        audioMixer.SetFloat("MasterVol", volume);
    }

    public void Quit()
    {
        Application.Quit();
    }

    void Update()
    {
        isTimerRunning = !areSimControlsActive;

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

    void DisablecontrolPanel()
    {
        controlPanel.GetComponent<CanvasGroup>().interactable = false;
    }

    void OnDisable()
    {
        CubesHandler.OnSortToggle -= ToggleControls;
        UFO.OnSwapIsOver -= CountSteps;
    }
}
