/*using Assets.Scripts.Model.Plutono;
using UnityEngine.Pool;

namespace Assets.Scripts.Controller.Game
{
    public class ObjectPool
    {
       notePool = new ObjectPool<GameNote>(OnCreatePooledItem, OnTakeFromPool, OnReturnToPool,
        OnDestroyPoolObject, collectionChecks, 10, maxPoolSize);
        PlaceNewNote();


        //Note
        private void PlaceNewNote()
        {
            foreach (var note in ChartInfo.notes)
                InitNoteObject(note);
        }

        private void InitNoteObject(GameNoteModel note)
        {
            var newNote = notePool.Get();
            newNote.Model = note;
            newNote.NoteView.SetNoteAppearance(note);
            notes.Add(newNote);
        }

        private GameNote OnCreatePooledItem()
        {
            var newNoteView = Instantiate(prefabNoteView, noteParentTransform);
            newNote.gameObject.SetActive(false);
            return newNote;
        }

        // Called when an item is returned to the pool using Release
        private void OnReturnToPool(GameNote note)
        {
            note.NoteView.gameObject.SetActive(false);
        }

        // Called when an item is taken from the pool using Get
        private void OnTakeFromPool(GameNote note)
        {
            note.gameObject.SetActive(true);
        }

        // If the pool capacity is reached then any items returned will be destroyed.
        // We can control what the destroy behavior does, here we destroy the GameObject.
        private void OnDestroyPoolObject(GameNote note)
        {
            Destroy(note.gameObject);
        }

    }
}*/