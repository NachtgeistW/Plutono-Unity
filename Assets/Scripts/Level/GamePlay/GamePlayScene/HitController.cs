using UnityEngine;
using Lean.Touch;
using Plutono.Song;

namespace Plutono.GamePlay
{
    public class HitController : MonoBehaviour
    {
        [SerializeField] private Camera orthoCam;

        /// <summary>
        /// Check if player hit a note
        /// </summary>
        /// <param name="finger"></param>
        /// <param name="touchTime">the time when player touches the screen</param>
        /// <param name="note">The hitted note. null if none</param>
        /// <returns>True if hit note. </returns>
        public bool TryHitNote(LeanFinger finger, double touchTime, out Note note, out NoteGrade grade)
        {
            note = null;
            grade = NoteGrade.None;
            if (finger.IsOverGui)
                return false;
            
            var pos = orthoCam.ScreenToWorldPoint(new Vector3(finger.ScreenPosition.x, finger.ScreenPosition.y, orthoCam.nearClipPlane));
            if (pos.y < 0.6) return false;

            var lastDeltaXPos = float.MaxValue;
            var mode = GamePlayController.Instance.Status.Mode;
            foreach (var curDetectingNote in GamePlayController.Instance.notesOnScreen)
            {
                if (curDetectingNote.IsHitted(pos.x, touchTime, mode, out var deltaTime, out var deltaXPos))
                {
                    var curNoteGrade = NoteGradeJudgment.Judge(deltaTime, mode);
                    if (curNoteGrade > grade)
                    {
                        note = curDetectingNote;
                        lastDeltaXPos = deltaXPos;
                        grade = curNoteGrade;
                    }
                    else if (curNoteGrade == grade && deltaXPos < lastDeltaXPos)
                    {
                        note = curDetectingNote;
                        lastDeltaXPos = deltaXPos;
                        grade = curNoteGrade;
                    }
                }
            }
            if (note == null || grade < NoteGrade.Bad) return false;
            else return true;
        }

        //        public bool IsHittedNote(LeanFinger finger, double hitTime, out Note note)
        //        {
        //            note = null;
        //            if (finger.IsOverGui == false)
        //            {
        //                var ray = gamePlayCamera.ScreenPointToRay(finger.ScreenPosition);
        //                if (ray.direction.y > -0.4) return false;

        //                //在y轴上的投影
        //                ray.direction = Vector3.ProjectOnPlane(ray.direction, Vector3.up);
        //                //将ray的起点移动到相机的高度（y轴
        //                ray.origin = new Vector3(ray.origin.x, ray.origin.y - gamePlayCamera.transform.position.y, ray.origin.z);
        //#if DEBUG
        //                Debug.DrawRay(ray.origin, ray.direction * 50, Color.red, 5);
        //#endif
        //                //TODO: delete raycast, use point of notes directly
        //                if (Physics.Raycast(ray, out RaycastHit hit, 50))
        //                {
        //                    if (hit.collider.TryGetComponent(out note))
        //                    {
        //                        return true;
        //                    }
        //                }
        //            }
        //            return false;
        //        }

        /// <summary>
        /// Search for the best note when player hit the screen
        /// </summary>
        /// <param name="touchTime">the time when player touches the screen</param>
        //Note SearchForBestNoteOnTime(List<Note> notesOnScreen, double touchTime)
        //{
        //    if (notesOnScreen.Count == 0)
        //    {
        //        return null;
        //    }
        //    Note bestNote = null;
        //    foreach (var curDetectingNote in notesOnScreen)
        //    {
        //        if (bestNote == null)
        //            bestNote = curDetectingNote;
        //        var curDeltaTime = Math.Abs(touchTime - curDetectingNote._details.time);
        //        var bestDeltaTime = Math.Abs(touchTime - bestNote._details.time);
        //        if (bestDeltaTime > curDeltaTime)
        //        {
        //            bestNote = curDetectingNote;
        //            continue;
        //        }
        //        if (curDeltaTime > GetClosedBestNoteRange(bestDeltaTime))
        //            return bestNote;
        //    }
        //    //Bestnote not found, return null
        //    return null;
        //}

    }
}