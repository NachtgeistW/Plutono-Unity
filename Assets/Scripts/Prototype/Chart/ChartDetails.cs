/*
 * History
 *      2020.07.29  COPY from Deenote
 *      2020.09.03  COPY from Deenote(Refactor) and EDIT
 *      2021.03.19  COPY from Deenote(Refactor) and EDIT; DELETE links variable; RENAME to GameChart
 *      2021.10.10  RENAME from GameChart to GameChartModel
 *      2022.07.25  RENAME from GameChartModel to ChartDetails
 */
using System.Collections.Generic;

namespace Plutono.Song
{

    //class Chart -- Store the information of a single chart.
    [System.Serializable]
    public sealed class ChartDetails
    {
        public List<float> beats = new();       //beats: List<float>, used to quantize the note.
        public string chartDesigner = "";       //chartDesigner: string, the name of the chart maker
        public string level = "";               //level: string, the level of a song.
        public List<NoteDetails> notes = new(); //notes: List<NoteDetails>, stores the information of the notes in a chart
    }
}