namespace Plutono.Song
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Store the information of chart collection of a single song.
    /// This class includes the song name, composer, charts and cover.
    /// </summary>
    [System.Serializable]
    public class SongDetail
    {
        public string SongName = "";
        public string Composer = "";
        public string MusicPath = "";
        public List<ChartDetail> ChartDetails = new();
#nullable enable
        public Sprite? Cover;
#nullable disable

        public SongDetail () {}
        public SongDetail (Legacy.LegacySongDetail legacySongDetail) 
        {
            SongName = legacySongDetail.IniInfo.SongName;
            Composer = legacySongDetail.IniInfo.Artist;
            ChartDetails = legacySongDetail.ChartDetails;
            Cover = legacySongDetail.Cover;
            MusicPath = legacySongDetail.MusicPath;
        }
    }
}