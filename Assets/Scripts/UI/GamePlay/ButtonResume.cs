/* 
 * History:
 *      2023.01.14  CREATED
 */

using UnityEngine;
using UnityEngine.UI;

namespace Plutono.UI
{

    /// <summary>
    /// Control the behaivour on resume bottom
    /// </summary>
    public class ButtonResume : MonoBehaviour
    {
        [SerializeField] TextCountdown textCountdown;
        public void OnClick()
        {
            StartCoroutine(textCountdown.ResumeCountdown());
            //textCountdown.ResumeCountdown();
        }
    }
}
