using System.Collections;
using UnityEngine;

namespace Plutono.Level.Result
{
    public class ShareResult : MonoBehaviour
    {
        public void OnClick()
        {
            StartCoroutine(Capture());

            var share = new Utilities.Android.AndroidNativeShareAdapter();
            share.ShareWithText(message: "Greeting from Plutono");
        }

        private static IEnumerator Capture()
        {
            yield return new WaitForEndOfFrame();

            ScreenCapture.CaptureScreenshot("Screenshot.png");

            yield return new WaitForEndOfFrame();
        }

    }
}