using Assets.Scripts.GamePlay;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModePanel : MonoBehaviour
{

    [SerializeField] TMP_Dropdown modeDropdown;
    [SerializeField] TMP_Dropdown speedDropdown;
    // Start is called before the first frame update
    void Start()
    {
        SetDefaultDropdown();
        modeDropdown.onValueChanged.AddListener(OnSelectMode);
        speedDropdown.onValueChanged.AddListener(OnSelectSpeed);
    }

    public void OnSelectMode(int value)
    {
        SongSelectDataTransformer.GameMode = (GameMode)value;
    }
    
    public void OnSelectSpeed(int value)
    {
        SongSelectDataTransformer.ChartPlaySpeed = value switch
        {
            0 => 0.5f,
            1 => 5.0f,
            2 => 5.5f,
            3 => 6.0f,
            4 => 6.5f,
            _ => 5.5f,
        };
    }

    private void SetDefaultDropdown()
    {
        modeDropdown.value = (int)SongSelectDataTransformer.GameMode;
        speedDropdown.value = SongSelectDataTransformer.ChartPlaySpeed switch
        {
            0.5f => 0,
            5.0f => 1,
            5.5f => 2,
            6.0f => 3,
            6.5f => 4,
            _ => 2,
        };
    }
}
