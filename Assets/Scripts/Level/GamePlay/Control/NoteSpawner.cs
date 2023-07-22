//����note�����ɣ��ж�
// Factory pattern?

using Plutono.GamePlay;
using Plutono.GamePlay.Notes;
using Plutono.Level.GamePlay;
using Plutono.Util;
using UnityEngine;
using UnityEngine.Pool;

namespace Plutono.Song
{
    public class NoteSpawner<TNote> : MonoBehaviour, INoteSpawner<TNote>
        where TNote : BaseNote
    {
        [Space(10)]
        [Header("Prefab")]
        public TNote notePrefab;

        public Transform noteParent;
        public ObjectPool<TNote> notePool;
        public bool collectionChecks = true;
        public int maxPoolSize;

        [SerializeField] protected GamePlayController game;

        #region UnityEvent

        private void Start()
        {
            //TODO: Player Settings
            maxPoolSize = 1000;
            //maxPoolSize = PlayerSettingsManager.Instance.PlayerSettings_Global_SO.NoteObjectpoolMaxSize;
            notePool = new ObjectPool<TNote>(
                OnCreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPooledItem, collectionChecks, maxPoolSize);
        }

        #endregion

        public TNote SpawnNotes(NoteDetail noteDetail)
        {
            notePrefab.SetData(noteDetail);
            return notePool.Get();
        }

        public virtual void OnNoteClear(NoteClearEvent<TNote> evt)
        {
            notePool.Release(evt.Note);
        }

        public virtual void OnNoteMiss(NoteMissEvent<TNote> evt)
        {
            Debug.Log("NoteSpawner OnNoteMiss noteId: " + evt.Note.id);
            notePool.Release(evt.Note);
        }

        #region ObjectPool
        private TNote OnCreatePooledItem()
        {
            //Note note = Instantiate(
            //    notePrefab, new Vector3((float)(notePrefab._details.pos * Settings.perspectiveHorizontalScale), 0, Settings.maximumNoteRange), Quaternion.identity, noteParent);
            var note = Instantiate(
                notePrefab, new Vector3(notePrefab.pos, 0, Settings.maximumNoteRange), Quaternion.identity, noteParent);
            return note;
        }

        // Called when an item is returned to the pool using Release
        private void OnReturnedToPool(TNote note)
        {
            note.gameObject.SetActive(false);
        }

        // Called when an item is taken from the pool using Get
        private void OnTakeFromPool(TNote note)
        {
            //Set the new data of the note
            note.SetData(notePrefab.id, notePrefab.pos, notePrefab.time, notePrefab.size);
            //Activate the note and make it fall down
            note.gameObject.SetActive(true);
        }

        private void OnDestroyPooledItem(TNote note)
        {
            Destroy(note.gameObject);
        }
        #endregion
    }
}
