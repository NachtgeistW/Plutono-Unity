using System.IO;
using UnityEditor;

#if UNITY_2019
namespace LeTai.Asset.TranslucentImage.Editor
{
[InitializeOnLoad]
public class UnityShaderBugHack
{
    static UnityShaderBugHack()
    {
        var guids = AssetDatabase.FindAssets("l:TranslucentImageEditorResources lib");
        var path  = AssetDatabase.GUIDToAssetPath(guids[0]);
        var text  = File.ReadAllText(path);
        File.WriteAllText(path, text + "//DELETE ME: Workaround shader not compiling in Unity 2019");
        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        File.WriteAllText(path, text);
        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
    }
}
}
#endif
