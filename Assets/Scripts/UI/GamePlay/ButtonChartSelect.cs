/* 
 * History:
 *      2023.01.14  CREATED
 */

using UnityEngine;
using UnityEngine.UI;

namespace Plutono.UI
{
    /// <summary>
    /// Contro the behaivour on chart select bottom
    /// </summary>
    public class ButtonChartSelect : MonoBehaviour
    {
        public void OnClick()
        {
            EventHandler.CallTransitionEvent("ChartSelect");
        }
    }
}