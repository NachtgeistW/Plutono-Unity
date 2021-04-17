using UnityEngine;
using UnityEngine.SceneManagement;
using Util.FileManager;

namespace Controller
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
