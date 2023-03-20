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
 *      2022.11.23  RENAME to LegacyChartDetail
*/
using System.Collections.Generic;
using System.IO;

namespace Plutono.Legacy
{
    using Newtonsoft.Json;
    using System.Linq;

    [System.Serializable]
    public class LegacyChartDetail
    {
        public float speed = 0.0f;
        public List<LegacyNoteDetail> notes = new();
        public List<Link> links = new();

        [System.Serializable]
        public class Link
        {
            [JsonProperty(PropertyName = "notes")]
            public List<Notes> notes;

            [System.Serializable]
            public class Notes
            {
                [JsonProperty(PropertyName = "$ref")]
                public uint reference;
            }
        }
        
        /// <summary>
        /// transfer LegacyChartDetail to the NoteDetail list
        /// </summary>
        /// <returns>A list containing transferred NoteDetail</returns>
        public List<Song.NoteDetail> ToNoteDetailList()
        {
            var result = notes.Select(n => n.ToNoteDetail()).ToList();
            result = result.OrderBy(n => n.time).ToList();
            //判断黄条
            foreach (var link in links)
            {
                foreach (var note in link.notes)
                {
                    var targetNote = result.Find(x => x.id == note.reference);
                    targetNote.type = Song.NoteType.Slide;
                    targetNote.isLink = true;
                }
            }
            return result;
        }

        /// <summary>
        /// transfer Json to LegacyChartDetail
        /// </summary>
        /// <param name="jsonPath">the path of json file</param>
        /// <returns>A transferred LegacyChartDetail</returns>
        public static LegacyChartDetail JsonToLegacyChartDetail(string jsonPath)
        {
            var settings = new JsonSerializerSettings()
            {
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore
            };
            var r = new StreamReader(jsonPath);
            var json = r.ReadToEnd();
            return JsonConvert.DeserializeObject<LegacyChartDetail>(json, settings);
        }
    }

}