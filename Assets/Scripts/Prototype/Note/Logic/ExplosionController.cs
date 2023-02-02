/*
 * History:
 *      2023.01.29  CREATED
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Plutono.Song
{

    public class ExplosionController : MonoBehaviour
    {
        private List<GameObject> explosionAnims;
        public Explosion explosionAnimPrefab;
        public Transform explosionAnimParent;
        public ObjectPool<Explosion> explosionAnimPool;
        public bool collectionChecks = true;
        public int maxPoolSize = PlayerSettingsManager.Instance.PlayerSettings_Global_SO.ExplosionAnimateObjectpoolMaxSize;

        public GamePlayController gamePlayController;

        private void Awake()
        {
            explosionAnimPool = new ObjectPool<Explosion>(
                OnCreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPooledItem, collectionChecks, maxPoolSize);
        }

        public void OnHitNote(Note note, NoteGrade noteGrade)
        {
            if (!note._details.IsShown)
                return;
            var xPos = (float)(note._details.pos * 10);
            float zPos;
            switch (noteGrade)
            {
                case NoteGrade.Perfect:
                    zPos = Settings.judgeLightPosition;
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
            obj.transform.position = new Vector3(xPos, 0, zPos);

            obj.PlayAnimation(noteGrade);
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