/*
 * class PackConfig -- Store the information of chart collection of a single song.
 *
 *      This class includes the speed, level, beats and notes property.
 *      songName: string, song name.
 *      composer: string, the composer of this song.
 *      charts: List<GameChart>, charts in the project
 *
 * History
 *      2021.03.31  CREATE.
 */

using System.Collections.Generic;

using Assets.Scripts.Model.Plutono;

using UnityEngine;

namespace Model.Plutono
{
    /// <summary>
    /// Store the information of chart collection of a single song.
    /// This class includes the song name, composer, charts and cover.
    /// </summary>
    [System.Serializable]
    public class SongInfo
    {
        public string SongName = "";
        public string Composer = "";
        public List<GameChartModel> Charts = new();
        public Sprite? Cover;
        public string MusicPath = "";
    }
}
