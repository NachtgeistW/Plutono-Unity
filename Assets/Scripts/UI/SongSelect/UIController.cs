using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Plutono.UI
{
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
                IO.FileManager.Instance.loadFiles.SavePlayerSettingsToJson();
                settingCanvas.gameObject.SetActive(false);
                settingButton.interactable = true;
            });
        }
    }
}