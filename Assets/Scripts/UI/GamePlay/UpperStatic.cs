/* 
 * History:
 *      2022.07.27  CREATED
 */

using UnityEngine;
using TMPro;
using Plutono.GamePlay;

namespace Plutono.UI
{
    public class UpperStatic : MonoBehaviour
    {
        public TMP_Text titleText;
        public TMP_Text modeText;
        public TMP_Text levelText;

        void Start()
        {
            titleText.text = GamePlayController.Instance.SongSource.songName;
            levelText.text = "Lv." + GamePlayController.Instance.ChartDetail.level;
            //mode
            switch (GamePlayController.Instance.Status.Mode)
            {
                case GameMode.Stelo:
                    modeText.text = "Stelo";
                    break;
                case GameMode.Arbo:
                    modeText.text = "Arbo";
                    break;
                case GameMode.Floro:
                    modeText.text = "Floro";
                    //modeText.color = new Color(171, 149, 174);
                    break;
                case GameMode.Autoplay:
                    modeText.text = "Auto";
                    break;
                default:
                    break;
            }
        }
    }
}