using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI : MonoBehaviour
{

    [SerializeField]
    Canvas canvas;

    [SerializeField]
    private List<Button> buttonlist = new List<Button>();


    private bool UIActive;

    void OnEnable()
    {
        CubesHandler.OnSortToggle += StateToggle;
    }

    void Awake()
    {
        UIActive = true;
    }

    void StateToggle()
    {
        UIActive = !UIActive;

        foreach(var btn in buttonlist)
        {
            btn.interactable = UIActive;
        }
        
    }

    void OnDisable()
    {
        CubesHandler.OnSortToggle -= StateToggle;
    }
}
