using Plutono.GamePlay.Notes;
using Plutono.Song;
using Plutono.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using Plutono.Level.GamePlay;
using UnityEngine;

namespace Plutono.GamePlay.Control
{
    [RequireComponent(typeof(NoteJudgeControl))]
    [RequireComponent(typeof(BlankNoteSpawner))]
    [RequireComponent(typeof(PianoNoteSpawner))]
    [RequireComponent(typeof(SlideNoteSpawner))]
    [RequireComponent(typeof(ExplosionController))]
    public class NoteControl : MonoBehaviour
    {
        public List<BlankNote> blankNotes = new();
        public List<PianoNote> pianoNotes = new();
        public List<SlideNote> slideNotes = new();

        public INoteSpawner<BlankNote> BlankNoteSpawnerClient;
        public INoteSpawner<PianoNote> PianoNoteSpawnerClient;
        public INoteSpawner<SlideNote> SlideNoteSpawnerClient;

        private readonly float chartPlaySpeed = SongSelectDataTransformer.ChartPlaySpeed;
        private readonly GameMode mode = SongSelectDataTransformer.GameMode;
        private double noteGenerationLeadTime;

        // Update in game
        private double curTime;
        private int lastAppearanceNoteIndex;

        #region UnityEvent

        private void Start()
        {
            noteGenerationLeadTime = Settings.NoteFallTime(chartPlaySpeed);

            BlankNoteSpawnerClient = GetComponent<BlankNoteSpawner>();
            PianoNoteSpawnerClient = GetComponent<PianoNoteSpawner>();
            SlideNoteSpawnerClient = GetComponent<SlideNoteSpawner>();
        }

        private void Update()
        {
            curTime = GamePlayController.Instance.CurTime;

            GenerateNote();

            if (mode == GameMode.Autoplay)
            {
                MoveAndCollectInAutoMode();
            }
            else
            {
                MoveAndCollect();
            }
        }

        #endregion

        private void GenerateNote()
        {
            //Generate notes according to the time
            while (lastAppearanceNoteIndex < GamePlayController.Instance.ChartDetail.noteDetails.Count)
            {
                var nextNote = GamePlayController.Instance.ChartDetail.noteDetails[lastAppearanceNoteIndex];
                //添加生成的提前量
                var nextNoteTime = nextNote.time;
                if (curTime + noteGenerationLeadTime >= nextNoteTime
                // || nextNoteTime - (curTime + noteGenerationLeadTime) < 0.001
                    )
                {
                    switch (nextNote.type)
                    {
                        case NoteType.Blank:
                            blankNotes.Add(BlankNoteSpawnerClient.SpawnNotes(nextNote));
                            break;
                        case NoteType.Piano:
                            pianoNotes.Add(PianoNoteSpawnerClient.SpawnNotes(nextNote));
                            break;
                        case NoteType.Slide:
                            slideNotes.Add(SlideNoteSpawnerClient.SpawnNotes(nextNote));
                            break;
                        case NoteType.Vibrate:
                            break;
                        case NoteType.Swipe:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    lastAppearanceNoteIndex++;
                }
                else
                    break;
            }
        }

        private void MoveAndCollect()
        {
            foreach (var note in blankNotes.ToList())
            {
                if (!note.IsClear && note.ShouldBeMiss())
                {
                    note.IsClear = true;
                    EventCenter.Broadcast(new NoteMissEvent<BlankNote> { Note = note });
                    blankNotes.Remove(note);
                }

                note.OnMove(chartPlaySpeed, curTime);
            }

            foreach (var note in pianoNotes.ToList())
            {
                if (!note.IsClear && note.ShouldBeMiss())
                {
                    note.IsClear = true;
                    Debug.Log("NoteControl Broadcast NoteMissEvent\n" +
                              $"Note: {note.id} Time: {note.time} CurTime: {curTime} Pos: {note.pos} JudgeSize: {(note.size < 1.2 ? 0.6 : note.size / 2)}");
                    EventCenter.Broadcast(new NoteMissEvent<PianoNote> { Note = note });
                    pianoNotes.Remove(note);
                }
                note.OnMove(chartPlaySpeed, curTime);
            }

            foreach (var note in slideNotes.ToList())
            {
                if (!note.IsClear && note.ShouldBeMiss())
                {
                    note.IsClear = true;
                    EventCenter.Broadcast(new NoteMissEvent<SlideNote> { Note = note });
                    slideNotes.Remove(note);
                }

                note.OnMove(chartPlaySpeed, curTime);
            }
        }
        private void MoveAndCollectInAutoMode()
        {
            foreach (var note in blankNotes.ToList())
            {
                if (!note.IsClear && note.transform.position.z <= 0)
                {
                    note.IsClear = true;
                    EventCenter.Broadcast(new NoteClearEvent<BlankNote> { Note = note, DeltaXPos = 0, Grade = NoteGrade.Perfect });
                    blankNotes.Remove(note);
                }

                note.OnMove(chartPlaySpeed, curTime);
            }

            foreach (var note in pianoNotes.ToList())
            {
                if (!note.IsClear && note.ShouldBeMiss())
                {
                    note.IsClear = true;
                    EventCenter.Broadcast(new NoteClearEvent<PianoNote> { Note = note, DeltaXPos = 0, Grade = NoteGrade.Perfect });
                    pianoNotes.Remove(note);
                }

                note.OnMove(chartPlaySpeed, curTime);
            }

            foreach (var note in slideNotes.ToList())
            {
                if (!note.IsClear && note.ShouldBeMiss())
                {
                    note.IsClear = true;
                    EventCenter.Broadcast(new NoteClearEvent<SlideNote> { Note = note, DeltaXPos = 0, Grade = NoteGrade.Perfect });
                    slideNotes.Remove(note);
                }

                note.OnMove(chartPlaySpeed, curTime);
            }
        }
    }
}