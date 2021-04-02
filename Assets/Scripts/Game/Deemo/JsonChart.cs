/*
 * class JsonChart -- Store the information of a single chart from json.
 *
 *      This class includes the speed and note property.
 *      speed: float, is always 0.0f (It is in official charts, but no one knows what this means...)
 *      notes: List<JsonNote>, stores the information of the notes in a chart
 *
 * Function:
 *      GameChart::ToGChart -- transfer JsonChart to GameChart
 *
 * History:
 *      2021.03.29  COPY from Deenote; ADD ToGChart function
*/

using System.Collections.Generic;
using Assets.Scripts.Game.Note;
using Assets.Scripts.Game.Plutono;

namespace Assets.Scripts.Game.Deemo
{
    [System.Serializable]
    public class JsonChart
    {
        public float speed = 0.0f;
        public List<JsonNote> notes = new List<JsonNote>();
        //public List<Link> links = new();

        public GameChart ToGChart()
        {
            var gChart = new GameChart();
            foreach (var jNote in notes)
            {
                var note = new GameNote()
                {
                    pos = jNote.pos,
                    size = jNote.size,
                    time = jNote._time,
                };
                gChart.notes.Add(note);
            }
            return gChart;
        }

    }

}