using UnityEngine;

#if UNITY_ANDROID
namespace Utilities.Android
{
    public class AndroidNativeShareAdapter : IUnityNativeShareAdapter
    {
        private readonly AndroidJavaClass intentClass;
        private readonly AndroidJavaObject intentObject;

        private readonly AndroidJavaClass uriClass;
        private AndroidJavaObject uriObject;

        private readonly AndroidJavaClass unityPlayerClass;

        public AndroidNativeShareAdapter()
        {
            intentClass = new AndroidJavaClass("android.content.Intent");
            intentObject = new AndroidJavaObject("android.content.Intent");

            uriClass = new AndroidJavaClass("android.net.Uri");

            unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

        }

        public void ShareWithScreenshot(string filePath, string message = "", string mimeType = "image/png")
        {
            uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "file://" + filePath);
            
            intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));

            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);
            intentObject.Call<AndroidJavaObject>("setType", mimeType);

            var currentActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
            var chooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject);
            currentActivity.Call("startActivity", chooser);
        }

        public void ShareWithScreenshotAndText(string filePath, string message, bool showShareDialogBox = false, string shareDialogBoxText = "", string mimeType = "image/*")
        {
            throw new System.NotImplementedException();
        }

        public void ShareWithText(string message, bool showShareDialogBox = false, string shareDialogBoxText = "")
        {
            throw new System.NotImplementedException();
        }
    }
}
#endif
