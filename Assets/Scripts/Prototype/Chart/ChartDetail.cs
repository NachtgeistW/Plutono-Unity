/*
 * History
 *      2020.07.29  COPY from Deenote
 *      2020.09.03  COPY from Deenote(Refactor) and EDIT
 *      2021.03.19  COPY from Deenote(Refactor) and EDIT; DELETE links variable; RENAME to GameChart
 *      2021.10.10  RENAME from GameChart to GameChartModel
 *      2022.07.25  RENAME from GameChartModel to ChartDetail
 */
using System.Collections.Generic;

namespace Plutono.Song
{
    /// <summary>
    /// class ChartDetail -- Store the information of a single chart.
    /// </summary>
    [System.Serializable]
    public sealed class ChartDetail
    {
        public string id;   //id: string, the id of a chart. value = MD5(songName + composer + level + chartDesigner + noteDetails)
        public List<float> beats = new();       //beats: List<float>, used to quantize the note.
        public string chartDesigner = "";       //chartDesigner: string, the name of the chart maker
        public string level = "";               //level: string, the level of a song.
        public List<NoteDetail> noteDetails = new(); //notes: List<NoteDetails>, stores the information of the notes in a chart
    }
}