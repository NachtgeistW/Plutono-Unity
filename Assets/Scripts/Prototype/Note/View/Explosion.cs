/* 
 * History:
 *      2022.07.22  CREATED
 */

using UnityEngine;

namespace Plutono.Song
{
    //class Explosion -- Control the note explosion effect on hitting.
    public class Explosion : MonoBehaviour
    {
        [SerializeField] Animator explosionAnim;

        private void OnEnable()
        {
            EventHandler.HitNoteEvent += OnHitNoteEvent;
        }

        private void OnDisable()
        {
            EventHandler.HitNoteEvent -= OnHitNoteEvent;
        }

        private void OnHitNoteEvent(Note note)
        {
            explosionAnim.SetBool("IsHit", true);
        }
    }
}

