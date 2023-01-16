/* 
 * History:
 *      2022.07.27  CREATED
 */

using UnityEngine;
using TMPro;

namespace Plutono.UI
{
    public class UpperStatic : MonoBehaviour
    {
        public TMP_Text titleText;
        public TMP_Text modeText;
        public TMP_Text levelText;
        public GamePlayController gamePlayController;

        // Start is called before the first frame update
        void Start()
        {
            titleText.text = gamePlayController.SongSource.SongName;
            levelText.text = "Lv." + gamePlayController.ChartDetail.level;
            //mode
            switch (gamePlayController.Status.Mode)
            {
                case GameMode.Stelo:
                    modeText.text = "Stelo";
                    break;
                case GameMode.Arbo:
                    modeText.text = "Arbo";
                    modeText.color = Color.white;
                    break;
                case GameMode.Pluvo:
                    modeText.text = "Floro";
                    modeText.color = new Color(171, 149, 174);
                    break;
                case GameMode.Persona:
                    modeText.text = "Persona";
                    break;
                case GameMode.Ekzerco:
                    modeText.text = "Ekzerco";
                    break;
                default:
                    break;
            }
        }
    }
}