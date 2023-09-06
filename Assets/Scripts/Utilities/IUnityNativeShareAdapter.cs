namespace Utilities
{
    public interface IUnityNativeShareAdapter
    {
        void ShareWithScreenshot(string filePath, string message = "", string mimeType = "image/*");
        void ShareWithText(string message, bool showShareDialogBox = false, string shareDialogBoxText = "");
        void ShareWithScreenshotAndText(string filePath, string message, bool showShareDialogBox = false, string shareDialogBoxText = "", string mimeType = "image/*");
    }
}