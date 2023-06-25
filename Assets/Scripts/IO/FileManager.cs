using UnityEngine;
using System.Collections.Generic;

namespace Plutono.IO
{
    
    public class FileManager : Singleton<FileManager>
    {
        [field: SerializeField] public string StoragePath { get; set; }
        public List<Song.SongDetail> songSourceList;
        public LoadFiles loadFiles;
        private void Start()
        {
            loadFiles = new LoadFiles();
            loadFiles.RequestReadPermission();
            loadFiles.LoadSongData(StoragePath).ForEach(song => songSourceList.Add(new Song.SongDetail(song)));
        }
    }
}