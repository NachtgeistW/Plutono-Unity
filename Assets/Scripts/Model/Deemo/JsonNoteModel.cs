/*
 * class JsonNoteModel -- Store the information of a note from json.
 *
 *      This class includes the id, type, position, size, time, shift, piano sounds and "is a link" property.
 *      If it is in a link, it also use prevLink and nextLink.
 *      type: uint, is always 0
 *      pos: float, the position of this note (from -2 to 2, or it won't be touched)
 *      size: float, the size of this note (from 0 to 4)
 *      _time: float, the time when this note should be touched (start from 0)
 *      time: float, is always equal to _time
 *      shift: uint, is always 0
 *
 * Function:
 *      GameNoteModel::ToGameNote -- transfer JsonNoteModel to GameNoteModel
 *
 * class JsonPianoSound -- Store the information of piano sound on a note.
 *
 *      This class includes the delay, duration, pitch and volume property.
 *
 *      
 * History:
 *      2020.08.10  COPY from Deenote and EDIT
 *      2020.09.03  COPY from Deenote(Refactor) and EDIT
 *      2021.03.07  MOVE the class into namespace Assets.Scripts.Game.Note
 *                  DELETE JsonLink class and definition about link
 *      2021.03.19  ADD function ToGameNote
 *      2021.10.10  RENAME from JsonNote to JsonNoteModel
 */

using Assets.Scripts.Model.Plutono;
using JetBrains.Annotations;
using Model.Plutono;
using Newtonsoft.Json;

namespace Model.Deemo
{
    [System.Serializable]
    [JsonObject(IsReference = true)]
    public sealed class JsonNoteModel
    {
        [JsonProperty(PropertyName = "$id")]
        public uint Id { get; set; }
        public float pos;
        public float shift;
        public float size;
        public float time;
        // There is only a _time before Deemo 3.0
        // ReSharper disable once InconsistentNaming
        public float _time;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [CanBeNull] public JsonPianoSound[] sounds;
        public uint type;

        public JsonNoteModel ToJson() => new JsonNoteModel
        {
            pos = pos,
            size = size,
            shift = shift,
            type = type,
            _time = time,
            time = time,
            //sounds = sounds.Select(sound => sound.ToJson()).ToArray()
        };

        public GameNoteModel ToGameNote() => new GameNoteModel
        {
            id = Id,
            pos = pos,
            size = size,
            time = time,
            //TODO:判断黄条
            type = sounds != null ? GameNoteModel.NoteType.Piano : GameNoteModel.NoteType.Blank
        };
    }

    /*    public sealed class JsonLink
        {
            [CanBeNull] public List<JsonNoteModel> notes;
        }
    */
    [System.Serializable]
    public sealed class JsonPianoSound
    {
        public float w; //w
        public float d; //d
        public short p; //p
        public short v; //v

        public JsonPianoSound ToJson() => new JsonPianoSound() { w = w, d = d, p = p, v = v };
    }
}