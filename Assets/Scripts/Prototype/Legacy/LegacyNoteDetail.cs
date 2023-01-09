/*
 * History:
 *      2020.08.10  COPY from Deenote and EDIT
 *      2020.09.03  COPY from Deenote(Refactor) and EDIT
 *      2021.03.07  MOVE the class into namespace Assets.Scripts.Game.Note
 *                  DELETE JsonLink class and definition about link
 *      2021.03.19  ADD function ToGameNote
 *      2021.10.10  RENAME from JsonNote to JsonNoteModel
 *      2022.11.24  RENAME to LegacyNoteDetail
 *                  MOVE the class into namespace Plutono.Legacy
 */

using JetBrains.Annotations;
using Newtonsoft.Json;
using Plutono.Song;

namespace Plutono.Legacy
{
    /// <summary>
    /// Store the information of piano sound on a note.
    /// This class includes the delay, duration, pitch and volume property.
    /// </summary>
    [System.Serializable]
    public sealed class LegacyPianoSound
    {
        public float w; //w
        public float d; //d
        public short p; //p
        public short v; //v

        //public LegacyPianoSound ToJson() => new() { w = w, d = d, p = p, v = v };
    }
    
    /// <summary>
    /// Store the information of a note from json.
    /// This class includes the id, type, position, size, time, shift, piano sounds and "is a link" property.
    /// If it is in a link, it also use prevLink and nextLink.
    /// </summary>
    [System.Serializable]
    [JsonObject(IsReference = true)]
    public sealed class LegacyNoteDetail
    {
        [JsonProperty(PropertyName = "$id")]
        public uint Id { get; set; }
        public float pos;   //the position of this note (from -2 to 2, or it won't be touched)
        public float shift; //is always 0
        public float size;  //the size of this note (from 0 to 4)
        public float time;  //is always equal to _time
        // There is only a _time before Deemo 3.0
        // ReSharper disable once InconsistentNaming
        public float _time; //the time when this note should be touched (start from 0)
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [CanBeNull] public LegacyPianoSound[] sounds;
        public uint type;   //is always 0

/*        public LegacyNoteDetail ToJson() => new()
        {
            pos = pos,
            size = size,
            shift = shift,
            type = type,
            _time = time,
            time = time,
            sounds = sounds.Select(sound => sound.ToJson()).ToArray()
        };
*/
        /// <summary>
        /// transfer LegacyNoteDetail to NoteDetail
        /// </summary>
        /// <returns>new NoteDetail data</returns>
        public NoteDetail ToNoteDetail() => new()
        {
            id = Id,
            pos = pos,
            size = size,
            time = _time,
            //初步判断note类型。因为黄条需要依靠LegacyChartDetail里的Link做判断，
            //所以黄条的检测放在LegacyChartDetail.ToNoteDetailList()中
            type = sounds != null ? NoteType.Piano : NoteType.Blank,
        };
    }

    /*    public sealed class JsonLink
        {
            [CanBeNull] public List<JsonNoteModel> notes;
        }
    */
}