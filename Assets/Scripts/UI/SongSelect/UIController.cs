using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] Button settingButton;
    [SerializeField] Button backButton;
    [SerializeField] Canvas settingCanvas;
    // Start is called before the first frame update
    void Start()
    {
        settingButton.onClick.AddListener(delegate
        {
            settingCanvas.gameObject.SetActive(true);
            settingButton.interactable = false;
        });
        backButton.onClick.AddListener(delegate
        {
            settingCanvas.gameObject.SetActive(false);
            settingButton.interactable = true;
        });
    }
}
