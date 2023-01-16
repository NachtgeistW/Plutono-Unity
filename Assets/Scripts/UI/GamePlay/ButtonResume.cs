/* 
 * History:
 *      2023.01.14  CREATED
 */

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Contro the behaivour on resume bottom
/// </summary>
public class ButtonResume : MonoBehaviour
{
    public void OnClick()
    {
        EventHandler.CallGameResumeEvent();
    }
}
