using UnityEngine;
using System.Collections.Generic;
using Plutono.IO;

namespace Plutono
{
    public class FileManager : Singleton<FileManager>
    {
        [field: SerializeField] public string StoragePath { get; set; }
        public List<Song.SongDetail> songSourceList;
        public LoadFiles LoadFiles;
        private void Start()
        {
            LoadFiles = new LoadFiles();
            LoadFiles.RequestReadPermission();
            LoadFiles.LoadSongData(StoragePath).ForEach(song => songSourceList.Add(new Song.SongDetail(song)));

            GameData.PlayerScores.Instance.Initialize(songSourceList);
        }
    }
}