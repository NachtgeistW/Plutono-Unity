//����note�����ɣ��ж�
using DG.Tweening;
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
        public int maxPoolSize = PlayerSettingsManager.Instance.PlayerSettings_Global_SO.NoteObjectpoolMaxSize;

        public GamePlayController gamePlayController;

        private void Awake()
        {
            notePool = new ObjectPool<Note>(
                OnCreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPooledItem, collectionChecks, maxPoolSize);
        }

        public void InstantiateNote(List<NoteDetail> noteDetails, List<Note> notesOnScreen)
        {
            foreach (var noteDetail in noteDetails)
            {
                notePrefab._details = noteDetail;
                notesOnScreen.Add(notePool.Get());
            }
        }

        public void OnHitNote(List<Note> notesOnScreen, Note note)
        {
            //FIXME: Prevent judgment if there aren't any note on the screen
            note.ForceStopAnimation();
            notePool.Release(note);
            notesOnScreen.Remove(note);
        }

        public void OnMissNote(List<Note> notesOnScreen, Note note)
        {
            notePool.Release(note);
            notesOnScreen.Remove(note);
        }

        #region ObjectPool
        Note OnCreatePooledItem()
        {
            Note note = Instantiate(
                //notePrefab, new Vector3(notePrefab._details.pos * 10, 0, Settings.maximumNoteRange / notePrefab._details.time * Settings.NoteFallTime(gamePlayController.Status.ChartPlaySpeed)),Quaternion.identity, noteParent);  
                notePrefab, new Vector3((float)(notePrefab._details.pos * 10), 0, Settings.maximumNoteRange), Quaternion.identity, noteParent);
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
            //Set the new data of the note
            note._details = notePrefab._details;
            note.SetSpriteRenderer();
            //Activate the note and make it fall down
            note.gameObject.SetActive(true);
            note.FallingDownAnimation();
        }

        void OnDestroyPooledItem(Note note)
        {
            Destroy(note.gameObject);
        }
        #endregion
    }
}
