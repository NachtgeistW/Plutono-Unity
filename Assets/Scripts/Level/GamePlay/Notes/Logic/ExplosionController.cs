/*
 * History:
 *      2023.01.29  CREATED
 */

using Plutono.GamePlay;
using System.Collections;
using Plutono.GamePlay.Notes;
using Plutono.Util;
using UnityEngine;
using UnityEngine.Pool;
using Plutono.Level.GamePlay;

namespace Plutono.Song
{

    public class ExplosionController : MonoBehaviour
    {
        public Explosion explosionAnimPrefab;
        public Transform explosionAnimParent;
        public ObjectPool<Explosion> explosionAnimPool;
        public bool collectionChecks = true;
        public int maxPoolSize;

        public GamePlayController gamePlayController;

        private void OnEnable()
        {
            EventCenter.AddListener<NoteClearEvent<BlankNote>>(OnNoteClear);
            EventCenter.AddListener<NoteClearEvent<PianoNote>>(OnNoteClear);
            EventCenter.AddListener<NoteClearEvent<SlideNote>>(OnNoteClear);
        }

        private void Start()
        {
            //maxPoolSize = PlayerSettingsManager.Instance.PlayerSettings_Global_SO.ExplosionAnimateObjectpoolMaxSize;
            maxPoolSize = 250;
            explosionAnimPool = new ObjectPool<Explosion>(
                OnCreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPooledItem, collectionChecks, maxPoolSize);
        }

        private void OnNoteClear(NoteClearEvent<BlankNote> evt) => OnHitNote(evt.Note, evt.Grade);
        private void OnNoteClear(NoteClearEvent<PianoNote> evt) => OnHitNote(evt.Note, evt.Grade);
        private void OnNoteClear(NoteClearEvent<SlideNote> evt) => OnHitNote(evt.Note, evt.Grade);

        public void OnHitNote(BaseNote note, NoteGrade noteGrade)
        {
            float zPos;
            switch (noteGrade)
            {
                case NoteGrade.Perfect:
                    zPos = Settings.judgeLinePosition;
                    break;
                case NoteGrade.None:
                    //Do nothing
                    return;
                default:
                    //The current position of this note
                    zPos = note.gameObject.transform.position.z;
                    break;
            }

            var obj = explosionAnimPool.Get();
            obj.transform.position = new Vector3(note.pos, 0, zPos);
            obj.transform.localScale = new Vector3((float)note.size, 1, 1);
            obj.PlayAnimation(noteGrade, (float)note.size);
            StartCoroutine(ReleaseEnumerator(obj));
        }

        private IEnumerator ReleaseEnumerator(Explosion explosion)
        {
            yield return new WaitForSeconds(Settings.noteAnimationPlayingTime);
            explosionAnimPool.Release(explosion);
        }

        #region ObjectPool
        Explosion OnCreatePooledItem()
        {
            return Instantiate(
                explosionAnimPrefab, new Vector3(0, 0, 0), Quaternion.identity, explosionAnimParent);
        }

        // Called when an item is returned to the pool using Release
        void OnReturnedToPool(Explosion explosion)
        {
            explosion.gameObject.SetActive(false);
        }

        // Called when an item is taken from the pool using Get
        void OnTakeFromPool(Explosion explosion)
        {
            //Set the new data of the note
            //Activate the note and make it fall down
            explosion.gameObject.SetActive(true);
        }

        void OnDestroyPooledItem(Explosion explosion)
        {
            Destroy(explosion.gameObject);
        }
        #endregion

    }
}