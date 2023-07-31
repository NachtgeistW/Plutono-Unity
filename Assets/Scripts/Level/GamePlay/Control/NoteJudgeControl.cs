using System.Collections.Generic;
using System.Linq;
using Plutono.GamePlay.Notes;
using Plutono.Level.GamePlay;
using Plutono.Song;
using Plutono.Util;
using UnityEngine;

namespace Plutono.GamePlay.Control
{
    public class NoteJudgeControl : MonoBehaviour
    {
        [SerializeField] private NoteControl noteControl;
        private GameMode mode;
        private readonly Dictionary<int, SlideNote> notesOnSliding = new(); //Finger index and sliding note on it
        //private readonly Dictionary<int, HoldNote> notesOnHolding= new(); //Finger index and holding note on it

        #region UnityEvent

        private void OnEnable()
        {
            EventCenter.AddListener<FingerDownEvent>(OnFingerDown);
            EventCenter.AddListener<FingerMoveEvent>(OnFingerMove);
            EventCenter.AddListener<FingerUpEvent>(OnFingerUp);

            EventCenter.AddListener<GamePauseEvent>(OnGamePause);
        }

        private void OnDisable()
        {
            EventCenter.RemoveListener<FingerDownEvent>(OnFingerDown);
            EventCenter.RemoveListener<FingerMoveEvent>(OnFingerMove);
            EventCenter.RemoveListener<FingerUpEvent>(OnFingerUp);

            EventCenter.RemoveListener<GamePauseEvent>(OnGamePause);
        }

        private void Start()
        {
            mode = SongSelectDataTransformer.GameMode;
        }

        #endregion

        private void OnGamePause(GamePauseEvent evt)
        {
            // Clear status
            notesOnSliding.Clear();
        }

        public void OnFingerDown(FingerDownEvent evt)
        {
            var worldPos = evt.WorldPos;
            var curTime = evt.Time;

            {
                // Query slide note
                foreach (var curDetectingNote in noteControl.slideNotes
                             .Where(note => note.OnTap(worldPos, curTime, out _, out _)))
                {
                    if (notesOnSliding.ContainsKey(evt.Finger.index) || notesOnSliding.ContainsValue(curDetectingNote))
                    {
                        // Player is sliding on another note, pass
                        continue;
                    }
                    notesOnSliding.Add(evt.Finger.index, curDetectingNote);
                    curDetectingNote.OnSlideStart(worldPos, curTime);
                    return;
                }
            }
            {
                // Blank note
                if (FindClosestHitNote(noteControl.blankNotes, worldPos, curTime,
                        out var note, out var deltaTime, out var deltaXPos))
                {
#if DEBUG
                    Debug.Log("NoteJudgeControl Broadcast NoteClearEvent\n" +
                              $"Note: {note.id} Time: {note.time} CurTime: {curTime} Pos: {note.pos} JudgeSize: {(note.size < 1.2 ? 0.6 : note.size / 2)}");
#endif
                    EventCenter.Broadcast(new NoteClearEvent<BlankNote>
                    {
                        Note = note,
                        Grade = NoteGradeJudgment.Judge(deltaTime, mode),
                        DeltaXPos = deltaXPos
                    });
                    noteControl.blankNotes.Remove(note);
                    return;
                }
            }
            {
                // Piano note
                if (FindClosestHitNote(noteControl.pianoNotes, worldPos, curTime, out var note, out var deltaTime, out var deltaXPos))
                {
#if DEBUG
                    Debug.Log("NoteJudgeControl Broadcast NoteClearEvent\n" +
                              $"Note: {note.id} Time: {note.time} CurTime: {curTime} Pos: {note.pos} JudgeSize: {(note.size < 1.2 ? 0.6 : note.size / 2)}");
#endif
                    EventCenter.Broadcast(new NoteClearEvent<PianoNote>
                    {
                        Note = note,
                        Grade = NoteGradeJudgment.Judge(deltaTime, mode),
                        DeltaXPos = deltaXPos
                    });
                    noteControl.pianoNotes.Remove(note);
                    return;
                }
            }
        }

        public void OnFingerMove(FingerMoveEvent evt)
        {
            // Query slide note
            var worldPos = evt.WorldPos;
            var curTime = evt.Time;

            if (notesOnSliding.TryGetValue(evt.Finger.index, out var note))
            {
                if (note.IsClear) return;

                var canBeClear = note.UpdateSlide(worldPos);
                if (canBeClear)
                {
                    note.OnSlideEnd(curTime, out var deltaTime, out var deltaXPos);
                    EventCenter.Broadcast(new NoteClearEvent<SlideNote>
                    {
                        Note = note,
                        Grade = NoteGradeJudgment.Judge(deltaTime, mode),
                        DeltaXPos = deltaXPos
                    });
                    notesOnSliding.Remove(evt.Finger.index);
                    noteControl.slideNotes.Remove(note);
                }
            }
            else
            {
                foreach (var curDetectingNote in noteControl.slideNotes
                             .Where(newNote => newNote.OnTap(worldPos, curTime, out _, out _)))
                {
                    notesOnSliding.Add(evt.Finger.index, curDetectingNote);
                    curDetectingNote.OnSlideStart(worldPos, curTime);
                    return;
                }
            }
        }

        public void OnFingerUp(FingerUpEvent evt)
        {
            var curTime = evt.Time;

            // Force clear this note
            if (notesOnSliding.TryGetValue(evt.Finger.index, out var note))
            {
                if (note.IsClear) return;
                note.OnSlideEnd(curTime,
                    out var deltaTime, out var deltaXPos);
                EventCenter.Broadcast(new NoteClearEvent<SlideNote>
                {
                    Note = note,
                    Grade = NoteGradeJudgment.Judge(deltaTime, mode),
                    DeltaXPos = deltaXPos
                });
                notesOnSliding.Remove(evt.Finger.index);
                noteControl.slideNotes.Remove(note);
            }
        }

        /// <summary>
        /// Check if player hit a note
        /// </summary>
        /// <param name="notes"></param>
        /// <param name="pos"></param>
        /// <param name="touchTime">the time when player touches the screen</param>
        /// <param name="note">The hit note. null if none</param>
        /// <param name="deltaTime">MaxValue if none</param>
        /// <param name="deltaXPos">MaxValue if none</param>
        /// <returns>True if hit note. </returns>
        private bool FindClosestHitNote<TNote>(List<TNote> notes, Vector3 pos, double touchTime, out TNote note, out double deltaTime, out float deltaXPos)
            where TNote : BaseNote, ITapable
        {
            note = null;
            deltaTime = double.MaxValue;
            deltaXPos = float.MaxValue;

            //if (finger.OnGui)
            //    return false;

            //if (pos.y < 0.6) return false;

            var lastDeltaXPos = float.MaxValue;
            var lastDeltaTime = double.MaxValue;
            foreach (var curDetectingNote in notes)
            {
                if (!curDetectingNote.IsHit(pos.x, out deltaXPos, touchTime, out deltaTime))
                    continue;
                if (deltaXPos < lastDeltaXPos)
                {
                    note = curDetectingNote;
                    lastDeltaXPos = deltaXPos;
                    lastDeltaTime = deltaTime;
                }
                // Touch point is closer to the centre of this note, despite having a larger interval.
                else if (deltaXPos >= lastDeltaXPos && deltaTime < lastDeltaTime)
                {
                    note = curDetectingNote;
                    lastDeltaXPos = deltaXPos;
                    lastDeltaTime = deltaTime;
                }
                //if (deltaTime < lastDeltaTime)
                //{
                //    note = curDetectingNote;
                //    lastDeltaXPos = deltaXPos;
                //    lastDeltaTime = deltaTime;
                //}
                //// Touch point is closer to the centre of this note, despite having a larger interval.
                //else if (deltaTime >= lastDeltaTime && deltaXPos < lastDeltaXPos)
                //{
                //    note = curDetectingNote;
                //    lastDeltaXPos = deltaXPos;
                //    lastDeltaTime = deltaTime;
                //}
            }

            if (note == null) return false;
            return deltaTime > Settings.SteloMode.badDeltaTime;
        }
    }
}