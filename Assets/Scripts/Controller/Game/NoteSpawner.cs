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
        [Tooltip("(放prefab不是script！)note prefab。")]
        [SerializeField] private GameNote prefabNoteView;
        [SerializeField] private Transform noteParentTransform;
        [SerializeField] private bool collectionChecks = true;
        [SerializeField] private int maxPoolSize = 20;


        private ObjectPool<GameNote> notePool;

        void Awake() =>
            notePool = new ObjectPool<GameNote>(OnCreatePooledItem, OnTakeFromPool, OnReturnToPool,
                OnDestroyPoolObject, collectionChecks, 10, maxPoolSize);

        //Note pool
        public void PlaceNewNote(ref ObjectPool<GameNote> notePool, ref List<GameNote> notesList, GameChartModel chartModel)
        {
            foreach (var note in chartModel.notes)
                InitNoteObject(ref notesList, note);
        }

        private void InitNoteObject(ref List<GameNote> notesList, GameNoteModel note)
        {
            var newNote = notePool.Get();
            newNote.Model = note;
            //if (note.type == GameNoteModel.NoteType.Blank)
            //    newNote.BlankNoteView.SetNoteAppearance(note.size, note.pos);

            notesList.Add(newNote);
        }

        private GameNote OnCreatePooledItem()
        {
            var newNote = Instantiate(prefabNoteView, noteParentTransform);
            newNote.gameObject.SetActive(false);
            return newNote;
        }

        private void OnReturnToPool(GameNote note) => note.gameObject.SetActive(false);

        private void OnTakeFromPool(GameNote note) => note.gameObject.SetActive(true);

        private void OnDestroyPoolObject(GameNote note) => Destroy(note);
    }
}