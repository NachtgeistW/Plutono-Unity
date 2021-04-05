/*
 * class SongSelectSceneController -- Control the events happened in Song Select Scene.
 *
 * Function
 *      void::PopulateSong -- Instantiate the name and cover of songs in songPackList using selected prefab.
 *
 * History
 *      2020.08.12  CREATE.
 *      2021.04.04  RENAME to SongSelectController.
 *      2021.04.04  ADD function PopulateSong.
 */


using System.Collections.Generic;
using Assets.Scripts.Model.Plutono;
using Assets.Scripts.Util.FileManager;
using Assets.Scripts.Views;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Controller
{
    public class SongSelectController : MonoBehaviour
    {
        private List<PackInfo> _songPackList = new List<PackInfo>();

        public GameObject songCoverPrefab; // This is our prefab object that will be exposed in the inspector

        void Awake()
        {
            var resourceManager = gameObject.AddComponent<ResourceManger>();
            resourceManager.RequestReadPermission();
            resourceManager.InitializeApplication(_songPackList);

            PopulateSong();
        }

        public void PopulateSong()
        {
            foreach (var t in _songPackList)
            {
                var newButton = Instantiate(songCoverPrefab, transform);
                newButton.GetComponentInChildren<Text>().text = t.songName;
            }
        }
    }
}