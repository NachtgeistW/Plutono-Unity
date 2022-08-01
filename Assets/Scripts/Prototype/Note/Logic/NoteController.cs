//控制note的生成，判定
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
        }

        private void OnDisable()
        {
            EventHandler.InstantiateLevel -= InstantiateNote;
            EventHandler.HitNoteEvent -= OnHitNoteEvent;
        }

        private void Start()
        {
            notePool = new ObjectPool<Note>(
                OnCreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPooledItem, collectionChecks, maxPoolSize);
        }

        private void InstantiateNote(ChartDetail chartDetails)
        {
            foreach (var noteDetail in chartDetails.noteDetails)
            {
                notePrefab._details = noteDetail;
                notePool.Get();
            }
        }
        private void OnHitNoteEvent(Note note, float curGameTime, GameStatus status)
        {
            var grade = NoteGradeJudgment.JudgeNoteGrade(note._details, curGameTime, status.Mode);
            status.Judge(note._details, grade, 0);
            notePool.Release(note);
        }

        // ObjectPool

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
        }

        void OnDestroyPooledItem(Note note)
        {
            Destroy(note.gameObject);
        }
    }
}
