using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Plutono.UI
{
    public class SettingPanel : MonoBehaviour
    {
        [SerializeField] private Button settingButton;

        [Space(10)]
        [Header("Global Settings")]
        [SerializeField] private Canvas settingCanvas;
        [SerializeField] private Button backButton;

        [Space(10)]
        [Header("Game Play")]
        [SerializeField] private Toggle isSuddenOnToggle;
        [SerializeField] private Slider suddenHeightSlider;
        [SerializeField] private InputField globalChartOffsetInputField;
        [SerializeField] private InputField chartMusicOffsetInputField;

        private void Start()
        {
            //Upper Buttons
            settingButton.onClick.AddListener(() =>
            {
                PlayerSettingsManager.Instance.LoadGlobalPlayerSettingsFromJson();
                settingCanvas.gameObject.SetActive(true);
                settingButton.interactable = false;
            });
            backButton.onClick.AddListener(() =>
            {
                PlayerSettingsManager.Instance.SaveGlobalPlayerSettingsToJson();
                settingCanvas.gameObject.SetActive(false);
                settingButton.interactable = true;
            });

            //Game Play
            isSuddenOnToggle.onValueChanged.AddListener(isSuddenOn =>
            {
                PlayerSettingsManager.Instance.GSettings.IsSuddenOn = isSuddenOn;
                suddenHeightSlider.interactable = isSuddenOn;
            });
            isSuddenOnToggle.isOn = PlayerSettingsManager.Instance.GSettings.IsSuddenOn;

            suddenHeightSlider.onValueChanged.AddListener(suddenHeight =>
                PlayerSettingsManager.Instance.GSettings.SuddenHeight = SetSuddenHeight(suddenHeight));
            suddenHeightSlider.value = SetSuddenHeight(PlayerSettingsManager.Instance.GSettings.SuddenHeight);

            globalChartOffsetInputField.onSubmit.AddListener(text =>
            {
                var globalChartOffset = float.Parse(text) switch
                {
                    < -3f => -3f,
                    > 3f => 3f,
                    _ => float.Parse(text)
                };
                PlayerSettingsManager.Instance.GSettings.GlobalChartOffset = globalChartOffset;
            });
            globalChartOffsetInputField.text = PlayerSettingsManager.Instance.GSettings.GlobalChartOffset.ToString("F2");

            chartMusicOffsetInputField.onSubmit.AddListener(text =>
            {
                var chartMusicOffset = float.Parse(text) switch
                {
                    < -3f => -3f,
                    > 3f => 3f,
                    _ => float.Parse(text)
                };
                PlayerSettingsManager.Instance.GSettings.ChartMusicOffset = chartMusicOffset;
            });
            chartMusicOffsetInputField.text = PlayerSettingsManager.Instance.GSettings.ChartMusicOffset.ToString("F2");
        }

        private float SetSuddenHeight(float value)
        {
            return value switch
            {
                < 0.1f => 0.1f,
                < 0.2f => 0.2f,
                < 0.3f => 0.3f,
                < 0.4f => 0.4f,
                < 0.5f => 0.5f,
                < 0.6f => 0.6f,
                < 0.7f => 0.7f,
                < 0.8f => 0.8f,
                < 0.9f => 0.9f,
                >= 1f => 1f,
                _ => value
            };
        }
    }
}