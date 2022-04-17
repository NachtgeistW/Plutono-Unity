/*
 * class NoteSpawner -- Generate note.
 *
 *      This class is used to generate notes.
 *
 * History
 *      2020.8.14 CREATED.
 */

using System.Collections.Generic;

using Assets.Scripts.Model.Plutono;

using UnityEngine;
using UnityEngine.Pool;

namespace Assets.Scripts.Controller.Game
{
    using System;

    using Views;

    public class NoteSpawner : MonoBehaviour
    {
        [Tooltip("(放prefab不是script！)note prefab。")]
        [SerializeField] private BlankNoteView PrefabBlankNoteView;
        [SerializeField] private PianoNoteView PrefabPianoNoteView;
        [SerializeField] private SlideNoteView PrefabSlideNoteView;
        [SerializeField] private Transform noteParentTransform;
        [SerializeField] private bool collectionChecks = true;
        [SerializeField] private int maxPoolSize = 20;

        private BlankNote PrefabBlankNote;
        private PianoNote PrefabPianoNote;
        private SlideNote PrefabSlideNote;

        public ObjectPool<BlankNote> BlankNotePool;
        public ObjectPool<PianoNote> PianoNotePool;
        public ObjectPool<SlideNote> SlideNotePool;

        void Awake()
        {
            BlankNotePool = new ObjectPool<BlankNote>(OnCreatePooledBlankNote, OnTakeFromPool, OnReturnToPool,
                OnDestroyPoolObject, collectionChecks, 10, maxPoolSize);
            PianoNotePool = new ObjectPool<PianoNote>(OnCreatePooledPianoNote, OnTakeFromPool, OnReturnToPool,
                OnDestroyPoolObject, collectionChecks, 10, maxPoolSize);
            SlideNotePool = new ObjectPool<SlideNote>(OnCreatePooledSlideNote, OnTakeFromPool, OnReturnToPool,
                OnDestroyPoolObject, collectionChecks, 10, maxPoolSize);
        }

        //Note pool
        public void PlaceNewNote(List<BlankNote> blankNotesList, List<PianoNote> pianoNotesList,
            List<SlideNote> slideNotesList, GameChartModel chartModel)
        {
            foreach (var note in chartModel.notes)
            {
                switch (note.type)
                {
                    case GameNoteModel.NoteType.Blank:
                        InitNoteObject(blankNotesList, note);
                        break;
                    case GameNoteModel.NoteType.Piano:
                        InitNoteObject(pianoNotesList, note);
                        break;
                    case GameNoteModel.NoteType.Slide:
                        InitNoteObject(slideNotesList, note);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            /*            foreach (var note in chartModel.BlankNotes)
                        {
                            InitNoteObject(blankNotesList, note.Model);
                        }
                        foreach (var note in chartModel.PianoNotes)
                        {
                            InitNoteObject(pianoNotesList, note.Model);
                        }
                        foreach (var note in chartModel.SlideNotes)
                        {
                            InitNoteObject(slideNotesList, note.Model);
                        }
            */
        }

        private void InitNoteObject(ICollection<BlankNote> notesList, GameNoteModel note)
        {
            var newNote = BlankNotePool.Get();
            newNote.Model = note;
            newNote.BlankNoteView = PrefabBlankNoteView;
            newNote.BlankNoteView.SetNoteAppearance(note.size, note.pos);
            notesList.Add(newNote);
        }

        private void InitNoteObject(ICollection<PianoNote> notesList, GameNoteModel note)
        {
            var newNote = PianoNotePool.Get();
            newNote.Model = note;
            newNote.PianoNoteView = PrefabPianoNoteView;
            newNote.PianoNoteView.SetNoteAppearance(note.size, note.pos);
            notesList.Add(newNote);
        }

        private void InitNoteObject(ICollection<SlideNote> notesList, GameNoteModel note)
        {
            var newNote = SlideNotePool.Get();
            newNote.Model = note;
            newNote.SlideNoteView = PrefabSlideNoteView;
            newNote.SlideNoteView.SetNoteAppearance(note.size, note.pos);
            notesList.Add(newNote);
        }

        private BlankNote OnCreatePooledBlankNote()
        {
            var newNote = Instantiate(PrefabBlankNote, noteParentTransform);
            newNote.gameObject.SetActive(false);
            return newNote;
        }

        private PianoNote OnCreatePooledPianoNote()
        {
            var newNote = Instantiate(PrefabPianoNote, noteParentTransform);
            newNote.gameObject.SetActive(false);
            return newNote;
        }

        private SlideNote OnCreatePooledSlideNote()
        {
            var newNote = Instantiate(PrefabSlideNote, noteParentTransform);
            newNote.gameObject.SetActive(false);
            return newNote;
        }

        private void OnReturnToPool(GameNote note) => note.gameObject.SetActive(false);

        private void OnTakeFromPool(GameNote note) => note.gameObject.SetActive(true);

        private void OnDestroyPoolObject(GameNote note) => Destroy(note);
    }
}