using Assets.Scripts.Util.FileManager;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Scenes.Welcome
{
    public class JumpToMainScene : MonoBehaviour
    {
        private void Awake()
        {
            var resourceManager = gameObject.AddComponent<ResourceManger>();
            resourceManager.RequestReadPermission();
            resourceManager.InitializeApplication();
        }

        public void OnClick()
        {
            SceneManager.LoadScene("SongSelectScene");
        }
    }
}
