/*
 * class PrefabCoverView -- Store the object and function that change UI on the Prefab ButtonSongCover.
 *      _songName: private Text, the song name attached to this button.
 *      PackInfoOnButton: public PackInfo, property, the song pack info attached to this button.
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

        [SerializeField] private Image cover;

        public SongInfo PackInfoOnButton { get; set; }

        /// <summary>
        /// Set the cover and song name to a prefab.
        /// </summary>
        /// <param name="songInfo"></param>
        public void SetSongInfo(SongInfo songInfo)
        {
            songName.text = songInfo.SongName;
            PackInfoOnButton = songInfo;
            cover.sprite = songInfo.Cover;
            Debug.Log(songInfo.SongName);
        }

        /// <summary>
        /// Jump to chart select scene.
        /// </summary>
        public void JumpToChartSelectScene()
        {
            GameManager.Instance.SongInfo = PackInfoOnButton;
            SceneManager.LoadScene("ChartSelectScene");
        }
    }
}
