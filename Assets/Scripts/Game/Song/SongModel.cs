/*
 * class SongModel -- Model of a song, including its original data.
 *
 *      This class include its music, preview, jacket and
 *      a list of charts (Because maybe there are more than one chart).
 *
 * History
 *      2020.8.12 Created.
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Game.Song
{
    public class SongModel
    {
        public List<Chart.Chart> chart;
        public AudioClip music;
        public AudioClip preview;
        public Image cover;

        public SongModel(List<Chart.Chart> chart, AudioClip music, AudioClip preview, Image cover)
        {
            this.chart = chart;
            this.music = music;
            this.preview = preview;
            this.cover = cover;
        }
    }
}