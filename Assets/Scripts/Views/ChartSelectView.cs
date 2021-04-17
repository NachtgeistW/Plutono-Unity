/*
 * class PrefabCoverView -- Control the events happened in Song Select Scene.
 *
 * Function
 *      void::SetSongInfo -- Set the cover and song name to a prefab.
 *      void::JumpToChartSelectScene -- Jump to chart select scene.
 *
 * History
 *      2021.04.08  CREATE.
 */

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Views
{
    public class ChartSelectView : MonoBehaviour
    {
        [SerializeField] private Text _songName;
        [SerializeField] private Text _composer;
        [SerializeField] private Text _score;
        
        public void SetSongName(string songName)
        {
            _songName.text = songName;
        }
        public void SetComposer(string composer)
        {
            _composer.text = composer;
            Log.LogStr(composer);
        }
    }
}