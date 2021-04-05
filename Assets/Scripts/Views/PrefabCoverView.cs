using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Views
{
    public class PrefabCoverView : MonoBehaviour
    {
        public void JumpToChartSelectScene()
        {
            SceneManager.LoadScene("ChartSelectScene");
        }
    }
}
