using TMPro;
using UnityEngine;

public class SettingPanel : MonoBehaviour
{
    [SerializeField] TMP_InputField globalChartOffset;
    [SerializeField] TMP_InputField chartMusicOffset;
    
    void Start()
    {
        globalChartOffset.text = System.Convert.ToString(PlayerSettingsManager.Instance.PlayerSettings_Global_SO.globalChartOffset);
        globalChartOffset.onSubmit.AddListener(delegate (string text)
        {
            PlayerSettingsManager.Instance.PlayerSettings_Global_SO.globalChartOffset = System.Convert.ToSingle(text);
        });
        chartMusicOffset.text = System.Convert.ToString(PlayerSettingsManager.Instance.PlayerSettings_Global_SO.chartMusicOffset);
        chartMusicOffset.onSubmit.AddListener(delegate (string text)
        {
            PlayerSettingsManager.Instance.PlayerSettings_Global_SO.chartMusicOffset = System.Convert.ToSingle(text);
        });
    }
}
