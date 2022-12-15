namespace Plutono.IO
{
    using System.Collections.Generic;
    using UnityEngine;
    
    public class FileManager : MonoBehaviour
    {
        public List<Legacy.SongInfo> list;
        public string StoragePath;

        private LoadFiles loadFiles;
        private void Start()
        {
            loadFiles = new LoadFiles();
            loadFiles.RequestReadPermission();
            list = loadFiles.Initialize(StoragePath);
        }
    }
}