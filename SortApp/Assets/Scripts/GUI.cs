using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using DG.Tweening;

public class GUI : MonoBehaviour
{ 
    //exit prompt
    [SerializeField]
    private GameObject modalPanel; 
    //simulation controls
    [SerializeField]
    private Canvas controlPanel;
    //all UI except exit prompt 
    [SerializeField]
    private Canvas UIPanel; 

    //text displays
    [SerializeField]
    private Text timer;
    [SerializeField]
    private Text stepCounter;

    //mute button elements
    [SerializeField]
    private Text muteText;
    [SerializeField]
    private Button muteButton;
    [SerializeField]
    private AudioMixer audioMixer;
    
    //modal panel fade animation speed
    [SerializeField]
    [Range(0,1)]
    private float animDuration = 0.1f; 

    //counter vars
    private int numOfSteps;
    private float timeElapsed;

    //UI active flags
    private bool isTimerRunning;
    private bool areSimControlsActive;
    private bool isUIActive;

    //mute utility vars
    private bool isMuted;
    private bool wasMuted;
    private float currentVolume;

    //list of all buttons in the UI
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
        wasMuted = false;
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
        Time.timeScale = 0f;
        modalPanel.transform.DOScaleX(0.3f, animDuration).SetUpdate(true);

        if(!isMuted)
        {
            ToggleMute(); 
        }
        else
        {
            wasMuted = true;
        }

        UIToggle();
    }

    public void CloseQuitPrompt()
    {
        Time.timeScale = 1f;
        modalPanel.transform.DOScaleX(0f, animDuration).SetUpdate(true); 

        if(!wasMuted)
        {
            ToggleMute(); 
        }

        wasMuted = false;

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

    public void ResetCounters()
    {
        timeElapsed = 0f;
        numOfSteps = 0;
        DisplaySteps(numOfSteps);
        DisplayTime(timeElapsed);
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
        CubesHandler.OnSortToggle -= ToggleControls;
        UFO.OnSwapIsOver -= CountSteps;
    }
}
