using System.Collections.Generic;
using Assets.Scripts.Model.Plutono;
using Assets.Scripts.Util.FileManager;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Controller
{
    public class WelcomeController : MonoBehaviour
    {
        private void Start()
        {
            InitializeSongList();
        }

        public void OnClick()
        {
            SceneManager.LoadScene("SongSelectScene");
        }

        public void InitializeSongList()
        {
            var resourceManager = new ResourceManger();
            resourceManager.RequestReadPermission();
            GameManager.Instance.songPackList = resourceManager.InitializeApplication();
        }
    }
}
