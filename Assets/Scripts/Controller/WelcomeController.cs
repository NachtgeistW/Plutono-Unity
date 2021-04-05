using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Controller
{
    public class WelcomeController : MonoBehaviour
    {
        public void OnClick()
        {
            SceneManager.LoadScene("SongSelectScene");
        }
    }
}
