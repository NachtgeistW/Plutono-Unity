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
 */


using System.Collections.Generic;
using Assets.Scripts.Model.Plutono;
using Assets.Scripts.Util.FileManager;
using Assets.Scripts.Views;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Controller
{
    public class SongSelectController : MonoBehaviour
    {
        private List<PackInfo> _songPackList = new List<PackInfo>();
        [Header("(放prefab不是script！)包含曲绘和曲名的button prefab。")]
        [SerializeField]private PrefabCoverView _prefabCoverView;

        void Start()
        {
            var resourceManager = new ResourceManger();
            resourceManager.RequestReadPermission();
            resourceManager.InitializeApplication(_songPackList);

            PopulateSong();
        }

        public void PopulateSong()
        {
            foreach (var packInfo in _songPackList)
            {
                var newButton = Instantiate(_prefabCoverView, transform);
                newButton.SetSongInfo(packInfo);
            }
        }
    }
}