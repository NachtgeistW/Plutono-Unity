/* 
 * History:
 *      2023.01.14  CREATED
 */

using UnityEngine;
using UnityEngine.UI;

namespace Plutono.UI
{
    /// <summary>
    /// Contro the behaivour on song select bottom
    /// </summary>
    public class ButtonSongSelect : MonoBehaviour
    {
        public void OnClick()
        {
            EventHandler.CallTransitionEvent("SongSelect");
        }
    }
}