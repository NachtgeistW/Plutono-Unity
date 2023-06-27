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
        public string songName = "";
        public string composer = "";
        public string musicPath = "";
        public List<ChartDetail> chartDetails = new();
#nullable enable
        public Sprite? Cover;
#nullable disable

        public SongDetail () {}
        public SongDetail (Legacy.LegacySongDetail legacySongDetail) 
        {
            songName = legacySongDetail.IniInfo.SongName;
            composer = legacySongDetail.IniInfo.Artist;
            chartDetails = legacySongDetail.ChartDetails;
            Cover = legacySongDetail.Cover;
            musicPath = legacySongDetail.MusicPath;
        }
    }
}