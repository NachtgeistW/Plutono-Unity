/*
 * class SongSelectSceneController -- Control the events happened in Song Select Scene.
 *
 * Function
 *      void::PopulateSong -- Instantiate the name and cover of songs in songPackList using selected prefab.
 *
 * History
 *      2020.08.12  CREATE.
 *      2021.04.04  RENAME to SongSelectController.
 *      2021.04.07  ADD function PopulateSong.
 *      2021.04.08  Import GameManager Singleton.
 */


using System.Collections.Generic;
using Assets.Scripts.Model.Plutono;
using Assets.Scripts.Util.FileManager;
using Assets.Scripts.Views;
using UnityEngine;

namespace Assets.Scripts.Controller
{
    public class SongSelectController : MonoBehaviour
    {
        [Header("(放prefab不是script！)包含曲绘和曲名的button prefab。")]
        [SerializeField] private PrefabCoverView prefabCoverView;

        private void Start()
        {
            PopulateSong(GameManager.Instance.songPackList);
        }

        public void PopulateSong(List<PackInfo> songPackList)
        {
            uint songIndex = 0;
            foreach (var packInfo in songPackList)
            {
                var newButton = Instantiate(prefabCoverView, transform);
                newButton.SetSongInfo(packInfo, songIndex);
                songIndex++;
            }
        }
    }
}