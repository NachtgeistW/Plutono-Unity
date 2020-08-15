using UnityEngine;
using UnityEngine.SceneManagement;

public class JumpToMainScene : MonoBehaviour
{
    public void OnClick()
    {
        SceneManager.LoadScene("SongSelectScene");
    }
}
