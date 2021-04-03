using System.Collections.Generic;
using Assets.Scripts.Game.Plutono;
using Assets.Scripts.Game.Song;
using Assets.Scripts.Util.FileManager;
using UnityEngine;

namespace Assets.Scripts.Scenes.SongSelect
{
    public class SongSelectScene : MonoBehaviour
    {
        public List<PackInfo> songPackList = new List<PackInfo>();
        void Awake()
        {
            var resourceManager = gameObject.AddComponent<ResourceManger>();
            resourceManager.RequestReadPermission();
            resourceManager.InitializeApplication(songPackList);
        }
    }
}