using Assets.Scripts.Util.FileManager;
using UnityEngine;

namespace Assets.Scripts.Scenes.SongSelect
{
    public class SongSelectScene : MonoBehaviour
    {
        void Awake()
        {
            var resourceManager = gameObject.AddComponent<ResourceManger>();
            resourceManager.RequestReadPermission();
            resourceManager.InitializeApplication();
        }
    }
}