/* 
 * History:
 *      2023.01.14  CREATED
 */

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Contro the behaivour on restart bottom
/// </summary>
public class ButtonRestart : MonoBehaviour
{
    public void OnClick()
    {
        EventHandler.CallGameRestartEvent();
    }
}
