/*
 * class PrefabCoverView -- Store the object and function that change UI on the Prefab ButtonSongCover.
 *      _songName: private Text, the song name attached to this button.
 *      PackInfoOnButton: public PackInfo, property, the song pack info attached to this button.
 *
 * History
 *      2021.04.07  CREATE.
 *      2021.04.08  REFACTOR with GameManager Singleton.
 */

using Plutono.GamePlay;
using UnityEngine;
using UnityEngine.UI;

namespace Plutono.Level.SongSelectScene
{
    public class PrefabCoverView : MonoBehaviour
    {
        [SerializeField] private Text songName;

        [SerializeField] private Image cover;

        public Song.SongDetail SongDetailOnButton { get; set; }

        public int songIndex;

        /// <summary>
        /// Set the cover and song name to a prefab.
        /// </summary>
        /// <param name="songDetail"></param>
        public void SetSongDetail(Song.SongDetail songDetail, int songIndex)
        {
            songName.text = songDetail.SongName;
            SongDetailOnButton = songDetail;
            cover.sprite = songDetail.Cover;
            this.songIndex = songIndex;
        }

        /// <summary>
        /// Jump to chart select scene.
        /// </summary>
        public void JumpToChartSelectScene()
        {
            SongSelectDataTransformer.SelectedSongIndex = songIndex;
            EventHandler.CallTransitionEvent("ChartSelect");
        }
    }
}
