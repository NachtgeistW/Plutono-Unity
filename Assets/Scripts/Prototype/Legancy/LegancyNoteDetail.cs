/*
 * History:
 *      2020.08.10  COPY from Deenote and EDIT
 *      2020.09.03  COPY from Deenote(Refactor) and EDIT
 *      2021.03.07  MOVE the class into namespace Assets.Scripts.Game.Note
 *                  DELETE JsonLink class and definition about link
 *      2021.03.19  ADD function ToGameNote
 *      2021.10.10  RENAME from JsonNote to JsonNoteModel
 *      2022.11.24  RENAME to LegancyNoteDetail
 *                  MOVE the class into namespace Plutono.Legancy
 */

using JetBrains.Annotations;
using Newtonsoft.Json;
using Plutono.Song;

namespace Plutono.Legancy
{
    /// <summary>
    /// Store the information of a note from json.
    /// This class includes the id, type, position, size, time, shift, piano sounds and "is a link" property.
    /// If it is in a link, it also use prevLink and nextLink.
    /// </summary>
    [System.Serializable]
    [JsonObject(IsReference = true)]
    public sealed class LegancyNoteDetail
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
        [CanBeNull] public LegancyPianoSound[] sounds;
        public uint type;   //is always 0

        public LegancyNoteDetail ToJson() => new LegancyNoteDetail
        {
            pos = pos,
            size = size,
            shift = shift,
            type = type,
            _time = time,
            time = time,
            //sounds = sounds.Select(sound => sound.ToJson()).ToArray()
        };

        /// <summary>
        /// transfer LegancyNoteDetail to NoteDetail
        /// </summary>
        /// <returns>new NoteDetail data</returns>
        public NoteDetail ToNoteDetail() => new NoteDetail
        {
            id = Id,
            pos = pos,
            size = size,
            time = time,
            //TODO:判断黄条
            type = sounds != null ? NoteType.Piano : NoteType.Blank,
        };
    }

    /*    public sealed class JsonLink
        {
            [CanBeNull] public List<JsonNoteModel> notes;
        }
    */

    /// <summary>
    /// Store the information of piano sound on a note.
    /// This class includes the delay, duration, pitch and volume property.
    /// </summary>
    [System.Serializable]
    public sealed class LegancyPianoSound
    {
        public float w; //w
        public float d; //d
        public short p; //p
        public short v; //v

        public LegancyPianoSound ToJson() => new LegancyPianoSound() { w = w, d = d, p = p, v = v };
    }
}