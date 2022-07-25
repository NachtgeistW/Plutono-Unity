/* 
 * class PianoNoteView -- Store the appearance of a piano note in game.
 *
 *      This class includes the id, type, position, size, time, piano sounds property.
 *
 * History:
 *      2021.12.20  CREATED
 */

using UnityEngine;

namespace Plutono.Song
{
    public class ExplosionRenderer : MonoBehaviour
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

