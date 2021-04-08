/*
 * class PrefabCoverView -- Control the events happened in Song Select Scene.
 *
 * Function
 *      void::SetSongInfo -- Set the cover and song name to a prefab.
 *      void::JumpToChartSelectScene -- Jump to chart select scene.
 *
 * History
 *      2020.08.12  CREATE.
 *      2021.04.04  RENAME to SongSelectController.
 *      2021.04.04  ADD function PopulateSong.
 *      2021.04.08  Import GameManager Singleton.
 */

using Assets.Scripts.Model.Plutono;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Views
{
    public class PrefabCoverView : MonoBehaviour
    {
        [SerializeField] private Text _text;
        private uint songIndex;
        public void SetSongInfo(PackInfo packInfo, uint index)
        {
            _text.text = packInfo.songName;
            songIndex = index;
        }

        public void JumpToChartSelectScene()
        {
            GameManager.Instance.songIndex = songIndex;
            SceneManager.LoadScene("ChartSelectScene");
        }
    }
}
