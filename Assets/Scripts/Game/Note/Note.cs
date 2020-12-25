/*
 * class Note -- Store the information of a note.
 *
 *      This class includes the position, size, time, shift, piano sounds and "is a link" property.
 *      If it is in a link, it also use prevLink and nextLink information.
 *
 * class PianoSound -- Store the information of piano sound on a note.
 *
 *      This class includes the delay, duration, pitch and volume property.
 * 
 * History:
 *     2020.8.10 COPY from Deenote and EDIT
 *     2020.9.03 COPY from Deenote(Refactor) and EDIT
 */

using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;

[JsonObject(IsReference = true)]
public sealed class JsonNote
{
    public int type;
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    [CanBeNull] public JsonPianoSound[] sounds;
    public float pos;
    public float size;
    // ReSharper disable once InconsistentNaming
    public float _time;
    public float shift;
    public float time;
}

public sealed class JsonLink
{
    [CanBeNull] public List<JsonNote> notes;
}

public sealed class JsonPianoSound
{
    public float delay; //w
    public float duration; //d
    public short pitch; //p
    public short volume; //v
}