using UnityEngine;
using UnityEngine.UI;

namespace Plutono.UI
{
    public class TextCombo : MonoBehaviour
    {
        public Text comboText;
        public GamePlayController gamePlayController;
        // Start is called before the first frame update
        void Start()
        {
            comboText.text = "0";
        }

        // Update is called once per frame
        void Update()
        {
            comboText.text = gamePlayController.Status.Combo.ToString();
        }

    }
}
