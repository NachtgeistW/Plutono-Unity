using UnityEngine;
using UnityEngine.SceneManagement;

using Util.FileManager;

namespace Controller
{
    public class WelcomeController : MonoBehaviour
    {
        [SerializeField]
        ResourceManger m_resourceManager;

        void Start()
        {
            m_resourceManager.RequestReadPermission();
            GameManager.Instance.songInfos = m_resourceManager.Initialize();
        }

        public void OnClick() => SceneManager.LoadScene("SongSelectScene");
    }
}
