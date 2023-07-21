using UnityEngine;
using TMPro;
using Plutono.GamePlay;

namespace Plutono.UI
{
    public class TextCombo : MonoBehaviour
    {
        public TMP_Text comboText;

        // Start is called before the first frame update
        void Start()
        {
            comboText.text = "0";
        }

        // Update is called once per frame
        void Update()
        {
            //comboText.text = GamePlayController.Instance.Status.Combo.ToString();
        }

    }
}
