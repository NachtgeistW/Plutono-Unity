using Assets.Scripts.Util.FileManager;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Scenes.Welcome
{
    public class JumpToMainScene : MonoBehaviour
    {
        public void OnClick()
        {
            SceneManager.LoadScene("SongSelectScene");
        }
    }
}
