using Plutono.GamePlay;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Plutono.GameData
{
    public class PlayerScores : Singleton<PlayerScores>
    {
        public Dictionary<string, int> ScoreList = new();

        /// <summary>
        /// Load score from local PlayerPrefs
        /// </summary>
        /// <param name="songs"></param>
        public void Initialize(List<Song.SongDetail> songs)
        {
            foreach (var song in songs)
            {
                foreach (var mode in Enum.GetValues(typeof(GameMode)))
                {
                    var key = $"{song.chartDetails[SongSelectDataTransformer.SelectedChartIndex].id}_{mode}";
                    if ((GameMode)mode == GameMode.Autoplay)
                        continue;
                    if (PlayerPrefs.HasKey(key))
                        ScoreList.Add(key, PlayerPrefs.GetInt(key));
                }
            }
        }

        /// <summary>
        /// Save score using song id and game mode
        /// </summary>
        /// <param name="id">song id</param>
        /// <param name="mode">the mode when playing this song</param>
        /// <param name="score">score</param>
        public void Save(string id, GameMode mode, int score)
        {
            var key = $"{id}_{mode}";

            // Save in game
            ScoreList[key] = score;

            // Save to local
            PlayerPrefs.SetInt(key, score);
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Load score using song id and game mode
        /// </summary>
        /// <returns>score if contains, otherwise is -1</returns>
        public int Load(string id, GameMode mode)
        {
            var key = $"{id}_{mode}";
            return ScoreList.TryGetValue(key, out var score) ? score : 0;
        }
    }
}