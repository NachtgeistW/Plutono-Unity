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

    public class NoteSpawner : MonoBehaviour
    {
        [Tooltip("(·Åprefab²»ÊÇscript£¡)note prefab¡£")]
        [SerializeField] private BlankNote PrefabBlankNoteView;
        [SerializeField] private PianoNote PrefabPianoNoteView;
        [SerializeField] private SlideNote PrefabSlideNoteView;
        [SerializeField] private Transform noteParentTransform;
        [SerializeField] private bool collectionChecks = true;
        [SerializeField] private int maxPoolSize = 20;


        private ObjectPool<BlankNote> BlankNotePool;
        private ObjectPool<PianoNote> PianoNotePool;
        private ObjectPool<SlideNote> SlideNotePool;

        NoteSpawner()
        {
            BlankNotePool = new ObjectPool<BlankNote>(OnCreatePooledBlankNote, OnTakeFromPool, OnReturnToPool,
                OnDestroyPoolObject, collectionChecks, 10, maxPoolSize);
            PianoNotePool = new ObjectPool<PianoNote>(OnCreatePooledPianoNote, OnTakeFromPool, OnReturnToPool,
                OnDestroyPoolObject, collectionChecks, 10, maxPoolSize);
            SlideNotePool = new ObjectPool<SlideNote>(OnCreatePooledSlideNote, OnTakeFromPool, OnReturnToPool,
                OnDestroyPoolObject, collectionChecks, 10, maxPoolSize);
        }

        //Note pool
        public void PlaceNewNote(ref List<BlankNote> blankNotesList, ref List<PianoNote> pianoNotesList,
            ref List<SlideNote> slideNotesList, GameChartModel chartModel)
        {
            foreach (var note in chartModel.notes)
            {
                switch (note.type)
                {
                    case GameNoteModel.NoteType.Blank:
                        InitNoteObject(ref blankNotesList, note);
                        break;
                    case GameNoteModel.NoteType.Piano:
                        InitNoteObject(ref pianoNotesList, note);
                        break;
                    case GameNoteModel.NoteType.Slide:
                        InitNoteObject(ref slideNotesList, note);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void InitNoteObject(ref List<BlankNote> notesList, GameNoteModel note)
        {
            var newNote = BlankNotePool.Get();
            newNote.Model = note;
            newNote.BlankNoteView.SetNoteAppearance(note.size, note.pos);
            notesList.Add(newNote);
        }

        private void InitNoteObject(ref List<PianoNote> notesList, GameNoteModel note)
        {
            var newNote = PianoNotePool.Get();
            newNote.Model = note;
            newNote.PianoNoteView.SetNoteAppearance(note.size, note.pos);
            notesList.Add(newNote);
        }

        private void InitNoteObject(ref List<SlideNote> notesList, GameNoteModel note)
        {
            var newNote = SlideNotePool.Get();
            newNote.Model = note;
            newNote.SlideNoteView.SetNoteAppearance(note.size, note.pos);
            notesList.Add(newNote);
        }

        private BlankNote OnCreatePooledBlankNote()
        {
            var newNote = Instantiate(PrefabBlankNoteView, noteParentTransform);
            newNote.gameObject.SetActive(false);
            return newNote;
        }

        private PianoNote OnCreatePooledPianoNote()
        {
            var newNote = Instantiate(PrefabPianoNoteView, noteParentTransform);
            newNote.gameObject.SetActive(false);
            return newNote;
        }

        private SlideNote OnCreatePooledSlideNote()
        {
            var newNote = Instantiate(PrefabSlideNoteView, noteParentTransform);
            newNote.gameObject.SetActive(false);
            return newNote;
        }

        private void OnReturnToPool(GameNote note) => note.gameObject.SetActive(false);

        private void OnTakeFromPool(GameNote note) => note.gameObject.SetActive(true);

        private void OnDestroyPoolObject(GameNote note) => Destroy(note);
    }
}