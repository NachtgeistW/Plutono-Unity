using System.Collections;
using UnityEngine;

//TODO:双击返回
namespace Util
{
    public class ClickTwiceToExit : MonoBehaviour
    {
        public GameObject quitObject;
        private bool _clickedBefore = false;

        void Update()
        {
            //Check input for the first time
            if (!Input.GetKeyDown(KeyCode.Escape) || _clickedBefore) return;
            Debug.Log("Back Button pressed for the first time");
            //Set to false so that this input is not checked again. It will be checked in the coroutine function instead
            _clickedBefore = true;

            //Activate Quit Object
            quitObject.SetActive(true);

            //Start quit timer
            StartCoroutine(QuittingTimer());
        }

        private IEnumerator QuittingTimer()
        {
            //Wait for a frame so that Input.GetKeyDown is no longer true
            yield return null;

            //3 seconds timer
            const float timerTime = 3f;
            float counter = 0;

            while (counter < timerTime)
            {
                //Increment counter while it is < timer time(3)
                counter += Time.deltaTime;

                //Check if Input is pressed again while timer is running then quit/exit if is
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Debug.Log("Back Button pressed for the second time. EXITING.....");
                    Quit();
                }

                //Wait for a frame so that Unity does not freeze
                yield return null;
            }

            Debug.Log("Timer finished...Back Button was NOT pressed for the second time within: '" + timerTime + "' seconds");

            //Timer has finished and NO QUIT(NO second press) so deactivate
            quitObject.SetActive(false);
            //Reset clickedBefore so that Input can be checked again in the Update function
            _clickedBefore = false;
        }

        static void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
    //Application.Quit();
    System.Diagnostics.Process.GetCurrentProcess().Kill();
#endif
        }
    }
}