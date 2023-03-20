using DG.Tweening.Core.Easing;
using Plutono.GamePlay;
using Plutono.IO;
using System;
using UnityEngine;

public class ScoreInfo : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI _songNameText;
    [SerializeField] private TMPro.TextMeshProUGUI _perfectText;
    [SerializeField] private TMPro.TextMeshProUGUI _goodText;
    [SerializeField] private TMPro.TextMeshProUGUI _badText;
    [SerializeField] private TMPro.TextMeshProUGUI _missText;
    [SerializeField] private TMPro.TextMeshProUGUI _modeText;
    [SerializeField] private TMPro.TextMeshProUGUI _scoreText;
    

    void Start()
    {
        _songNameText.text = FileManager.Instance.songSourceList[SongSelectDataTransformer.SelectedSongIndex].SongName;
        _perfectText.text = Convert.ToString("P " + ResultDataTransformer.PCount);
        _goodText.text = Convert.ToString("G " + ResultDataTransformer.GCount);
        _badText.text = Convert.ToString("B " + ResultDataTransformer.BCount);
        _missText.text = Convert.ToString("M " + ResultDataTransformer.MCount);
        _modeText.text = Convert.ToString(SongSelectDataTransformer.GameMode);
        _scoreText.text = Convert.ToString(ResultDataTransformer.BasicScore) + " + "
                + Convert.ToString(ResultDataTransformer.ComboScore * 100000 / 1024);
    }

}
