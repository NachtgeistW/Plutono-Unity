//����note�����ɣ��ж�
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Plutono.Song
{
    public class NoteController : MonoBehaviour
    {
        public Note notePrefab;
        public Transform noteParent;
        public ObjectPool<Note> notePool;
        public bool collectionChecks = true;
        public int maxPoolSize = 10;

        private void OnEnable()
        {
            EventHandler.InstantiateLevel += InstantiateNote;
            EventHandler.HitNoteEvent += OnHitNoteEvent;
            EventHandler.ExecuteActionAfterNoteAnimate += OnExecuteActionAfterNoteAnimate;
        }

        private void OnDisable()
        {
            EventHandler.InstantiateLevel -= InstantiateNote;
            EventHandler.HitNoteEvent -= OnHitNoteEvent;
            EventHandler.ExecuteActionAfterNoteAnimate -= OnExecuteActionAfterNoteAnimate;
        }

        private void Start()
        {
            notePool = new ObjectPool<Note>(
                OnCreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPooledItem, collectionChecks, maxPoolSize);
        }

        private void InstantiateNote(List<NoteDetail> noteDetails, List<Note> notesOnScreen)
        {
            foreach (var noteDetail in noteDetails)
            {
                notePrefab._details = noteDetail;
                notesOnScreen.Add(notePool.Get());
            }
        }
        private void OnHitNoteEvent(List<Note> notesOnScreen, Note note, float curGameTime, GameStatus status)
        {
            //FIXME: Prevent judgment if there aren't any note on the screen
            var grade = NoteGradeJudgment.JudgeNoteGrade(note._details, curGameTime, status.Mode);
            status.Judge(note._details, grade, 0);
            notesOnScreen.Remove(note);
        }

        private void OnExecuteActionAfterNoteAnimate(Note note)
        {
            //TODO: Wait for the hitting animation to finish before releasing the note.
            notePool.Release(note);
        }

#region ObjectPool
        Note OnCreatePooledItem()
        {
            Note note = Instantiate(
                notePrefab, new Vector3(notePrefab._details.pos * 10, 0, Settings.maximumNoteRange),Quaternion.identity, noteParent);  
            return note;
        }

        // Called when an item is returned to the pool using Release
        void OnReturnedToPool(Note note)
        {
            note.gameObject.SetActive(false);
        }

        // Called when an item is taken from the pool using Get
        void OnTakeFromPool(Note note)
        {
            note.gameObject.SetActive(true);
            note.FallingDown();
        }

        void OnDestroyPooledItem(Note note)
        {
            Destroy(note.gameObject);
        }
#endregion
    }
}
