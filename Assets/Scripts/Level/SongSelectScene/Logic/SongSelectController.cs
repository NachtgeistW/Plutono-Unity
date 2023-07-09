/*
 * History
 *      2020.08.12  CREATE.
 *      2021.04.04  RENAME to SongSelectController.
 *      2021.04.07  ADD function PopulateSong.
 *      2021.04.08  Import GameManager Singleton.
 */


using System.Collections.Generic;
using UnityEngine;

namespace Plutono.Level.SongSelectScene
{
    /// <summary>
    /// Control the events happened in Song Select Scene.
    /// </summary>
    public sealed class SongSelectController : MonoBehaviour
    {
        [Header("(放prefab不是script！)包含曲绘和曲名的button prefab。")]
        [SerializeField]
        private PrefabCoverView prefabCoverView;

        private void Start()
        {
            PopulateSong(FileManager.Instance.songSourceList);
        }

        /// <summary>
        /// Instantiate the name and cover of songs in songPackList using selected prefab.
        /// </summary>
        /// <param name="songDetails"></param>
        public void PopulateSong(List<Song.SongDetail> songDetails)
        {
            for (var i = 0; i < songDetails.Count; i++)
            {
                var newButton = Instantiate(prefabCoverView, transform);
                newButton.SetSongDetail(songDetails[i], i);
            }
        }
    }
}