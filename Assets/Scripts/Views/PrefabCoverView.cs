/*
 * class PrefabCoverView -- Store the object and function that change UI on the Prefab ButtonSongCover.
 *      _songName: private Text, the song name attached to this button.
 *      PackInfoOnButton: public PackInfo, property, the song pack info attached to this button.
 *
 * Function
 *      void::SetSongInfo -- Set the cover and song name to a prefab.
 *      void::JumpToChartSelectScene -- Jump to chart select scene.
 *
 * History
 *      2021.04.07  CREATE.
 *      2021.04.08  REFACTOR with GameManager Singleton.
 */

using Model.Plutono;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Views
{
    public class PrefabCoverView : MonoBehaviour
    {
        [SerializeField] private Text songName;

        public PackInfo PackInfoOnButton { get; set; }
        public string MusicSourcePath { get; set; }

        public void SetSongInfo(PackInfo packInfo, string musicPath)
        {
            songName.text = packInfo.songName;
            PackInfoOnButton = packInfo;
            MusicSourcePath = musicPath;
        }

        public void JumpToChartSelectScene()
        {
            GameManager.Instance.packInfo = PackInfoOnButton;
            GameManager.Instance.songPath = MusicSourcePath;
            SceneManager.LoadScene("ChartSelectScene");
        }
    }
}
