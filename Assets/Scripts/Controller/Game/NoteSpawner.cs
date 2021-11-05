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
    public class NoteSpawner : MonoBehaviour
    {
        [Tooltip("(·Åprefab²»ÊÇscript£¡)note prefab¡£")]
        [SerializeField] private GameNote prefabNoteView;
        [SerializeField] private Transform noteParentTransform;


        private ObjectPool<GameNote> notePool = new ObjectPool<GameNote>(OnCreatePooledItem, OnTakeFromPool, OnReturnToPool,
        OnDestroyPoolObject, collectionChecks, 10, maxPoolSize);

        //Note
        public void PlaceNewNote(ref List<GameNote> notesList, GameChartModel chartModel)
        {
            foreach (var note in chartModel.notes)
                InitNoteObject(ref notesList, note);
        }

        private void InitNoteObject(ref List<GameNote> notesList, GameNoteModel note)
        {
            var newNote = notePool.Get();
            newNote.Model = note;
            newNote.NoteView.SetNoteAppearance(note);
            notesList.Add(newNote);
        }

        private GameNote OnCreatePooledItem()
        {
            var newNote = Instantiate(prefabNoteView, noteParentTransform);
            newNote.gameObject.SetActive(false);
            return newNote;
        }

    }
}