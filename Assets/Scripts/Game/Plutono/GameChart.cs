/*
 * class Chart -- Store the information of a single chart.
 *
 *      This class includes the speed, level, beats and notes property.
 *      beats: List<float>, used to quantize the note.
 *      chartDesigner: string, the name of the chart maker
 *      level: string, the level of a song.
 *      notes: List<GameNote>, stores the information of the notes in a chart
 *
 * Function
 *
 *
 * History
 *      2020.07.29  COPY from Deenote
 *      2020.09.03  COPY from Deenote(Refactor) and EDIT
 *      2021.03.19  COPY from Deenote(Refactor) and EDIT; DELETE links variable; RENAME to GameChart
 */

using System.Collections.Generic;
using Assets.Scripts.Game.Note;

namespace Assets.Scripts.Game.Plutono
{
    public sealed class GameChart
    {
        public List<float> beats = new List<float>();
        public string chartDesigner = "";
        public string level = "";
        public List<GameNote> notes = new List<GameNote>();
    }
}