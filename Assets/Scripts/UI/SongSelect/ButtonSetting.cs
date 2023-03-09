using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSetting : MonoBehaviour
{
    [SerializeField] Button settingButton;
    [SerializeField] Canvas settingCanvas;
    // Start is called before the first frame update
    void Start()
    {
        settingButton.onClick.AddListener(ShowSettingCanvas);
    }

    void ShowSettingCanvas()
    {
        settingCanvas.gameObject.SetActive(true);
        settingButton.interactable = false;
    }
}
