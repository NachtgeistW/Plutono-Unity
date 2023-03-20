using UnityEngine;
using Lean.Touch;
using System;
using static UnityEngine.UI.Image;

public class HitController : MonoBehaviour
{
    [SerializeField] private Camera gamePlayCamera;

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

            //��y���ϵ�ͶӰ
            ray.direction = Vector3.ProjectOnPlane(ray.direction, Vector3.up);
            //��ray������ƶ�������ĸ߶ȣ�y��
            ray.origin = new Vector3(ray.origin.x, ray.origin.y - gamePlayCamera.transform.position.y, ray.origin.z);
#if DEBUG
            Debug.DrawRay(ray.origin, ray.direction * 50, Color.red, 5);
#endif
            //TODO: delete raycast, use point of notes directly
            if (Physics.Raycast(ray, out RaycastHit hit, 50))
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
