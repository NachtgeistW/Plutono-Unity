/* 
 * History:
 *      2023.01.14  CREATED
 */

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Plutono.UI
{
    /// <summary>
    /// Contro the behaivour on chart select bottom
    /// </summary>
    public class TextCountdown : MonoBehaviour
    {
        [SerializeField] TMPro.TMP_Text textCountdown;

        private void Start()
        {
            StartCoroutine(StartCountdown());
        }

        public IEnumerator StartCountdown()
        {
            yield return new WaitForSecondsRealtime(1.0f);

            textCountdown.text = "Start";
            yield return new WaitForSecondsRealtime(1.0f);
            textCountdown.gameObject.SetActive(false);
            EventHandler.CallGameStartEvent();
        }

        public IEnumerator ResumeCountdown()
        {
            textCountdown.gameObject.SetActive(true);
            textCountdown.text = "Music";
            yield return new WaitForSecondsRealtime(1.0f);

            textCountdown.text = "Resume";
            yield return new WaitForSecondsRealtime(1.0f);
            textCountdown.gameObject.SetActive(false);
            EventHandler.CallGameResumeEvent();
        }
    }
}