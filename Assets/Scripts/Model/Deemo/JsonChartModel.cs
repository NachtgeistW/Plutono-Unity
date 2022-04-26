/*
 * class JsonChartModel -- Store the information of a single chart from json.
 *
 *      This class includes the speed and note property.
 *      speed: float, is always 0.0f (It is in official charts, but no one knows what this means...)
 *      notes: List<JsonNoteModel>, stores the information of the notes in a chart
 *
 * History:
 *      2021.03.29  COPY from Deenote; ADD ToGameChart function
 *      2021.04.04  MOVE JsonToJChart to class JsonChartModel
 *      2021.10.10  RENAME from JsonChart to JsonChartModel
*/
using Model.Plutono;
using Model.Deemo;

namespace Assets.Scripts.Model.Deemo
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Assets.Scripts.Model.Plutono;


    using Newtonsoft.Json;

    [Serializable]
    public class JsonChartModel
    {
        public float speed = 0.0f;
        public List<JsonNoteModel> notes = new();
        //public List<Link> links = new();

        /// <summary>
        /// transfer JsonChartModel to the note list of BlankNote, PianoNote and SlideNote
        /// </summary>
        /// <param name="blankNotes">A list containing Blank Note</param>
        /// <param name="pianoNotes">A list containing Piano Note</param>
        /// <param name="slideNotes">A list containing Slide Note</param>
        public void ToGameChartNoteList(List<BlankNote> blankNotes, List<PianoNote> pianoNotes,
            List<SlideNote> slideNotes)
        {
            foreach (var jNote in notes)
            {
                var gNote = jNote.ToGameNote();
                switch (gNote.type)
                {
                    case GameNoteModel.NoteType.Blank:
                        blankNotes.Add(gNote.ToBlankNote());
                        break;
                    case GameNoteModel.NoteType.Piano:
                        pianoNotes.Add(gNote.ToPianoNote());
                        break;
                    case GameNoteModel.NoteType.Slide:
                        slideNotes.Add(gNote.ToSlideNote());
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// transfer Json to JsonChartModel
        /// </summary>
        /// <param name="jsonPath">the path of json file</param>
        /// <returns>A transferred JsonChartModel</returns>
        public static JsonChartModel JsonToJsonChart(string jsonPath)
        {
            var settings = new JsonSerializerSettings();
            settings.MetadataPropertyHandling = MetadataPropertyHandling.Ignore;
            var r = new StreamReader(jsonPath);
            var json = r.ReadToEnd();
            var jChart = JsonConvert.DeserializeObject<JsonChartModel>(json, settings);
            return jChart;
        }
    }

}