namespace Plutono.IO
{
    using System.Collections.Generic;
    
    public class FileManager : Singleton<FileManager>
    {
        public string StoragePath;
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