using UnityEngine;

#if UNITY_ANDROID
namespace Utilities.Android
{
    public class AndroidNativeShareAdapter : IUnityNativeShareAdapter
    {
        private AndroidJavaClass intentClass;
        private AndroidJavaObject intentObject;

        private AndroidJavaClass uriClass;
        private AndroidJavaObject uriObject;

        private  AndroidJavaClass unityPlayerClass;

        public AndroidNativeShareAdapter()
        {

        }

        public void ShareWithScreenshot(string filePath, string message = "", string mimeType = "image/*")
        {
            //unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            //var currentActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");

            //intentClass = new AndroidJavaClass("android.content.Intent");
            //intentObject = new AndroidJavaObject("android.content.Intent");

            ////create file object of the screenshot captured
            //var fileObject = new AndroidJavaObject("java.io.File", filePath);

            ////create FileProvider class object
            //var fileProviderClass = new AndroidJavaClass("androidx.content.FileProvider");
            //var providerParams = new object[3];
            //providerParams[0] = currentActivity;
            //providerParams[1] = "com.plutono.unityclient.provider";
            //providerParams[2] = fileObject;

            ////instead of parsing the uri, 
            ////will get the uri from file using FileProvider
            //uriObject = fileProviderClass.CallStatic<AndroidJavaObject>("getUriForFile", providerParams);

            //intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
            //intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);
            //intentObject.Call<AndroidJavaObject>("setType", mimeType);

            //var chooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, "Share Via");
            //currentActivity.Call("startActivity", chooser);
        }

        public void ShareWithScreenshotAndText(string filePath, string message, bool showShareDialogBox = false, string shareDialogBoxText = "", string mimeType = "image/*")
        {
            throw new System.NotImplementedException();
        }

        public void ShareWithText(string message, bool showShareDialogBox = false, string shareDialogBoxText = "")
        {
            unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            var currentActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");

            intentClass = new AndroidJavaClass("android.content.Intent");
            intentObject = new AndroidJavaObject("android.content.Intent");

            intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
            intentObject.Call<AndroidJavaObject>("setType", "text/plain");
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), message);

            var chooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, "Share Via");
            currentActivity.Call("startActivity", chooser);
        }
    }
}
#endif
