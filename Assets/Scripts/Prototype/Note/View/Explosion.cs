/* 
 * History:
 *      2022.07.22  CREATED
 */

using System.Collections.Generic;
using UnityEngine;

namespace Plutono.Song
{
    //class Explosion -- Control the note explosion effect on hitting.
    public class Explosion : MonoBehaviour
    {
        private void OnEnable()
        {
            EventHandler.HitNoteEvent += OnHitNoteEvent;
        }

        private void OnDisable()
        {
            EventHandler.HitNoteEvent -= OnHitNoteEvent;
        }

        private void OnHitNoteEvent(List<Note> notesOnScreen, Note note, float curGameTime, GameStatus status)
        {
            var explosionAnim = note.GetComponent<Animator>();
            explosionAnim.SetBool("IsHit", true);
        }
    }
}

