using Plutono.GamePlay;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Plutono.Level.Result
{
    public class ResultController : MonoBehaviour
    {
        [SerializeField] private Text songNameText;
        [SerializeField] private Text composerText;
        [SerializeField] private Text perfectText;
        [SerializeField] private Text goodText;
        [SerializeField] private Text badText;
        [SerializeField] private Text missText;
        [SerializeField] private Text modeText;
        [SerializeField] private Text scoreText;

        private void Start()
        {
            var song = FileManager.Instance.songSourceList[SongSelectDataTransformer.SelectedSongIndex];
            var score = ResultDataTransformer.BasicScore + ResultDataTransformer.ComboScore * 100000 / 1024;
            var mode = SongSelectDataTransformer.GameMode;
            
            // Save the highest player score
            if (mode != GameMode.Autoplay)
            {
                var id = song.chartDetails[SongSelectDataTransformer.SelectedChartIndex].id;
                if (score > GameData.PlayerScores.Instance.Load(id, mode))
                {
                    GameData.PlayerScores.Instance.Save(id, mode, score);
                }
            }

            songNameText.text = song.songName;
            composerText.text = song.composer;
            perfectText.text = Convert.ToString("P " + ResultDataTransformer.PCount);
            goodText.text = Convert.ToString("G " + ResultDataTransformer.GCount);
            badText.text = Convert.ToString("B " + ResultDataTransformer.BCount);
            missText.text = Convert.ToString("M " + ResultDataTransformer.MCount);
            modeText.text = Convert.ToString(SongSelectDataTransformer.GameMode);
            scoreText.text = Convert.ToString(ResultDataTransformer.BasicScore) + " + "
                + Convert.ToString(ResultDataTransformer.ComboScore * 100000 / 1024);
        }
    }
}