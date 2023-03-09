using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSettingBack : MonoBehaviour
{
    [SerializeField] Button settingButton;
    [SerializeField] Button backButton;
    [SerializeField] Canvas settingCanvas;
    // Start is called before the first frame update
    void Start()
    {
        backButton.onClick.AddListener(delegate
        {
            settingCanvas.gameObject.SetActive(false);
            settingButton.interactable = true;
        });
    }
}
