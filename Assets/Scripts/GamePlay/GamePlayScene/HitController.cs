using UnityEngine;
using Lean.Touch;
using System;
using static UnityEngine.UI.Image;

public class HitController : MonoBehaviour
{
    [SerializeField] private Camera gamePlayCamera;

    public void EnableInput()
    {
        LeanTouch.OnFingerDown += OnFingerDown;
        LeanTouch.OnFingerUpdate += OnFingerUpdate;
        LeanTouch.OnFingerUp += OnFingerUp;
    }

    public void DisableInput()
    {
        LeanTouch.OnFingerDown -= OnFingerDown;
        LeanTouch.OnFingerUpdate -= OnFingerUpdate;
        LeanTouch.OnFingerUp -= OnFingerUp;
    }

    protected void OnFingerDown(LeanFinger finger)
    {
        var hitPosition = gamePlayCamera.ScreenPointToRay(finger.ScreenPosition);
        if (hitPosition.direction.y > -0.4) return;

        //Projection on Y axis
        hitPosition.direction = Vector3.ProjectOnPlane(hitPosition.direction, Vector3.up);
        //将ray的起点移动到相机的高度（y轴
        hitPosition.origin = new Vector3(hitPosition.origin.x, hitPosition.origin.y - gamePlayCamera.transform.position.y, hitPosition.origin.z);

        //Detect Slide first
        if (Physics.Raycast(hitPosition, out RaycastHit hit, 500))
        {
            if (hit.collider.TryGetComponent(out Plutono.Song.Note note))
            {
                if (note._details.type == Plutono.Song.NoteType.Slide)
                {
                    //note.OnSlideStart();
                }
            }
        }
    }

    private void OnFingerUpdate(LeanFinger finger)
    {
        throw new NotImplementedException();
    }

    private void OnFingerUp(LeanFinger finger)
    {
        throw new NotImplementedException();
    }

    //public void HandleFingerTap(LeanFinger finger, Plutono.Song.Note note)
    //{
    //    var ray = gamePlayCamera.ScreenPointToRay(finger.ScreenPosition);
    //    //limit the raycast area to the bottom half of the screen
    //    if (ray.direction.y > -0.4) return;
    //    var hit = default(RaycastHit);
    //    Debug.Log("You just tapped the screen with finger " + finger.Index + " at " + finger.ScreenPosition + "(" + ray.direction);
    //}

    /// <summary>
    /// Check if player hit a note
    /// </summary>
    /// <param name="finger"></param>
    /// <returns>The hitted note. null if none</returns>
    public bool IsHittedNote(LeanFinger finger, out Plutono.Song.Note note)
    {
        note = null;
        if (finger.IsOverGui == false)
        {
            var ray = gamePlayCamera.ScreenPointToRay(finger.ScreenPosition);
            if (ray.direction.y > -0.4) return false;

            //在y轴上的投影
            ray.direction = Vector3.ProjectOnPlane(ray.direction, Vector3.up);
            //将ray的起点移动到相机的高度（y轴
            ray.origin = new Vector3(ray.origin.x, ray.origin.y - gamePlayCamera.transform.position.y, ray.origin.z);
#if DEBUG
            Debug.DrawRay(ray.origin, ray.direction * 500, Color.red, 5);
#endif
            if (Physics.Raycast(ray, out RaycastHit hit, 500))
            {
                if (hit.collider.TryGetComponent(out note))
                {
                    return true;
                }
            }
        }
        return false;
    }
}
